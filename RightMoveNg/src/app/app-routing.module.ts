import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DatabaseTablePageComponent } from './components/database-table-page/database-table-page.component';
import { HomeComponent } from './components/home/home.component';
import { MainComponent } from './components/main/main.component';
import { SelectSearchPageComponent } from './components/select-search-page/select-search-page.component';

const routes: Routes = [
  { 
    path: '', 
    component: SelectSearchPageComponent 
  },
  {
    path: 'select',
    component: SelectSearchPageComponent
  },
  {
    path: 'rightmovetable',
    component: DatabaseTablePageComponent
  },
  {
    path: 'home',
    component: HomeComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
