using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Neo4j1.Models;

namespace Neo4j1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        [HttpGet("all")]
        public IActionResult Get()
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Restaurant> restaurant = ds.RestaurantRepository.GetAll();
            if (restaurant == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(restaurant, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }
        
        [HttpGet("{name}/{city}")]
        public IActionResult Get(string name, string city)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Restaurant> restaurants = ds.RestaurantRepository.GetByNC(name, city);
            if (restaurants == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(restaurants, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet("getByCity/{city}")]
        public IActionResult GetByCity(string city)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            List<Restaurant> restaurants = ds.RestaurantRepository.GetByCity(city);
            if (restaurants == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(restaurants, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody] Restaurant r)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            List<Restaurant> restaurantExists = ds.RestaurantRepository.GetByNC(r.Name,r.City);
            Restaurant restaurant;

            if (restaurantExists.Count == 0)
            {
             restaurant = ds.RestaurantRepository.Create(r);
            }

             restaurant = ds.RestaurantRepository.Get(r.Name, r.City);


            if (restaurant == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(restaurant, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("add")]
        public IActionResult AddRestaurant()
        {
            return Ok();
        }

        [HttpPut]
        // [Route("edit")]
        public IActionResult Put([FromBody] Restaurant r)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            Restaurant restaurant = ds.RestaurantRepository.Update(r);
            if (restaurant == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(restaurant, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            ds.RestaurantRepository.Delete(id);
        }

        //[HttpPost("addFoodToMenu/{restaurantName}")]
        //public IActionResult AddFoodToMenu([FromBody] Food f, string restaurantName)
        //{
        //    DataLayerService.DataLayerService ds = DataLayer.GetService();

        //    Food food = ds.FoodRepository.Create(f);
        //    Restaurant restaurant = ds.RestaurantRepository.AddFoodToMenu(food, restaurantName);

        //    if (restaurant == null || food == null)
        //        return NotFound();

        //    restaurant.Menu.Add(food);

        //    var json = JsonConvert.SerializeObject(restaurant, new JsonSerializerSettings()
        //    {
        //        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        //    });
        //    return Ok(json);
        //}

        [HttpGet("menu/{restaurantName}/and/{restaurantCity}")]
        public IActionResult GetMenu(string restaurantName, string restaurantCity)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            List<Food> menu = ds.RestaurantRepository.GetMenu(restaurantName, restaurantCity);
            if (menu == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(menu, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }
    }
}