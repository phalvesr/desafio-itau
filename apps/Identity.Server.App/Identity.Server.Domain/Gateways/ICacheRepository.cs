namespace Identity.Server.Domain.Gateways;

public interface ICacheRepository<T>
{
    Task SetEntryAsync(string key, T unserializedObject, TimeSpan? expiry = null);
    Task<T?> GetEntryOrDefaultAsync(string key);
}
