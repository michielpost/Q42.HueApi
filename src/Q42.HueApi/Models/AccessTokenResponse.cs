using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
    public class AccessTokenResponse
    {
        private DateTimeOffset CreatedDate = DateTimeOffset.UtcNow;

        public string Access_token { get; set; }
        public int Access_token_expires_in { get; set; }
        public string Refresh_token { get; set; }
        public int Refresh_token_expires_in { get; set; }
        public string Token_type { get; set; }

        public DateTimeOffset AccessTokenExpireTime()
        {
            return CreatedDate.AddSeconds(Access_token_expires_in);
        }

        public DateTimeOffset RefreshTokenExpireTime()
        {
            return CreatedDate.AddSeconds(Refresh_token_expires_in);
        }

    }
}
