using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountryWeatherAPI.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string State { get; set; }
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }
        //public int Touristrating { get; set; }
        //public System.DateTime DateEstablished { get; set; }
        //public long EstimatedPopulation { get; set; }
    }
}
