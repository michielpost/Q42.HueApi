using HueApi.Models;

namespace HueApi.HueEndpoints
{
  public interface IGetAllEndpoint<TGet> where TGet : HueResource
  {
    Task<HueResponse<TGet>> GetAllAsync();
  }

  public interface IGetByIdEndpoint<TGet> where TGet : HueResource
  {
    Task<HueResponse<TGet>> GetByIdAsync(Guid id);
  }

  public interface IPostEndpoint<TPost>
  {
    Task<HuePostResponse> CreateAsync(TPost data);
  }

  public interface IPutEndpoint<TPut>
  {
    Task<HuePutResponse> UpdateAsync(Guid id, TPut data);
  }

  public interface IDeleteEndpoint
  {
    Task<HueDeleteResponse> DeleteAsync(Guid id);
  }
}
