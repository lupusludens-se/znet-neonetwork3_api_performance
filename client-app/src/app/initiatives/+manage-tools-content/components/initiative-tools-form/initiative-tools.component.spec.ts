import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InitiativeToolsComponent } from './initiative-tools.component';


describe('InitiativeToolsComponent', () => {
  let component: InitiativeToolsComponent;
  let fixture: ComponentFixture<InitiativeToolsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeToolsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeToolsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
