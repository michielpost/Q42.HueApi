using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
    /// <summary>
    /// Partial HueClient, contains requests to the /config/ url
    /// </summary>
    public partial class HueClient : IHueClient_Capabilities
  {

        /// <summary>
        /// Get bridge capabilities
        /// </summary>
        /// <returns></returns>
        public async Task<BridgeCapabilities?> GetCapabilitiesAsync()
        {
            CheckInitialized();

            HttpClient client = await GetHttpClient().ConfigureAwait(false);
            var stringResult = await client.GetStringAsync(new Uri(ApiBase + "capabilities")).ConfigureAwait(false);

            BridgeCapabilities? capabilities = DeserializeResult<BridgeCapabilities>(stringResult);

            return capabilities;
        }

    }
}
