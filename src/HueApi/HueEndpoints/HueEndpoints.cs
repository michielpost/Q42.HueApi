using HueApi.Models;

namespace HueApi.HueEndpoints
{
  public abstract class EndpointBase
  {
    protected readonly BaseHueApi _hueApi;
    protected readonly string _type;

    protected EndpointBase(BaseHueApi hueApi, string type)
    {
      _hueApi = hueApi;
      _type = type;
    }

  }

  public class ReadOnlyListEndpoint<TGet>(BaseHueApi hueApi, string type) : EndpointBase(hueApi, type),
    IGetAllEndpoint<TGet>
    where TGet : HueResource
  {
    public Task<HueResponse<TGet>> GetAllAsync() => _hueApi.HueGetRequestAsync<TGet>(_hueApi.ResourceTypeIdUrl(_type, null));
  }

  public class ReadOnlyEndpoint<TGet>(BaseHueApi hueApi, string type) : ReadOnlyListEndpoint<TGet>(hueApi, type),
    IGetAllEndpoint<TGet>,
    IGetByIdEndpoint<TGet>
    where TGet : HueResource
  {
    public Task<HueResponse<TGet>> GetByIdAsync(Guid id) => _hueApi.HueGetRequestAsync<TGet>(_hueApi.ResourceTypeIdUrl(_type, id));
  }

  public class ReadCreateEndpoint<TGet, TPost>(BaseHueApi hueApi, string type) : ReadOnlyEndpoint<TGet>(hueApi, type),
    IGetAllEndpoint<TGet>,
    IGetByIdEndpoint<TGet>,
    IPostEndpoint<TPost>
    where TGet : HueResource
  {
    public Task<HuePostResponse> CreateAsync(TPost data) => _hueApi.HuePostRequestAsync<TPost>(_hueApi.ResourceTypeIdUrl(_type), data);
  }

  public class ReadEditEndpoint<TGet, TPut>(BaseHueApi hueApi, string type) : ReadOnlyEndpoint<TGet>(hueApi, type),
    IGetAllEndpoint<TGet>,
    IGetByIdEndpoint<TGet>,
    IPutEndpoint<TPut>
    where TGet : HueResource
  {
    public Task<HuePutResponse> UpdateAsync(Guid id, TPut data) => _hueApi.HuePutRequestAsync<TPut>(_hueApi.ResourceTypeIdUrl(_type, id), data);
  }

  public class CrudWithoutCreateEndpoint<TGet, TPut>(BaseHueApi hueApi, string type) : ReadOnlyEndpoint<TGet>(hueApi, type),
    IGetAllEndpoint<TGet>,
    IGetByIdEndpoint<TGet>,
    IPutEndpoint<TPut>,
    IDeleteEndpoint
    where TGet : HueResource
  {
    public Task<HuePutResponse> UpdateAsync(Guid id, TPut data) => _hueApi.HuePutRequestAsync<TPut>(_hueApi.ResourceTypeIdUrl(_type, id), data);
    public Task<HueDeleteResponse> DeleteAsync(Guid id) => _hueApi.HueDeleteRequestAsync(_hueApi.ResourceTypeIdUrl(_type, id));

  }

  public class CrudEndpoint<TGet, TPost, TPut>(BaseHueApi hueApi, string type) : ReadOnlyEndpoint<TGet>(hueApi, type),
    IGetAllEndpoint<TGet>,
    IGetByIdEndpoint<TGet>,
    IPostEndpoint<TPost>,
    IPutEndpoint<TPut>,
    IDeleteEndpoint
    where TGet : HueResource
  {
    public Task<HuePostResponse> CreateAsync(TPost data) => _hueApi.HuePostRequestAsync<TPost>(_hueApi.ResourceTypeIdUrl(_type), data);
    public Task<HuePutResponse> UpdateAsync(Guid id, TPut data) => _hueApi.HuePutRequestAsync<TPut>(_hueApi.ResourceTypeIdUrl(_type, id), data);
    public Task<HueDeleteResponse> DeleteAsync(Guid id) => _hueApi.HueDeleteRequestAsync(_hueApi.ResourceTypeIdUrl(_type, id));

  }

  public class ReadDeleteEndpoint<TGet>(BaseHueApi hueApi, string type) : ReadOnlyEndpoint<TGet>(hueApi, type),
   IGetAllEndpoint<TGet>,
   IGetByIdEndpoint<TGet>,
   IDeleteEndpoint
   where TGet : HueResource
  {
    public Task<HueDeleteResponse> DeleteAsync(Guid id) => _hueApi.HueDeleteRequestAsync(_hueApi.ResourceTypeIdUrl(_type, id));

  }

}
