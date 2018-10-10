using CountryWeatherAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWeatherAPI.Services
{
    //deals with entities
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int Id);
        bool CityExists(int cityId);
        void AddCity(City city);
        bool Save();
        void DeleteCity(City city);
    }
}
