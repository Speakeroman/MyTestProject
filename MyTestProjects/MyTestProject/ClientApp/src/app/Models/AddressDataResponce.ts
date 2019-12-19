export interface AddressDataResponce {
  countryCode: string;
  latitude: number;
  longitude: number;
  formattedAddress: string;
  googleTimeZone: any;
}

export interface AddressDataRequest {
  locationAddress: string;
}
