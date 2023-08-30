using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Models.Requests.Interface
{
  public interface IUpdateDynamics
  {
    public Dynamics? Dynamics { get; set; }
  }
}
