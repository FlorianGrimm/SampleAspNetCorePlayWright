import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageAdministrationHome } from './page-administration-home';

describe('PageAdministrationHome', () => {
  let component: PageAdministrationHome;
  let fixture: ComponentFixture<PageAdministrationHome>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PageAdministrationHome]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PageAdministrationHome);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
