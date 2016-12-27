using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Search.Sample.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Azure.Search.Sample.Services
{
    public interface IAzureSearchService
    {
        void CreateIndex<T>(string indexName);

        DocumentIndexResult AddOrUpdateDocumentToIndex<T>(List<T> documents, string indexName) where T : class, new();

        void DeleteIndexIfExists(string indexName);

        DocumentSearchResult<T> Search<T>(string keyword, SearchParameters searchParameters, string indexName) where T : class, new();

    }

    public class AzureSearchService : IAzureSearchService
    {
        private readonly string _indexName;
        private readonly SearchServiceClient _searchClient;

        public AzureSearchService(string searchServiceName, string searchServiceApiKey)
        {
            _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceApiKey));
        }

        public void CreateIndex<T>(string indexName)
        {
            if (_searchClient.Indexes.Exists(indexName))
            {
                return;
            }
            
            var definition = new Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            _searchClient.Indexes.Create(definition);
        }

        public DocumentIndexResult AddOrUpdateDocumentToIndex<T>(List<T> documents, string indexName) where T : class, new()
        {
            ISearchIndexClient indexClient = _searchClient.Indexes.GetClient(indexName);
            IndexBatch<T> batch = IndexBatch.Upload(documents);

            try
            {
                return indexClient.Documents.Index(batch);
            }
            catch (IndexBatchException e)
            {
                throw e;
            }
        }

        public void DeleteIndexIfExists(string indexName)
        {
            if (_searchClient.Indexes.Exists(indexName))
            {
                _searchClient.Indexes.Delete(indexName);
            }
        }

        public DocumentSearchResult<T> Search<T>(string keyword, SearchParameters searchParameters, string indexName) where T : class, new()
        {
            ISearchIndexClient indexClient = _searchClient.Indexes.GetClient(indexName);
            return indexClient.Documents.Search<T>(keyword, searchParameters);
        }

    }


}
