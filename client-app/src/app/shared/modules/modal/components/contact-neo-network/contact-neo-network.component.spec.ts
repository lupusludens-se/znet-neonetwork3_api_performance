import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactNeoNetworkComponent } from './contact-neo-network.component';

describe('ContactNeoNetworkComponent', () => {
  let component: ContactNeoNetworkComponent;
  let fixture: ComponentFixture<ContactNeoNetworkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ContactNeoNetworkComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactNeoNetworkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
