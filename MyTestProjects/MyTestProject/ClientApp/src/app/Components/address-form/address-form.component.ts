import { Component, SimpleChanges } from '@angular/core';
import { LocationService } from '../../Services/location.service';

@Component({
  selector: 'app-address-form',
  templateUrl: './address-form.component.html'
})
export class AddressFormComponent {

  public addressText = "";
  public invalidString = false;
  public addressResponce = {};
  constructor(private locationService: LocationService) {
  }

  public getLocation() {
    if (this.addressText) {
      this.invalidString = false;
      this.locationService.getLocation({ locationAddress: this.addressText }).subscribe(result => {
        this.addressResponce = result;
      });
    } else {
      this.invalidString = true;
    }
  }
}
