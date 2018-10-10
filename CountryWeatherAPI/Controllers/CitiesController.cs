using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CountryWeatherAPI.Models;
using CountryWeatherAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CountryWeatherAPI.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : Controller
    {
        //AI- container(HTTP Context) by deafult creates an instance of Ilogger<Type of the class using this instance in DI)
        readonly ILogger<CitiesController> _logger;

        readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository, ILogger<CitiesController> logger)
        {
            _logger = logger;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            /* returned in memory data
            var cities = new JsonResult(CitiesDataStore.Current.Cities);
              cities.StatusCode = 200;
            return cities;*/

            //getting data from DB
            var cityEntities = _cityInfoRepository.GetCities();

            var result = AutoMapper.Mapper.Map<IEnumerable<CityDTO>>(cityEntities);

            /*use AutoMapper Nuget package to map between entities to DTO
            //put data from entities to DTOs for consumer
            var result = new List<CityDTO>();           
            
            *foreach(var ent in cityEntities)
            {
                result.Add(new CityDTO() { Id = ent.Id, Name = ent.Name, State = ent.State, Country = ent.Country });

            }*/
            return Ok(result);

        }

        [HttpGet("{id}", Name = "GetCity")]
        public IActionResult GetCity(int id)
        {
            try
            {
                /* var city = new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id)); //AI-use LINQ query to match in collection */
                if (!_cityInfoRepository.CityExists(id))
                {
                    _logger.LogInformation($"city with if {id} is not found");
                    return NotFound();
                }
                //getting data from DB if city exists
                var cityEntity = _cityInfoRepository.GetCity(id);
                return Ok(cityEntity);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting city with id {id}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        //   [HttpPost("{Id}")]
        //AI-request attribute along with parameters 

        /* sample URI is http://localhost:56282/api/cities with post msg and sending json object in body*/
        [HttpPost()]
        //AI- content body data to create a city is passed using FromBody
        public IActionResult AddCity([FromBody] CityForCreationDTO city)
        {
            Debug.WriteLine("add city initiated");
            //deserialise the frombody data and use to create city
            if (city == null)
                return BadRequest();

            //[maxlength] in DTOs these are data annotation attributes which are contained in ModelState dictionary
            //these are used for data validation, if not valid they will make ModelState invalid
            //in that case body can not be deserialized

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // var FinalCity = CitiesDataStore.Current.Cities
            // FinalCity = city
            //returns a response with location header containing URI for the created city
            var cityEntity = AutoMapper.Mapper.Map<Entities.City>(city);

            //cityEntity is passed by reference into the context DBset
            _cityInfoRepository.AddCity(cityEntity);

            //On saving Context, the reference object cityEntity is populated with actual id
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Error adding city");
            }

            //cityDTO is used only for retrieval or get or return process
            //here the entity is convereted back to model
            var createdCity = AutoMapper.Mapper.Map<Models.CityDTO>(cityEntity);

            return CreatedAtRoute("GetCity", new { id = createdCity.Id }, createdCity);
        }

        /* Update action to update an object */
        //full update
        [HttpPut("{Id}")]
        public IActionResult UpdateCity(int Id, [FromBody] CityForUpdateDTO city)
        {
            // for repeated validations use Fluent Validation

            Debug.WriteLine("update city initiated");
            //deserialise the frombody data and use to create city
            if (city == null)
                return BadRequest();

            //[maxlength] in DTOs these are data annotation attributes which are contained in ModelState dictionary
            //these are used for data validation, if not valid they will make ModelState invalid
            //in that case body can not be deserialized

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cityInfoRepository.CityExists(Id))
                return NotFound();

            var cityToUpdate = _cityInfoRepository.GetCity(Id);
            if (cityToUpdate == null)
            {
                return NotFound();
            }
            //this overload Map method of Mapper copies the source object content (1st param) into the destination object (2nd param)
            //which is what is required to update the content entity
            AutoMapper.Mapper.Map(city, cityToUpdate);

            //On saving Context, the reference object cityUpdate is automatically populated
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Error updating city");
            }

            return NoContent();
        }

        /* Delete action to delete an object */

        [HttpDelete("{Id}")]
        public IActionResult DeleteCity(int Id)
        {
            Debug.WriteLine("Delete city initiated");

            if (!_cityInfoRepository.CityExists(Id))
                return NotFound();

            var cityToDelete = _cityInfoRepository.GetCity(Id);
            if (cityToDelete == null)
            {
                return NotFound();
            }
            _cityInfoRepository.DeleteCity(cityToDelete);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Error deleting city");
            }

            return NoContent();

        }

        [HttpPatch("{id}")]
        public IActionResult UpdateCityPartial(int Id, [FromBody] JsonPatchDocument<CityForUpdateDTO> jsonPatchDoc)
        {
            if (jsonPatchDoc == null)
                return BadRequest();
            if (!_cityInfoRepository.CityExists(Id))
                return NotFound();

            //get the entity from DB
            var cityToUpdateEntity = _cityInfoRepository.GetCity(Id);
            if (cityToUpdateEntity == null)
            {
                return NotFound();
            }
            //map the entity onto the corresponding DTO object
            var cityUpdateDTOtoPatch = AutoMapper.Mapper.Map<CityForUpdateDTO>(cityToUpdateEntity);

            //apply json patch to the DTO and merge changes and log any errors in the modelstate dictionary
            jsonPatchDoc.ApplyTo(cityUpdateDTOtoPatch, ModelState);

            //if there are any errors patching it to the DTO 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // just adding an error in model state in case
            if (cityUpdateDTOtoPatch.Name == cityUpdateDTOtoPatch.Country)
                ModelState.AddModelError("Name", "The provided Name should be different from the Country.");

            // this method validates the
            TryValidateModel(cityUpdateDTOtoPatch);

            if (!ModelState.IsValid)
                 return BadRequest(ModelState);
            
            //put the DTO back in the Entity 
            AutoMapper.Mapper.Map(cityUpdateDTOtoPatch, cityToUpdateEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}