import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AddressDataResponce, AddressDataRequest } from "../Models/AddressDataResponce";

@Injectable()
export class LocationService {
  public forecasts: AddressDataResponce[];
  constructor(public http: HttpClient) {
  }

  public getLocation(address: AddressDataRequest) {
    const config = { headers: new HttpHeaders().set('Content-Type', 'application/json') };
    return this.http.post<AddressDataResponce[]>('address/', address, config);
  }
}
