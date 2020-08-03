using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j1.Models;
using Neo4j1.Repository.Repositories;
using Newtonsoft.Json;

namespace Neo4j1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        // GET: api/Food
        [HttpGet("all")]
        public IActionResult Get()
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Food> food = ds.FoodRepository.GetAll();
            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        // GET: api/Food/5
        //[HttpGet("{id}", Name = "Get")]
        [HttpGet]
        [Route("Find/{id}")]

        public IActionResult Get(string id)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Food> food = ds.FoodRepository.GetById(id);
            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("Find/{id}")]
        public IActionResult GetIdCheck()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Find/n/{name}")]
        public IActionResult GetByName(string name)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Food> food = ds.FoodRepository.GetName(name);
            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("Find/n/{name}")]
        public IActionResult VratiJeloCheck()
        {
            return Ok();
        }

        [HttpGet]
        [Route("search/{name}/and/{description}")]

        public IActionResult GetByCriteria(string name, string description)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Food> food = ds.FoodRepository.SearchFood(name, description);
            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("search/{name}/and/{description}")]
        public IActionResult VratiJelo1Check()
        {
            return Ok();
        }
        // POST: api/Food
        [HttpPost]
        [Route("add")]
        public IActionResult AddFood([FromBody] LinkUserFood l )
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();
       
            List<Food> foodExist = ds.FoodRepository.GetSame(l.Name, l.Description, l.Type);
            if (foodExist.Count != 0)
            {
                foreach (Food fe in foodExist)
                {
                    if (ds.RestaurantRepository.RelationshipServeExist(fe, l.RestaurantName, l.RestaurantCity))
                    {
                        var json = JsonConvert.SerializeObject(fe, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });
                        return Ok(json);
                    }
                }
            }

            string username = l.Username;

            Food food;
           
            if (foodExist.Count == 0)
            {
                Food f = new Food();
                f.Name = l.Name;
                f.Type = l.Type;
                f.Description = l.Description;
                f.AverageRating = 0;
                f.NumberOfVotes = 0;

                food = ds.FoodRepository.Create(f);
            }
            else
                food = ds.FoodRepository.Get(foodExist[0].Id);

            Picture picture=new Picture();
            picture.PictureURL = l.PictureURL;
            picture.FoodId = food.Id;

            ds.PictureRepository.StoreImageUrl(picture);

            if (!ds.FoodRepository.RelationshipTastedExist(username, food))
                ds.FoodRepository.LinkFromUserToFood(username, food);

            ds.RestaurantRepository.AddFoodToMenu(food, l.RestaurantName,l.RestaurantCity);
            Restaurant rest = ds.RestaurantRepository.Get(l.RestaurantName, l.RestaurantCity);
            rest.Menu.Add(food);

            if (food == null)
                return NotFound();
            return Ok(true);
        }

        [HttpOptions]
        [Route("add")]
        public IActionResult AddFood()
        {
            return Ok();
        }

        [HttpGet]
        [Route("tasters/{foodId}")]
        public IActionResult GetTasters([FromRoute] string foodId)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<User> users = ds.FoodRepository.GetTasters(foodId);
            if (users == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(users, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("tasters")]
        public IActionResult GTCheck()
        {
            return Ok();
        }

        [HttpGet]
        [Route("getPictures/{foodId}")]
        public IActionResult GetPictures([FromRoute] string foodId)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Picture> pics = ds.FoodRepository.GetPictures(foodId);
            if (pics == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(pics, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("getPictures/{foodId}")]
        public IActionResult GPCheck()
        {
            return Ok();
        }

        [HttpPut]
       // [Route("edit")]
        public IActionResult Put( [FromBody] Food f)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            Food food = ds.FoodRepository.Update(f);
            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            ds.FoodRepository.Delete(id);   
        }
        [HttpOptions("{id}")]
        public IActionResult DeleteCorseCheck()
        {
            return Ok();
        }

        [HttpPost]
        [Route("vote")]
        public IActionResult RateFood([FromBody] Rate rated)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();
            string id, username;
            float rate;
            id = rated.Id;
            username = rated.Username;
            rate = rated.Rating;

            Food food = ds.FoodRepository.RateFood(id, rate, username);

            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            var tip = new { tip = food.AverageRating };
            return Ok(tip);
        }

        [HttpOptions]
        [Route("vote")]
        public IActionResult VoteCorseCheck()
        {
            return Ok();
        }

        [HttpPost]
        [Route("similarly")]
        public IActionResult GetSimilarlyFood([FromBody] Food f)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            List<Food> food = ds.FoodRepository.GetSimilarlyFood(f);

            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }
        [HttpOptions]
        [Route("similarly")]
        public IActionResult SimilarityCorseCheck()
        {
            return Ok();
        }
    }
}
