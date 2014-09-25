using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
  public partial class HueClient
  {

   /// <summary>
    /// Get all Timezones
   /// </summary>
   /// <returns></returns>
    public async Task<IEnumerable<string>> GetTimeZonesAsync()
    {
      CheckInitialized();

      HttpClient client = new HttpClient();
      var result = await client.GetAsync(new Uri(String.Format("{0}info/timezones", ApiBase))).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

#if DEBUG
      //Normal result example
      jsonResult = "[\"Africa/Abidjan\",  \"Africa/Accra\",  \"Africa/Addis_Ababa\",  \"Africa/Algiers\",  \"Africa/Asmara\",  \"Africa/Asmera\",  \"Africa/Bamako\", \"Pacific/Wake\",  \"Pacific/Wallis\",  \"US/Pacific-New\"]";
#endif

      List<string> timezones = DeserializeResult<List<string>>(jsonResult);

      return timezones;
    }


  }
}
