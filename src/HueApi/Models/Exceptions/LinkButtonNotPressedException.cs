using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Models.Exceptions
{
  public class LinkButtonNotPressedException : HueException
  {
    public LinkButtonNotPressedException() { }
    public LinkButtonNotPressedException(string message) : base(message) { }
    public LinkButtonNotPressedException(string message, Exception inner) : base(message, inner) { }
  }
}
