using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyTestProject.DTO
{
  [DataContract]
  class GeocodeResponse
  {
    [DataMember(Name = "status")]
    public string Status { get; set; }
    [DataMember(Name = "results")]
    public CResult[] Results { get; set; }

    [DataContract]
    public class CResult
    {
      [DataMember(Name = "address_components")]
      public AddressItems[] AddressComponents { get; set; }

      [DataMember(Name = "formatted_address")]
      public string FormatedAddress { get; set; }

      [DataMember(Name = "geometry")]
      public CGeometry Geometry { get; set; }

      [DataContract]
      public class CGeometry
      {
        [DataMember(Name = "location")]
        public CLocation Location { get; set; }

        [DataContract]
        public class CLocation
        {
          [DataMember(Name = "lat")]
          public double Lat { get; set; }
          [DataMember(Name = "lng")]
          public double Lng { get; set; }
        }
      }
    }

    [DataContract]
    public class AddressItems
    {
      [DataMember(Name = "long_name")]
      public string LongName { get; set; }
      [DataMember(Name = "short_name")]
      public string ShortName { get; set; }
      [DataMember(Name = "types")]
      public List<string> Types { get; set; }
    }
  }
}
