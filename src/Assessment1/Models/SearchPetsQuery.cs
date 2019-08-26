using System.ComponentModel.DataAnnotations;

namespace Assessment1.Models
{
    /// <summary>
    /// Represents a query to search pets in pest store with given parameters 
    /// </summary>
    public class SearchPetsQuery
    {
        /// <summary>
        /// Search pets by multiple statuses
        /// </summary>
        [Required]
        public PetStatus[] Statuses { get; set; }

        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 20;
    }
}
