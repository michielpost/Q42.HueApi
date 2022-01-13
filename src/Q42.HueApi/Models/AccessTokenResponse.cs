using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  public class AccessTokenResponse
  {
    public DateTimeOffset CreatedDate { get; set; }

    public string Access_token { get; set; }
    public int Expires_in { get; set; }
    public string Refresh_token { get; set; }
    public string Token_type { get; set; }

    public AccessTokenResponse()
    {
      CreatedDate = DateTimeOffset.UtcNow;
    }
    public DateTimeOffset AccessTokenExpireTime()
    {
      return CreatedDate.AddSeconds(Expires_in);
    }
  }
}
