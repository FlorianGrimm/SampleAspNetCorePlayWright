import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageUser } from './page-user';

describe('PageUser', () => {
  let component: PageUser;
  let fixture: ComponentFixture<PageUser>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PageUser]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PageUser);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
