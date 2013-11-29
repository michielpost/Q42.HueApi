using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Groups
{
  public class DefaultPutResult
  {
    public SuccessResult Success { get; set; }
    public ErrorResult Error { get; set; }
  }

  public class SuccessResult
  {
    public string Id { get; set; }
  }

  public class ErrorResult
  {
    public int Type { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
  }



}
