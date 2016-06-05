using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
	[DataContract]
	public class ResourceLink
	{
		public ResourceLink()
		{
			Links = new List<string>();
		}

		[DataMember]
		public string Id { get; set; }

		/// <summary>
		/// Human readable name for this resourcelink
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Human readable description of what this resourcelink does. If not specified it's set to "".
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		/// Not writeable and there is only 1 type: "Link"
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		/// Class of resourcelink given by application. The resourcelink class can be used to identify resourcelink with the same purpose, like classid 1 for wake-up, 2 for going to sleep, etc. (best practice use range 1 - 10000)
		/// </summary>
		[DataMember(Name = "classid")]
		public int ClassId { get; set; }

		/// <summary>
		/// Not writeable, this respresents the owner (username) of the creator of the resourcelink
		/// </summary>
		[DataMember(Name = "owner")]
		public string Owner { get; set; }

		/// <summary>
		/// When true: Resource is automatically deleted when not referenced anymore in any resource link. Only on creation of resourcelink. "false" when omitted.
		/// </summary>
		[DataMember(Name = "recycle")]
		public bool? Recycle { get; set; }

		/// <summary>
		/// References to resources which are used by this resourcelink resource. In case the referenced resource was created with "recycle":true and no other references are present, the resourcelink resource will be automatically deleted when removed when empty.
		/// Allowed resources paths(given as ASCII String with pattern: "/<resource>/<resource id>":
		/// </summary>
		[DataMember(Name = "links")]
		public List<string> Links { get; set; }

	}
}
