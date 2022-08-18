import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DatabaseTablePageComponent } from './components/database-table-page/database-table-page.component';
import { MainComponent } from './components/main/main.component';

const routes: Routes = [
  { 
    path: '', 
    component: DatabaseTablePageComponent 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
