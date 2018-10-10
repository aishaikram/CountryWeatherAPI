using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWeatherAPI.Models
{
    //creation DTO without Id, will do validation on input
    public class CityForUpdateDTO
    {
        //[maxlength] these are data annotation attributes which are contained in ModelState dictionary
        //these are used for data validation, if not valid they will make ModelState invalid
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string State { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }

        //public int Touristrating { get; set; }
        //public DateTime DateEstablished { get; set; }
        //public long EstimatedPopulation { get; set; }

    }
}
