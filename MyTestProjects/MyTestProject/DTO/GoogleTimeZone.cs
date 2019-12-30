using System.Runtime.Serialization;

namespace MyTestProject.DTO
{
  [DataContract]
  public class GoogleTimeZone
    {
    [DataMember(Name = "dstOffset")]
    public double DstOffset { get; set; }
    [DataMember(Name = "rawOffset")]
    public double RawOffset { get; set; }
    [DataMember(Name = "status")]
    public string Status { get; set; }
    [DataMember(Name = "timeZoneId")]
    public string TimeZoneId { get; set; }
    [DataMember(Name = "timeZoneName")]
    public string TimeZoneName { get; set; }
    }
}
