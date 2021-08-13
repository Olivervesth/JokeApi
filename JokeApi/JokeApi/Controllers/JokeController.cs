using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JokeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {

        JokeManager jkm = new JokeManager();

        [HttpGet]
        [Route("{cat}")]
        public string GetJoke(string cat)
        {

            List<Joke> usedjokelist = new List<Joke>();

            string usedjksjson = HttpContext.Session.GetString("usedjokes");

            if (usedjksjson != "" || usedjksjson != null)
            {
                usedjokelist = JsonSerializer.Deserialize<List<Joke>>(usedjksjson);

            }
            Joke tempjoke = jkm.JokeByCat(cat, usedjokelist);
            string joke = tempjoke.joke;
            List<Joke> jokelist = new List<Joke>();
            jokelist.Add(tempjoke);
            string jsonstring = "";
            if (usedjksjson == "" || usedjksjson == null)
            {
                jsonstring = JsonSerializer.Serialize(jokelist);
            }
            else
            {
                //List<Joke> usedjokelist = JsonSerializer.Deserialize<List<Joke>>(usedjksjson);
                jokelist.AddRange(usedjokelist);
                jsonstring = JsonSerializer.Serialize(jokelist);
            }
            Request.HttpContext.Session.SetString("usedjokes", jsonstring);
            return joke;
        }
        [HttpGet]
        [Route("[action]")]
        public string test()
        {
            return "test";
        }
    }
}
