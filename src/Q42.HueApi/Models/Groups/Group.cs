using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Groups
{
	[DataContract]
	public class Group
	{
		[DataMember]
		public string Id { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Luminaire / Lightsource / LightGroup
		/// </summary>
		[JsonConverter(typeof(StringNullableEnumConverter))]
		[DataMember(Name = "type")]
		public GroupType? Type { get; set; }

		/// <summary>
		/// Category of the Room type. Default is "Other".
		/// </summary>
		[JsonConverter(typeof(StringNullableEnumConverter))]
		[DataMember(Name = "class")]
		public RoomClass? Class { get; set; }

		/// <summary>
		/// As of 1.4. Uniquely identifies the hardware model of the luminaire. Only present for automatically created Luminaires.
		/// </summary>
		[DataMember(Name = "modelid")]
		public string ModelId { get; set; }

		/// <summary>
		/// The IDs of the lights that are in the group.
		/// </summary>
		[DataMember(Name = "lights")]
		public List<string> Lights { get; set; }

		/// <summary>
		/// The light state of one of the lamps in the group.
		/// </summary>
		[DataMember(Name = "action")]
		public State Action { get; set; }

		[DataMember(Name = "state")]
		public GroupState State { get; set; }

    [DataMember(Name = "locations")]
    public Dictionary<string, LightLocation> Locations { get; set; }

    [DataMember(Name = "recycle")]
    public bool? Recycle { get; set; }

    [DataMember(Name = "stream")]
    public Stream Stream { get; set; }

    [DataMember(Name = "sensors")]
    public List<string> Sensors { get; set; }

    [DataMember(Name = "presence")]
    public GroupPresence Presence { get; set; }

    [DataMember(Name = "lightlevel")]
    public GroupLightLevel LightLevel { get; set; }

    /// <summary>
    /// Overrides ToString() to give something more useful than object name.
    /// </summary>
    /// <returns>A string like "Group 1: Living"</returns>
    public override string ToString()
    {
      return String.Format("Group {0}: {1}", Id, Name);
    }
  }

  public class GroupPresence
  {
    [DataMember(Name = "State")]
    public State State { get; set; }

    [JsonProperty("lastupdated")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? Lastupdated { get; set; }

    [JsonProperty("presence")]
    public bool Presence { get; set; }

    [JsonProperty("presence_all")]
    public bool PresenceAll { get; set; }
  }

  public class GroupLightLevel
  {
    [DataMember(Name = "State")]
    public SensorState State { get; set; }

    [JsonProperty("lastupdated")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? Lastupdated { get; set; }

    [JsonProperty("dark")]
    public bool Dark { get; set; }

    [JsonProperty("dark_all")]
    public bool DarkAll { get; set; }

    [JsonProperty("daylight")]
    public bool Daylight { get; set; }

    [JsonProperty("daylight_any")]
    public bool DaylightAny { get; set; }

    [JsonProperty("lightlevel")]
    public int LightLevel { get; set; }

    [JsonProperty("lightlevel_min")]
    public int LightLevelMin { get; set; }

    [JsonProperty("lightlevel_max")]
    public int LightLevelMax { get; set; }
  }

  public class LightLocation : List<double>
  {
    [JsonIgnore]
    public double X
    {
      get { return this[0];  }
      set { this[0] = value; }
    }

    [JsonIgnore]
    public double Y
    {
      get { return this[1]; }
      set { this[1] = value; }
    }

    [JsonIgnore]
    public double Z
    {
      get { return this[2]; }
      set { this[2] = value; }
    }

    /// <summary>
    /// Default constructor used for json deserialization
    /// </summary>
    public LightLocation()
    {
    }

    public LightLocation(double x, double y, double z)
    {
      this.Add(x);
      this.Add(y);
      this.Add(z);
    }

    public bool IsLeft => X <= 0; //Include 0 with left
    public bool IsRight => X > 0;
    public bool IsFront => Y >= 0; //Include 0 with front
    public bool IsBack => Y < 0;
    public bool IsTop => Z >= 0; //Include 0 with top
    public bool IsBottom => Z < 0;


    /// <summary>
    /// X > -0.1 && X < 0.1
    /// </summary>
    public bool IsCenter => X > -0.1 && X < 0.1 ;

    public double Distance(double x, double y, double z)
    {
      var x2 = this.X;
      var y2 = this.Y;
      var z2 = this.Z;

      return Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2) + Math.Pow(z - z2, 2));
    }

    public double Angle(double x, double y)
    {
      var lengthX = this.X - x;
      var lengthY = this.Y - y;

      return (Math.Atan2(lengthY, lengthX) * (180 / Math.PI)) + 180;
    }

  }

  [DataContract]
	public class GroupState
	{
		[DataMember(Name = "any_on")]
		public bool? AnyOn { get; set; }

		[DataMember(Name = "all_on")]
		public bool? AllOn { get; set; }
	}

	/// <summary>
	/// Possible group types
	/// </summary>
	public enum GroupType
	{
		
		[EnumMember(Value = "LightGroup")]
		LightGroup,
		[EnumMember(Value = "Room")]
		Room,
		[EnumMember(Value = "Luminaire")]
		Luminaire,
		[EnumMember(Value = "LightSource")]
		LightSource,
    [EnumMember(Value = "Entertainment")]
    Entertainment,
    [EnumMember(Value = "Zone")]
    Zone
  }

	/// <summary>
	/// Possible room types
	/// </summary>
	public enum RoomClass
	{
		[EnumMember(Value = "Other")]
		Other,
		[EnumMember(Value = "Living room")]
		LivingRoom,
		[EnumMember(Value = "Kitchen")]
		Kitchen,
		[EnumMember(Value = "Dining")]
		Dining,
		[EnumMember(Value = "Bedroom")]
		Bedroom,
		[EnumMember(Value = "Kids bedroom")]
		KidsBedroom,
		[EnumMember(Value = "Bathroom")]
		Bathroom,
		[EnumMember(Value = "Nursery")]
		Nursery,
		[EnumMember(Value = "Recreation")]
		Recreation,
		[EnumMember(Value = "Office")]
		Office,
		[EnumMember(Value = "Gym")]
		Gym,
		[EnumMember(Value = "Hallway")]
		Hallway,
		[EnumMember(Value = "Toilet")]
		Toilet,
		[EnumMember(Value = "Front door")]
		FrontDoor,
		[EnumMember(Value = "Garage")]
		Garage,
		[EnumMember(Value = "Terrace")]
		Terrace,
		[EnumMember(Value = "Garden")]
		Garden,
		[EnumMember(Value = "Driveway")]
		Driveway,
		[EnumMember(Value = "Carport")]
		Carport,
    [EnumMember(Value = "TV")]
    TV,
    [EnumMember(Value = "Home")]
    Home,
    [EnumMember(Value = "Downstairs")]
    Downstairs,
    [EnumMember(Value = "Upstairs")]
    Upstairs,
    [EnumMember(Value = "Top floor")]
    TopFloor,
    [EnumMember(Value = "Attic")]
    Attic,
    [EnumMember(Value = "Guest room")]
    GuestRoom,
    [EnumMember(Value = "Staircase")]
    Staircase,
    [EnumMember(Value = "Lounge")]
    Lounge,
    [EnumMember(Value = "Man cave")]
    ManCave,
    [EnumMember(Value = "Computer")]
    Computer,
    [EnumMember(Value = "Studio")]
    Studio,
    [EnumMember(Value = "Music")]
    Music,
    [EnumMember(Value = "Reading")]
    Reading,
    [EnumMember(Value = "Closet")]
    Closet,
    [EnumMember(Value = "Storage")]
    Storage,
    [EnumMember(Value = "Laundry room")]
    LaundryRoom,
    [EnumMember(Value = "Balcony")]
    Balcony,
    [EnumMember(Value = "Porch")]
    Porch,
    [EnumMember(Value = "Barbecue")]
    Barbecue,
    [EnumMember(Value = "Pool")]
    Pool
  }

  [DataContract]
  public class Stream
  {
		[DataMember(Name = "proxymode")]
    public string ProxyMode { get; set; }
		[DataMember(Name = "proxynode")]
    public string ProxyNode { get; set; }
		[DataMember(Name = "active")]
    public bool Active { get; set; }
		[DataMember(Name = "owner")]
    public string Owner { get; set; }
  }

}
