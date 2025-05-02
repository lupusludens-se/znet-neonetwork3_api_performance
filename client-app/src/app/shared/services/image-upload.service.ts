import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';

import { HttpService } from 'src/app/core/services/http.service';
import { GeneralApiEnum } from '../enums/api/general-api.enum';
import { ImageInterface } from '../interfaces/image.interface';
import { SnackbarService } from 'src/app/core/services/snackbar.service';

@Injectable()
export class ImageUploadService {
  apiRoutes = GeneralApiEnum;

  constructor(private httpService: HttpService, private readonly snackbarService: SnackbarService) {}

  renderPreview(event): Promise<string> {
    if (event.target.files.length > 0) {
      return new Promise((resolve, reject) => {
        let imageSrc: string;
        const file = (event.target as HTMLInputElement).files[0];
        const reader = new FileReader();

        reader.onload = () => {
          // eslint-disable-next-line
          resolve((imageSrc = reader.result as string));
        };
        reader.onerror = reject;
        reader.readAsDataURL(file);
      });
    }
  }

  uploadImage(blobType: number, file: FormData): Observable<ImageInterface> {
    return this.httpService
      .post<ImageInterface>(
        this.apiRoutes.Media,
        file,
        {
          BlobType: blobType
        },
        false
      )
      .pipe(
        catchError(() => {
          this.snackbarService.showError('general.defaultImageUploadErrorLabel');
          return of(null); // Return a null observable to continue the stream
        })
      );
  }
}
