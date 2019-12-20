import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AddressFormComponent } from './address-form.component';

describe('CounterComponent', () => {
  let component: AddressFormComponent;
  let fixture: ComponentFixture<AddressFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AddressFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should display a title', async(() => {
    const titleText = fixture.nativeElement.querySelector('h1').textContent;
    expect(titleText).toEqual('Address');
  }));

  it('should start with count 0, then increments by 1 when clicked', async(() => {
    const addressInformation = fixture.nativeElement.querySelector('app-address-information');
    expect(addressInformation).toBeNull;

    const incrementButton = fixture.nativeElement.querySelector('button');
    incrementButton.click();
    fixture.detectChanges();
    expect(addressInformation).toBeDefined;
  }));
});
