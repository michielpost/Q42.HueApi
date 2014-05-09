using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Groups
{
  public class DefaultHueResult
  {
    public SuccessResult Success { get; set; }
    public ErrorResult Error { get; set; }
  }

  public class SuccessResult
  {
    public string Id { get; set; }
  }

}
