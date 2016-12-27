using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Azure.Search.Sample.Models
{
    [SerializePropertyNamesAsCamelCase]
    public class Trip
    {
        [Key, IsFilterable]
        public string Id { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        public string Name { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public int? Duration { get; set; }

        [IsSearchable, IsFilterable, IsFacetable]
        public string[] Tags { get; set; }
     
    }
}