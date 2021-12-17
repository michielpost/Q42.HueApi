using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class HueResponse<T>
  {
    public T? Result { get; set; }

    public HueErrors Errors { get; set; } = new();

    public int StatusCodeResponse { get; set; }

    public bool HasErrors => !Errors.Any();


    public HueResponse(T? result, HueErrors? errors) : this(errors)
    {
      Result = result;
    }

    public HueResponse(HueErrors? errors)
    {
      if(errors != null)
        Errors = errors;
    }
  }
}
