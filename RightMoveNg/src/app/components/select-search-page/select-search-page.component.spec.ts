import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectSearchPageComponent } from './select-search-page.component';

describe('SelectSearchPageComponent', () => {
  let component: SelectSearchPageComponent;
  let fixture: ComponentFixture<SelectSearchPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectSearchPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectSearchPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
