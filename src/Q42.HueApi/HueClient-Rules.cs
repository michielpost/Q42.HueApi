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
using Q42.HueApi.Interfaces;

namespace Q42.HueApi
{
  /// <summary>
  /// Partial HueClient, contains requests to the /rules/ url
  /// </summary>
  public partial class HueClient : IHueClient_Rules
  {

    /// <summary>
    /// Asynchronously gets all rules registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Rule"/>s registered with the bridge.</returns>
    public async Task<IReadOnlyCollection<Rule>> GetRulesAsync()
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}rules", ApiBase))).ConfigureAwait(false);

//#if DEBUG
//			//stringResult = "{\"1\": {    \"name\": \"Wall Switch Rule\",    \"lasttriggered\": \"2013-10-17T01:23:20\",    \"creationtime\": \"2013-10-10T21:11:45\",    \"timestriggered\": 27,    \"owner\": \"78H56B12BA\",    \"status\": \"enabled\",    \"conditions\": [        {            \"address\": \"/sensors/2/state/buttonevent\",            \"operator\": \"eq\",            \"value\": \"16\"        },        {            \"address\": \"/sensors/2/state/lastupdated\",            \"operator\": \"dx\"        }    ],    \"actions\": [        {            \"address\": \"/groups/0/action\",            \"method\": \"PUT\",            \"body\": {                \"scene\": \"S3\"            }        }    ]} }";
//			stringResult = "{    \"29\": {        \"name\": \"MotionSensor 13.day-on\",        \"owner\": \"Ftg8b0E4NVk3vVpodvV-Nh-rg8q3XfB0sDUZv3Hy\",        \"created\": \"2016-10-15T12:01:33\",        \"lasttriggered\": \"2016-10-23T08:59:32\",        \"timestriggered\": 5,        \"status\": \"enabled\",        \"recycle\": true,        \"conditions\": [            {                \"address\": \"/config/localtime\",                \"operator\": \"in\",                \"value\": \"T08:00:00/T23:00:00\"            },            {                \"address\": \"/sensors/13/state/presence\",                \"operator\": \"eq\",                \"value\": \"true\"            },            {                \"address\": \"/sensors/13/state/presence\",                \"operator\": \"dx\"            },            {               \"address\": \"/sensors/15/state/status\",                \"operator\": \"lt\",                \"value\": \"1\"            },            {                \"address\": \"/sensors/14/state/dark\",                \"operator\": \"eq\",                \"value\": \"true\"            }        ],        \"actions\": [            {                \"address\": \"/groups/3/action\",                \"method\": \"PUT\",                \"body\": {                    \"scene\": \"kZvpr1yJTqvHyQA\"                }            },            {                \"address\": \"/sensors/15/state\",                \"method\": \"PUT\",                \"body\": {                    \"status\": 1                }            }        ]    }}";
//#endif


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
    public async Task<Rule?> GetRuleAsync(string id)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id can not be empty or a blank string", nameof(id));

      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}rules/{1}", ApiBase, id))).ConfigureAwait(false);

//#if DEBUG
//      stringResult = "{    \"name\": \"Wall Switch Rule\",    \"owner\": \"ruleOwner\",    \"created\": \"2014-07-23T15:02:56\",    \"lasttriggered\": \"none\",    \"timestriggered\": 0,    \"status\": \"enabled\",    \"conditions\": [        {            \"address\": \"/sensors/2/state/buttonevent\",            \"operator\": \"eq\",            \"value\": \"16\"        },        {            \"address\": \"/sensors/2/state/lastupdated\",            \"operator\": \"dx\"        }    ],    \"actions\": [        {            \"address\": \"/groups/0/action\",            \"method\": \"PUT\",            \"body\": {                \"scene\": \"S3\"            }        }    ]}";
//#endif

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Array)
      {
        // Hue gives back errors in an array for this request
        JToken? error = token.First?["error"];
        if (error != null)
        {
          if (error["type"]?.Value<int>() == 3) // Rule not found
            return null;

          var msg = error["description"]?.Value<string>();
          if(msg != null)
            throw new HueException(msg);
        }
      }

      var rule = token.ToObject<Rule>();
      if(rule != null)
        rule.Id = id;

      return rule;
    }

    public Task<string> CreateRule(Rule rule)
    {
      if(rule == null)
        throw new ArgumentNullException(nameof(rule));

      return CreateRule(rule.Name, rule.Conditions, rule.Actions);
    }

    public async Task<string?> CreateRule(string name, IEnumerable<RuleCondition> conditions, IEnumerable<InternalBridgeCommand> actions)
    {
      CheckInitialized();

      if (conditions == null || !conditions.Any())
        throw new ArgumentNullException(nameof(conditions));
      if (actions == null || !actions.Any())
        throw new ArgumentNullException(nameof(actions));

      if (conditions.Count() > 8)
        throw new ArgumentException("Max 8 conditions allowed", nameof(conditions));
      if (actions.Count() > 8)
        throw new ArgumentException("Max 8 actions allowed", nameof(actions));

      JObject jsonObj = new JObject();
      if (conditions != null && conditions.Any())
        jsonObj.Add("conditions", JToken.FromObject(conditions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }));
      if (actions != null && actions.Any())
        jsonObj.Add("actions", JToken.FromObject(actions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }));

      if (!string.IsNullOrEmpty(name))
        jsonObj.Add("name", name);

      string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      //Create group with the lights we want to target
      var response = await client.PostAsync(new Uri(String.Format("{0}rules", ApiBase)), new JsonContent(jsonString)).ConfigureAwait(false);
      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      HueResults rulesResult = DeserializeDefaultHueResult(jsonResult);

      if (rulesResult.Count > 0 && rulesResult[0].Success != null && !string.IsNullOrEmpty(rulesResult[0].Success.Id))
      {
        return rulesResult[0].Success.Id;
      }

      if (rulesResult.HasErrors())
        throw new HueException(rulesResult.Errors.First().Error.Description);

      return null;
    }

    public Task<HueResults> UpdateRule(Rule rule)
    {
      if (rule == null)
        throw new ArgumentNullException(nameof(rule));

      return UpdateRule(rule.Id, rule.Name, rule.Conditions, rule.Actions);
    }

    public async Task<HueResults> UpdateRule(string id, string name, IEnumerable<RuleCondition> conditions, IEnumerable<InternalBridgeCommand> actions)
    {
      CheckInitialized();

      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", nameof(id));


      JObject jsonObj = new JObject();
      if (conditions != null && conditions.Any())
        jsonObj.Add("conditions", JToken.FromObject(conditions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }));
      if (actions != null && actions.Any())
        jsonObj.Add("actions", JToken.FromObject(actions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }));

      if (!string.IsNullOrEmpty(name))
        jsonObj.Add("name", name);

      string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var response = await client.PutAsync(new Uri(String.Format("{0}rules/{1}", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);

      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    /// <summary>
    /// Deletes a rule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteRule(string id)
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var result = await client.DeleteAsync(new Uri(ApiBase + string.Format("rules/{0}", id))).ConfigureAwait(false);

      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult<DeleteDefaultHueResult>(jsonResult);

    }

  }
}
