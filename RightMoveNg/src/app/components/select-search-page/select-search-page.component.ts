import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DbService } from 'src/app/services/db.service';

@Component({
  selector: 'app-select-search-page',
  templateUrl: './select-search-page.component.html',
  styleUrls: ['./select-search-page.component.scss']
})
export class SelectSearchPageComponent implements OnInit {

  tables$: Observable<string[]>;

  constructor(private dbService: DbService,
    private router: Router) {
    this.tables$ = dbService.tables$;
  }

  ngOnInit(): void {
    this.dbService.UpdateTables().subscribe(o => 
      console.log("Updated tables")
    )
  }

  onClick(s: string) {
    // this.router.navigate()
    console.log(s);
    this.dbService.UpdateProperties(s).subscribe((o: any) => {
      this.router.navigate(['rightmovetable'])
    })
  }
}
