using HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Extensions.cs
{
  public static class HueResourceExtensions
  {
    public static ResourceIdentifier ToResourceIdentifier(this HueResource hueResource)
    {
      return new ResourceIdentifier
      {
        Rid = hueResource.Id,
        Rtype = hueResource.Type
      };
    }
  }
}
