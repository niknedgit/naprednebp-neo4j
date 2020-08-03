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
    public class UserController : ControllerBase
    {
        [HttpGet("all")]
        public IActionResult Get()
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<User> user = ds.UserRepository.GetAll();
            if (user == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        //[HttpGet("{id}", Name = "Get")]
        [HttpGet]
        [Route("{username}")]

        public IActionResult Get(string username)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            List<User> user = ds.UserRepository.Get(username);
            if (user == null)
            return NotFound();
            var json = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }
        [HttpOptions]
        [Route("{username}")]
        public IActionResult GetUserCheck()
        {
            return Ok();
        }

        [HttpGet]
        [Route("search/{username}/and/{password}")]

        public IActionResult GetByCriteria(string username, string password)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            List<User> user = ds.UserRepository.SearchUser(username, password);
            if (user == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("search/{username}/and/{password}")]

        public IActionResult VratiUser11Check()
        {
            return Ok();
        }


        [HttpPost]
        [Route("add")]

        public IActionResult Post([FromBody] User u)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();


            User user = ds.UserRepository.Create(u);
            if (user == null)
                return NotFound();
                
            return Ok(true);
        }

        [HttpOptions]
        [Route("add")]

        public IActionResult PostCorseCheck()
        {
            return Ok();
        }

        [HttpPut]
        // [Route("edit")]
        public IActionResult Put([FromBody] User u)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            User user = ds.UserRepository.Update(u);
            if (user == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            ds.UserRepository.Delete(id);
        }

        [HttpGet]
        [Route("tastedFood/{username}")]

        public IActionResult TastedFood(string username)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Food> food = ds.UserRepository.TastedFood(username);

            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("tastedFood/{username}")]
        public IActionResult TastedCheck()
        {
            return Ok();
        }



        [HttpGet("usernameExist/{username}")]
        public IActionResult UsernameExist(string username)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            bool exist = ds.UserRepository.UsernameExist(username);
            
            var json = JsonConvert.SerializeObject(exist, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("recomendedFood/{usernameS}/and/{usernameD}")]
        
        public IActionResult GetRecomendedFood(string usernameS, string usernameD)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Food> food = ds.UserRepository.GetRecomendedFood(usernameS, usernameD);

            if (food == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(food, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }
        [HttpOptions]
        [Route("tastedFood/{username}")]
        public IActionResult GetRCheck()
        {
            return Ok();
        }



        [HttpPost]
        [Route("LogIn")]
        public IActionResult LogIn([FromBody]LogIn logIn)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            if (ds.UserRepository.UserExist(logIn))
            {
                return Ok(true);
            }

            return NotFound();
        }

        [HttpOptions]
        [Route("LogIn")]
        public IActionResult PrijavljivanjeCorseCheck()
        {
            return Ok();
        }
    }
}