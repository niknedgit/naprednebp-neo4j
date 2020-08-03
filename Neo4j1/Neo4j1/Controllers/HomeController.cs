using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4j1.Models;
using Newtonsoft.Json;
using Neo4j1.Models;

namespace Neo4j1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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

        public object Get(string username)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            List<User> user = ds.UserRepository.Get(username);
            if (user == null)
                return null;

            return user;
            //  var res = new { result = user };
            //  return new ObjectResult(user);
        }
        [HttpOptions]
        [Route("{username}")]


        public IActionResult GetUserCheck()
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

        [HttpGet("tastedFood/{username}")]
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

        [HttpGet("recomendedFood/{usernameS}&{usernameD}")]
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
