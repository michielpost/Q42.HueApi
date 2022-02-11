using Newtonsoft.Json;
using System.Collections.Generic;

namespace Q42.HueApi.Models
{
	public class ResourceLink
	{
		public ResourceLink()
		{
			Links = new List<string>();
		}

		public string Id { get; set; }

		/// <summary>
		/// Human readable name for this resourcelink
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Human readable description of what this resourcelink does. If not specified it's set to "".
		/// </summary>
		[JsonProperty("description")]
		public string Description { get; set; }

		/// <summary>
		/// Not writeable and there is only 1 type: "Link"
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }

		/// <summary>
		/// Class of resourcelink given by application. The resourcelink class can be used to identify resourcelink with the same purpose, like classid 1 for wake-up, 2 for going to sleep, etc. (best practice use range 1 - 10000)
		/// </summary>
		[JsonProperty("classid")]
		public int ClassId { get; set; }

		/// <summary>
		/// Not writeable, this respresents the owner (username) of the creator of the resourcelink
		/// </summary>
		[JsonProperty("owner")]
		public string Owner { get; set; }

		/// <summary>
		/// When true: Resource is automatically deleted when not referenced anymore in any resource link. Only on creation of resourcelink. "false" when omitted.
		/// </summary>
		[JsonProperty("recycle")]
		public bool? Recycle { get; set; }

		/// <summary>
		/// References to resources which are used by this resourcelink resource. In case the referenced resource was created with "recycle":true and no other references are present, the resourcelink resource will be automatically deleted when removed when empty.
		/// Allowed resources paths(given as ASCII String with pattern: "/<resource>/<resource id>":
		/// </summary>
		[JsonProperty("links")]
		public List<string> Links { get; set; }

	}
}
