using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Groups
{
  public class Group
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Lights { get; set; }
    public LightCommand action { get; set; }
  }
}
