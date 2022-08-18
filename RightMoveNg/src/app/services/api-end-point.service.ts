import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiEndPointService {

  constructor() { }

  createApi(api: string) : string {
    return `http://localhost:5201/${api}`
  }

  private _getTableList: string = "api/property/tables";
  private _getTable: string = "api/property/gettable";

  get tableList() {
    return this.createApi(this._getTableList);
  }

  get getTable() {
    return this.createApi(this._getTable);
  }
}
