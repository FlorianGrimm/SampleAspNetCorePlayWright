import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageGrid } from './page-grid';

describe('PageGrid', () => {
  let component: PageGrid;
  let fixture: ComponentFixture<PageGrid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PageGrid]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PageGrid);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
