using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWeatherAPI.Models
{
    //used to retrieve the data as contains id, DTO will be used to return data to consumer
    public class CityDTO
    {

        public int Id { get; set; }
        //[maxlength] these are data annotation attributes which are contained in ModelState dictionary
        //these are used for data validation, if not valid they will make ModelState invalid
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        //public int Touristrating { get; set; }
        //public DateTime DateEstablished { get; set; }
        //public long EstimatedPopulation { get; set; }

    }
}
