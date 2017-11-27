using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tavis.Models;

namespace Tavis.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ResponseCache(Duration =15)]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [SwaggerOperation("GetTestStringValues")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values
        [HttpGet]
        [SwaggerOperation("GetTestOUs")]
        [Route("api/values/GetTestOUs")]
        public IEnumerable<TestOU> GetTestOUs()
        {
            var retval = new List<TestOU> { new TestOU { Prop1 = "11", Prop2 = "12" }, new TestOU { Prop1 = "21", Prop2 = "22" } };
            return retval;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
