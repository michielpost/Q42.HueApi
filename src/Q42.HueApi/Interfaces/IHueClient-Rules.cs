using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_Rules
  {
    Task<IReadOnlyCollection<Rule>> GetRulesAsync();
    Task<Rule?> GetRuleAsync(string id);
    Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteRule(string id);
    Task<string> CreateRule(Rule rule);
    Task<string?> CreateRule(string name, IEnumerable<RuleCondition> conditions, IEnumerable<InternalBridgeCommand> actions);
    Task<HueResults> UpdateRule(Rule rule);
    Task<HueResults> UpdateRule(string id, string name, IEnumerable<RuleCondition> conditions, IEnumerable<InternalBridgeCommand> actions);

  }
}
