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

    /// <summary>
    /// Lights property only filled when getting a single group
    /// </summary>
    public List<string> Lights { get; set; }
    
    /// <summary>
    /// Action property only filled when getting a single group
    /// </summary>
    public LightCommand Action { get; set; }
  }
}
