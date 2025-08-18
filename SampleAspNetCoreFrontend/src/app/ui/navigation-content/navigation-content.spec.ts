import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavigationContent } from './navigation-content';

describe('NavigationContent', () => {
  let component: NavigationContent;
  let fixture: ComponentFixture<NavigationContent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavigationContent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavigationContent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
