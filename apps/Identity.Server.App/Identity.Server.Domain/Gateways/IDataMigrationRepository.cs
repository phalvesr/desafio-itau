namespace Identity.Server.Domain.Gateways;

public interface IDataMigrationRepository
{
    Task MigrateDataAsync();
}
