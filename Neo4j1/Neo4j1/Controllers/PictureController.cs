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
    public class PictureController : ControllerBase
    {
        [HttpGet("all")]
        public IActionResult Get()
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            IEnumerable<Picture> picture = ds.PictureRepository.GetAll();
            if (picture == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(picture, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        //[HttpGet("{id}", Name = "Get")]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            Picture picture = ds.PictureRepository.Get(id);
            if (picture == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(picture, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody] Picture p)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            Picture picture = ds.PictureRepository.Create(p);
            if (picture == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(picture, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }


        [HttpOptions]
        [Route("add")]

        public IActionResult PostCorseCheck()
        {
            return Ok();
        }


        [HttpPut]
        // [Route("edit")]
        public IActionResult Put([FromBody] Picture p)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            Picture picture = ds.PictureRepository.Update(p);
            if (picture == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(picture, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            ds.PictureRepository.Delete(id);
        }

        [HttpPost]
        [Route("storeImageUrl")]
        public IActionResult StoreImageUrl([FromBody] Picture p)
        {
            DataLayerService.DataLayerService ds = DataLayer.GetService();

            Food f = ds.PictureRepository.StoreImageUrl(p);
            if (f == null)
                return NotFound();
            var json = JsonConvert.SerializeObject(f, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpOptions]
        [Route("storeImageUrl")]

        public IActionResult StoreCorseCheck()
        {
            return Ok();
        }
    }
}