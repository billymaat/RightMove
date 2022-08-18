import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { DbService } from 'src/app/services/db.service';

@Component({
  selector: 'app-database-table-page',
  templateUrl: './database-table-page.component.html',
  styleUrls: ['./database-table-page.component.scss']
})
export class DatabaseTablePageComponent implements OnInit {

  tableName$: Observable<string>;
  constructor(private db: DbService) { 
    this.tableName$ = db.tableName$;
  }

  ngOnInit(): void {
  }

}
