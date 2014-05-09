using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  public class ErrorResult
  {
    public Error Error { get; set; }
  }

  public class Error
  {
    public int Type { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
  }

}
