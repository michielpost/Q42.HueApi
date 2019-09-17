using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Groups
{

  /// <summary>
  /// A PUT or POST returns a list which can contain multiple success and errors
  /// </summary>
  public class HueResults : List<DefaultHueResult>
  {

    public bool HasErrors()
    {
      return this.Where(x => x.Error != null).Any();
    }

    public IEnumerable<DefaultHueResult> Errors
    {
      get
      {
        return this.Where(x => x.Error != null);
      }
    }
    
    
  }

  public class DefaultHueResult
  {
    public SuccessResult Success { get; set; }
    public ErrorResult Error { get; set; }
  }

  public class DeleteDefaultHueResult
  {
    public string Success { get; set; }
    public ErrorResult Error { get; set; }
  }


  public class SuccessResult
  {
    public string Id { get; set; }

    [JsonExtensionData]
#pragma warning disable IDE0044 // Add readonly modifier
    private IDictionary<string, JToken> OtherData;
#pragma warning restore IDE0044 // Add readonly modifier

  }

  public class ErrorResult
  {
    public int Type { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
  }

}
