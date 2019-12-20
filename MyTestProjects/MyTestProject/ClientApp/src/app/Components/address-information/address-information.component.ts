import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-address-information',
  templateUrl: './address-information.component.html'
})
export class AddressInformationComponent implements OnChanges {
  @Input() addressInformation;
  public formatedTime : string;
  public formatedDate: string;

  constructor() {
    console.log(this.addressInformation);
  }

  ngOnChanges(changes: SimpleChanges) {
    this.getLocaTimeForTimeZone();
  }

  getLocaTimeForTimeZone() {
    var optionsDate = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    const timeZone = this.addressInformation.googleTimeZone.timeZoneId;
    const countryCode = this.addressInformation.countryCode;
    this.formatedDate = new Date().toLocaleString(countryCode, { timeZone, ...optionsDate });
    this.formatedTime = new Date().toLocaleTimeString(countryCode, { timeZone });
  }


}
