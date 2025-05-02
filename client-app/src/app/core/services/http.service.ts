import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import structuredClone from '@ungap/structured-clone';
import { encode } from 'html-entities';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(private readonly httpClient: HttpClient) {}

  getAny<T>(url: string): Observable<T> {
    return this.httpClient.get<T>(url);
  }

  get<T>(url: string, params?: any): Observable<T> {
    //firewall blocks get requests with encoded html
    const encodeParams = <any>this.encodeBody(structuredClone(params));
    return this.httpClient.get<T>(`${environment.apiUrl}/${url}`, { params: encodeParams });
  }

  post<T>(url: string, data?, params?: any, encodeBody: boolean = true): Observable<T> {
    const body = encodeBody && environment.encodeUri ? this.encodeBody(structuredClone(data)) : data;

    return this.httpClient.post<T>(`${environment.apiUrl}/${url}`, body, {
      params
    });
  }

  put<T>(url: string, data?: T, params?: any, encodeBody: boolean = true): Observable<T> {
    const body = encodeBody && environment.encodeUri ? this.encodeBody(structuredClone(data)) : data;
    return this.httpClient.put<T>(`${environment.apiUrl}/${url}`, body, {
      params
    });
  }

  patch<T>(url: string, data?: T, params?: any): Observable<T> {
    return this.httpClient.patch<T>(`${environment.apiUrl}/${url}`, { ...data }, { params });
  }

  delete<T>(url: string, params?: any, body?: any): Observable<T> {
    return this.httpClient.delete<T>(`${environment.apiUrl}/${url}`, {
      params,
      body
    });
  }

  download(url: string, params?: any): Observable<unknown> {
    return this.httpClient.get(`${environment.apiUrl}/${url}`, {
      responseType: 'blob',
      params
    });
  }

  downloadFile(url: string, params?: any): Observable<unknown> {
    return this.httpClient.post(`${environment.apiUrl}/${url}`, { params }, { responseType: 'blob' });
  }

  private encodeBody(body): unknown {
    if (!body) return;

    if (Array.isArray(body)) {
      body.map(element => this.encodeBody(element));
    } else {
      Object.keys(body).forEach(key => {
        if (!body[key]) return;

        if (typeof body[key] === 'string') {
          if (this.includesHtmlTags(body[key]) || this.includesForbiddenSymbols(body[key])) {
            body[key] = encodeURIComponent(encode(body[key]));
          } else if (
            body[key]?.includes('http:') ||
            body[key]?.includes('https:') ||
            body[key]?.includes('ftp:') ||
            body[key]?.includes('www.')
          ) {
            body[key] = encodeURIComponent(body[key]);
          }
        } else if (typeof body[key] === 'object' && !Array.isArray(body[key])) {
          body[key] = this.encodeBody(body[key]);
        } else if (Array.isArray(body[key])) {
          body[key].map(element => this.encodeBody(element));
        }
      });
    }

    return body;
  }

  private includesHtmlTags(text: string): boolean {
    return text && /(<)|(>)|(&lt;)|(&gt;)/g.test(text);
  }

  private includesForbiddenSymbols(text: string): boolean {
    return text && /\+/g.test(text);
  }
}
