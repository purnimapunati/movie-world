using MovieWorld.Infra.Models;

namespace MovieWorld.Infra
{
    public interface IApiClientFactory
    {
      Task<T> Get<T>(MovieProviderType provider, string endPoint);
    }
}
