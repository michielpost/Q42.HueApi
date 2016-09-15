using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
	public class RemoteAuthorizeResponse
	{
		public string Code { get; set; }

		/// <summary>
		/// Roundtrip parameter to validate the response
		/// </summary>
		public string State { get; set; }
	}
}
