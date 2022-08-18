import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseTablePageComponent } from './database-table-page.component';

describe('DatabaseTablePageComponent', () => {
  let component: DatabaseTablePageComponent;
  let fixture: ComponentFixture<DatabaseTablePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DatabaseTablePageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseTablePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
