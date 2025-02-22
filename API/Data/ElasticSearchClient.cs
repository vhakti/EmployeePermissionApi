namespace EmployeePermissionApi.Data
{
    using Elastic.Clients.Elasticsearch;
    using EmployeePermissionApi.Models;

    public interface IElasticSearchClient
    {
        Task IndexPermissionAsync(Permission permission);
    }

    public class ElasticSearchClient : IElasticSearchClient
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;

        public ElasticSearchClient(IConfiguration configuration)
        {
            var settings = new ElasticsearchClientSettings(new Uri(configuration["ElasticSearch:Uri"]))
                .DefaultIndex(configuration["ElasticSearch:Index"]);
            _client = new ElasticsearchClient(settings);
            _indexName = configuration["ElasticSearch:Index"];
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            var response = await _client.IndexAsync(permission, i => i
                .Index(_indexName)
                .Id(permission.Id.ToString()));

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to index permission: {response.DebugInformation}");
            }
        }
    }
}
