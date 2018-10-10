using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWeatherAPI.Entities
{
    //AI- DBContext is used to save a session to the database. It will be used for dependency injection
    //DBsets are used to query and save the entities in the database. LINQ queries against dbset will be
    //converted into the database queries to get data
    public class CityInfoContext : DbContext
    {//bind the entity class here
        public DbSet<City> Cities { get; set; }
        
        //AI_ this constructor is called at the time of instantiation at the time of  to dependency injection as this service is registered in ConfigureServices
        public CityInfoContext(DbContextOptions<CityInfoContext> options): base(options)
        {
            /*AI_ generates db if not already created
            Database.EnsureCreated();*/
            //create the db if doesnt exist and execute migration
            Database.Migrate();
        }


    }
}
