using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Search.Sample.Models;
using Azure.Search.Sample.Services;
using Microsoft.Azure.Search.Models;

namespace Azure.Search.Sample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            const string serviceName = "";
            const string apiKey = "";
            const string indexName = "";

            Console.WriteLine("- Starting -");

            var service = new AzureSearchService(serviceName, apiKey);
            Console.WriteLine("- Creating Search Service -");

            service.DeleteIndexIfExists(indexName);
            Console.WriteLine("- Index deleted -");

            service.CreateIndex<Trip>(indexName);
            Console.WriteLine("- Index created -");

            var documents = new List<Trip>
            {
                new Trip
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Trip to Alaska",
                    Tags = new[] {"Alaska", "Holiday"},
                    Duration = 21
                },
                new Trip
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Cruise to Sweden",
                    Tags = new[] {"Sweden", "Cruise"},
                    Duration = 14
                }
            };

            service.AddOrUpdateDocumentToIndex<Trip>(documents, indexName);
            Console.WriteLine("- 2 documents added to Index -");

            // Search for a keyword in the compleet index
            DocumentSearchResult<Trip> results = service.Search<Trip>("Alaska",new SearchParameters(), indexName);
            foreach (var trip in results.Results)
            {
                Console.WriteLine("Result: " + trip.Document.Name);
            }

            // Find trip with a duration less then 18 days
            var parameters = new SearchParameters()
            {
                Filter = "duration lt 18"
            };

            DocumentSearchResult<Trip> resultsFilter = service.Search<Trip>("*", parameters, indexName);
            foreach (var trip in resultsFilter.Results)
            {
                Console.WriteLine("Result: " + trip.Document.Name + " Duration: " + trip.Document.Duration);
            }

            Console.WriteLine("- End, press any key to close -");
            Console.ReadLine();

        }
    }
}
