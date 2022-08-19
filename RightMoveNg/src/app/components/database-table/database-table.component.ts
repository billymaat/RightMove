import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { DbService } from 'src/app/services/db.service';
import { DatabaseTableDataSource, DatabaseTableItem } from './database-table-datasource';

@Component({
  selector: 'app-database-table',
  templateUrl: './database-table.component.html',
  styleUrls: ['./database-table.component.scss']
})
export class DatabaseTableComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<DatabaseTableItem>;
  dataSource!: DatabaseTableDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'houseInfo', 'address', 'lastUpdateDate', 'price'];

  constructor(private db: DbService) {
    this.dataSource = new DatabaseTableDataSource([]);

    db.properties$.subscribe(o => {
      this.dataSource = new DatabaseTableDataSource(o);
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }

  onDoubleClick(row: any) {
    // make link
    let link: string = `https://www.rightmove.co.uk/properties/${row.rightMoveId}`;
    window.open(link, "_blank");
    console.log(row);
  }

  getLastUpdateDate(row: any) : Date {
    let dateAdded: Date = this.parseDate(row.dateAdded);
    let dateReduced: Date = this.parseDate(row.dateReduced);

    return (dateReduced > dateAdded) 
      ? dateReduced
      : dateAdded;
  }

  getPrice(row: any): string {
    console.log(row.price);
    var parts: string[] = row.price.split('|');
    // let ret = Number.parseInt(parts[parts.length - 1]);
    let ret = parts[parts.length - 1];
    return ret;
  }

  parseDate(str: string) : Date {
    var parts = str.split("/");
    var dt = new Date(parseInt(parts[2], 10),
    parseInt(parts[1], 10) - 1,
    parseInt(parts[0], 10));

    return dt;
  }

}
