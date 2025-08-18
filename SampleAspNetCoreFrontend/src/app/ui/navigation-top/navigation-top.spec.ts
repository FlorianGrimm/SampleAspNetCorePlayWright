import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavigationTop } from './navigation-top';

describe('NavigationTop', () => {
  let component: NavigationTop;
  let fixture: ComponentFixture<NavigationTop>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavigationTop]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavigationTop);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
