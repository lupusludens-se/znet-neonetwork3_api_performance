import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationLoaderComponent } from './notification-loader.component';

describe('NotificationLoaderComponent', () => {
  let component: NotificationLoaderComponent;
  let fixture: ComponentFixture<NotificationLoaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NotificationLoaderComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
