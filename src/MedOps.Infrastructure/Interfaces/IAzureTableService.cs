namespace MedOps.Infrastructure.Interfaces;

using Azure.Data.Tables;

public interface IAzureTableService
{
    Task CreateTableIfNotExistsAsync(string tableName, CancellationToken cancellationToken = default);
    Task AddEntityAsync<T>(string tableName, T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
    Task<T?> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
    Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> QueryEntitiesAsync<T>(string tableName, string filter, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
}