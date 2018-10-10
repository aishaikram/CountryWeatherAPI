using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountryWeatherAPI.Entities;

namespace CountryWeatherAPI.Services
{
  
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        void ICityInfoRepository.AddCity(City city)
        {
            _context.Cities.Add(city);

        }

        bool ICityInfoRepository.CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        IEnumerable<City> ICityInfoRepository.GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        City ICityInfoRepository.GetCity(int Id)
        {
            return _context.Cities.Where(c => c.Id == Id).FirstOrDefault();
        }

        bool ICityInfoRepository.Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        void ICityInfoRepository.DeleteCity(City city)
        {
           // var cityToUpdate = _context.Cities.Find(Id);
           // cityToUpdate = city;
            _context.Cities.Remove(city);
        }
    }
}
