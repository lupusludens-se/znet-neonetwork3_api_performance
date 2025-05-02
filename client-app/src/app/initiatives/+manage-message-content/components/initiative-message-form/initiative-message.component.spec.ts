import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InitiativeMessageComponent } from './initiative-message.component';


describe('InitiativeMessageComponent', () => {
  let component: InitiativeMessageComponent;
  let fixture: ComponentFixture<InitiativeMessageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeMessageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
