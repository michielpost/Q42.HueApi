using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Models.Requests.Interface
{
  public interface IUpdateDimming
  {
    public Dimming? Dimming { get; set; }
  }
}
