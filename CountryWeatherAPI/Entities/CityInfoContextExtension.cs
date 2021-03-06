﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWeatherAPI.Entities
{
    public static class CityInfoContextExtension
    {
        public static void EnsureSeedDataforContext(this CityInfoContext context)
        {
            if(context.Cities.Any())
            {
                return;
            }
            //AI- init seed data (startup data for test) ids are auto generated by DB - use City Entity
            var cities = new List<City>
            {
                new City()  {  Name = "London", State = "", Country= "United Kingdom" },
                new City () { Name = "New York", State = "Ohio", Country = "United States of America" }
             };
            // add cities to the context 
            context.AddRange(cities);
            context.SaveChanges(); //commit changes in the context to the actual DB
        }
    }
}
