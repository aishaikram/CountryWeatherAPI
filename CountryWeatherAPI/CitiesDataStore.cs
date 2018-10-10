using CountryWeatherAPI.Models;
using System.Collections.Generic;

namespace CountryWeatherAPI
{
    //AI- static class returning hardcoded data
    public class CitiesDataStore
    {
        //AI- readonly property using property initializer syntax
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDTO> Cities { get; set; }
        public CitiesDataStore() => Cities = new List<CityDTO>
            {
                new CityDTO() {Id=1, Name="NewYork"},

                new CityDTO() {  Id = 2, Name = "London" }

                };
    }


}
