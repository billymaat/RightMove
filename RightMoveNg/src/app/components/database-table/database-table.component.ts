import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { DbService } from 'src/app/services/db.service';

// TODO: Replace this with your own data model type
export interface IRightMoveItem {
  name: string;
  id: number;
  price: number;
  prices: number[];
  houseInfo: string;
  address: string;
}

@Component({
  selector: 'app-database-table',
  templateUrl: './database-table.component.html',
  styleUrls: ['./database-table.component.scss']
})
export class DatabaseTableComponent implements AfterViewInit {
  @Input() items!: any;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<IRightMoveItem>;
  dataSource!: MatTableDataSource<IRightMoveItem>

  shouldFilter: boolean = false;
  showTerrace: boolean = true;

  filterValues = {
    filterByReduced: false,
    showTerrace: true
  };

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'houseInfo', 'address', 'lastUpdateDate', 'price'];

  constructor(private db: DbService) {
  }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<IRightMoveItem>(this.items);
    this.dataSource.filterPredicate = (data: IRightMoveItem, filterParams: string) => {
      let filt = JSON.parse(filterParams);
      return !(filt.filterByReduced && data.prices.length >= 2) &&
        !(!filt.showTerrace && data.houseInfo.indexOf("terrace") > -1)
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
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
    return row.price;

    console.log(row.price);
    var parts: string[] = row.price.split('|');
    // let ret = Number.parseInt(parts[parts.length - 1]);
    let ret = parts[parts.length - 1];
    return ret;
  }

  getPrices(row: IRightMoveItem) : number[] {
    return row.prices;
  }

  parseDate(str: string) : Date {
    var parts = str.split("/");
    var dt = new Date(parseInt(parts[2], 10),
    parseInt(parts[1], 10) - 1,
    parseInt(parts[0], 10));

    return dt;
  }

  filterChanged() {
    console.log("Filter changed: " + this.shouldFilter)
    this.applyFilter();
  }

  applyFilter() {
    this.filterValues.filterByReduced = this.shouldFilter;
    this.filterValues.showTerrace = this.showTerrace;

    this.dataSource.filter = JSON.stringify(this.filterValues);

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
