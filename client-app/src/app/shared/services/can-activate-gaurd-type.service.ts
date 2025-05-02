import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CanActivateGaurdTypeService {
  private canActivateGaurdType: string;
  constructor() { }

  setCanActivateGaurdType(type: string) {
    this.canActivateGaurdType = type;
  }

  getCanActivateGaurdType() {
    return this.canActivateGaurdType;
  }
}
