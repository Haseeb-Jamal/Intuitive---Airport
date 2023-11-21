using System.ComponentModel.DataAnnotations;

namespace Web.API.Models
{
    public class Country
    {
        [Key]
        public int GeographyLevel1ID { get; set; }
        public string Name { get; set; }
    }
}
