namespace MedOps.Infrastructure.Services;

using MedOps.Infrastructure.Interfaces;
using Azure.Data.Tables;

public class AzureTableService : IAzureTableService
{
    private readonly string _connectionString;
    private readonly TableClient _tableClient;

    public AzureTableService(string connectionString, string tableName)
    {
        _connectionString = connectionString;
        var tableServiceClient = new TableServiceClient(connectionString);
        _tableClient = tableServiceClient.GetTableClient(tableName);
    }

    public async Task CreateTableIfNotExistsAsync(string tableName, CancellationToken cancellationToken = default)
    {
        var client = new TableClient(_connectionString, tableName);
        await client.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task AddEntityAsync<T>(string tableName, T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
    {
        var client = new TableClient(_connectionString, tableName);
        await client.AddEntityAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task<T?> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
    {
        var client = new TableClient(_connectionString, tableName);
        var response = await client.GetEntityAsync<T>(partitionKey, rowKey, cancellationToken: cancellationToken);
        return response.Value;
    }

    public async Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default)
    {
        var client = new TableClient(_connectionString, tableName);
        await client.DeleteEntityAsync(partitionKey, rowKey, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<T>> QueryEntitiesAsync<T>(string tableName, string filter, CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
    {
        var client = new TableClient(_connectionString, tableName);
        var results = client.QueryAsync<T>(filter: filter, cancellationToken: cancellationToken);
        var list = new List<T>();
        await foreach (var result in results)
        {
            list.Add(result);
        }
        return list;
    }
}