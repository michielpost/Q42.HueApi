using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Newtonsoft.Json;
using Q42.HueApi.Models.Groups;
using Q42.HueApi.Models;
using System.Dynamic;

namespace Q42.HueApi
{
  /// <summary>
  /// Partial HueClient, contains requests to the /rules/ url
  /// </summary>
  public partial class HueClient
  {

    /// <summary>
    /// Asynchronously gets all rules registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Rule"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<Rule>> GetRulesAsync()
    {
      CheckInitialized();

      HttpClient client = new HttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}rules", ApiBase))).ConfigureAwait(false);

#if DEBUG
      stringResult = "{\"1\": {    \"name\": \"Wall Switch Rule\",    \"lasttriggered\": \"2013-10-17T01:23:20\",    \"creationtime\": \"2013-10-10T21:11:45\",    \"timestriggered\": 27,    \"owner\": \"78H56B12BA\",    \"status\": \"enabled\",    \"conditions\": [        {            \"address\": \"/sensors/2/state/buttonevent\",            \"operator\": \"eq\",            \"value\": \"16\"        },        {            \"address\": \"/sensors/2/state/lastupdated\",            \"operator\": \"dx\"        }    ],    \"actions\": [        {            \"address\": \"/groups/0/action\",            \"method\": \"PUT\",            \"body\": {                \"scene\": \"S3\"            }        }    ]} }";
#endif


      List<Rule> results = new List<Rule>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a scene
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          Rule rule = JsonConvert.DeserializeObject<Rule>(prop.Value.ToString());
          rule.Id = prop.Name;

          results.Add(rule);
        }

      }

      return results;

    }

    /// <summary>
    /// Asynchronously gets single rule
    /// </summary>
    /// <returns><see cref="Rule"/></returns>
    public async Task<Rule> GetRuleAsync(string id)
    {
      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id can not be empty or a blank string", "id");

      CheckInitialized();

      HttpClient client = new HttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}rules/{1}", ApiBase, id))).ConfigureAwait(false);

#if DEBUG
      stringResult = "{    \"name\": \"Wall Switch Rule\",    \"owner\": \"ruleOwner\",    \"created\": \"2014-07-23T15:02:56\",    \"lasttriggered\": \"none\",    \"timestriggered\": 0,    \"status\": \"enabled\",    \"conditions\": [        {            \"address\": \"/sensors/2/state/buttonevent\",            \"operator\": \"eq\",            \"value\": \"16\"        },        {            \"address\": \"/sensors/2/state/lastupdated\",            \"operator\": \"dx\"        }    ],    \"actions\": [        {            \"address\": \"/groups/0/action\",            \"method\": \"PUT\",            \"body\": {                \"scene\": \"S3\"            }        }    ]}";
#endif

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Array)
      {
        // Hue gives back errors in an array for this request
        JObject error = (JObject)token.First["error"];
        if (error["type"].Value<int>() == 3) // Rule not found
          return null;

        throw new Exception(error["description"].Value<string>());
      }

      var rule = token.ToObject<Rule>();
      rule.Id = id;

      return rule;
    }

  }
}
