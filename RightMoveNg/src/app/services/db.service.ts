import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject, tap } from 'rxjs';
import { ApiEndPointService } from './api-end-point.service';

@Injectable({
  providedIn: 'root'
})
export class DbService {

  constructor(private api: ApiEndPointService,
    private http: HttpClient) { }

  // constructor(private api: ApiEndPointService) { }

  tableName$: BehaviorSubject<string> = new BehaviorSubject<string>("");
  tables$: BehaviorSubject<string[]> = new BehaviorSubject<string[]>([]);
  properties$: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  public UpdateTables() {
    return this.http.get(this.api.tableList).pipe(
      tap(o => this.tables$.next(o as string[]))
    );
  }

  public UpdateProperties(tableName: string) {
    return this.http.get(this.api.getTable + '/' + tableName).pipe(
      tap(o => this.properties$.next(o)),
      tap(o => this.tableName$.next(tableName))
    )
  }
}
