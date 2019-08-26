using System.Collections.Generic;

namespace Assessment1.Models
{
    /// <summary>
    /// Represents a pets search query result 
    /// </summary>
    public class SearchPetsResult
    {
        public IEnumerable<Pet> Pets { get; set; }
        public int TotalCount { get; set; }
    }
}
