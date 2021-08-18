using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SecuringWebApiUsingApiKey.Attributes;

namespace JokeApi.Controllers
{
    //[ApiKey]
    [Route("[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {

        JokeManager jkm = new JokeManager();

        [HttpGet]
        [Route("{cat}")]
        public string GetJoke(string cat)//Returns a joke from chosen category
        {
            Request.Headers.TryGetValue("Accept-Language", out var alpha);
            Response.Cookies.Append("favoritecat", cat, jkm.CookieExpire());
            string[] lang = alpha.ToString().Split(',');
            Joke tempjoke = jkm.JokeByCat(cat, jkm.UsedJokesList(HttpContext.Session.GetString("usedjokes")), lang[0]);
            string usedjksjson = HttpContext.Session.GetString("usedjokes");

            if (tempjoke.type != "no")
            {
                Request.HttpContext.Session.SetString("usedjokes", jkm.SaveJokeInSession(tempjoke, usedjksjson));

            }
            string joke = tempjoke.joke;
            return joke;
        }
       
        [HttpGet]
        [Route("categorylist")]
        public string ListOfCategorys()//Returns a list of categories
        {
            return jkm.CategoryList();
        }

        [HttpGet]
        [Route("random")]
        public string RandJoke()//Returns a random joke
        {
            List<Joke> usedjokes = jkm.UsedJokesList(HttpContext.Session.GetString("usedjokes"));
            Joke joke = jkm.RndJoke(usedjokes);
            return joke.joke;
        }

        [HttpGet]
        [Route("fav")]
        public string GetCookie()//Get the favorit category
        {
            return Request.Cookies["favoritecat"];
        }

    }
}
