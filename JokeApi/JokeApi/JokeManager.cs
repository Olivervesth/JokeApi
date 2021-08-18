using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace JokeApi
{
    public class JokeManager
    {
        Jokes jk = new Jokes();
        Session_jokes session_Jokes = new Session_jokes();
        public Joke RndJoke(List<Joke> usedjokes)//Gets a random joke
        {
            Array values = Enum.GetValues(typeof(category));//Category from enum list
            Random rnd = new Random();
            category cat = (category)values.GetValue(rnd.Next(values.Length));

            return JokeByCat(cat.ToString(), usedjokes,"da-DK");//Random jokes are only legal in Denmark as they can handle everything in the world together as vikings
        }

        public Joke JokeByCat(string category, List<Joke> usedjokes, string lang)//Gets a joke by category
        {
            Joke safetyjoke = new Joke();
            if (Enum.IsDefined(typeof(category), category))//Checks if the category is available
            {
                List<Joke> list = jokelist(lang);
                for (int i = 0; i < list.Count; i++)
                {
                    if (!ContainsValue(usedjokes, list[i].joke) && list[i].type == category)
                    {
                        return list[i];
                    }

                }
            }
            else
            {
                return safetyjoke;
                //return "There is no such Category of jokes";

            }
            return NoMoreJks();
            //return "Hello";
        }
        public List<Joke> jokelist(string lang)//Convert items in array to joke and add them to list
        {
            string[,] jokearray;
            if (lang == "da-DK")
            {
                jokearray = jk.jkarraydk;
            }
            else
            {
                jokearray = jk.jkarrayeng;//Not tested but should work
            }
            //Getting 2dim array from data

            List<Joke> list = new List<Joke>();

            for (int i = 0; i < jokearray.GetLength(0); i++)//Going thru 2dim array
            {
                for (int j = 0; j < jokearray.GetLength(1); j++)
                {
                    Joke newjk = new Joke()
                    {
                        type = jokearray[i, 0],
                        joke = jokearray[i, 1],
                    };
                    list.Add(newjk);
                }


            }
            return list;

        }

        public bool ContainsValue(List<Joke> list, string value)//check if list contains joke
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].joke == value)
                {
                    return true;
                }
            }
            return false;
        }

        public Joke NoMoreJks()//Ran out of jokes
        {

            return session_Jokes.EmptyJoke(); ;
        }

        public string CategoryList()//List of all available categories
        {
            List<string> catlist = Enum.GetNames(typeof(category)).ToList();

            return JsonSerializer.Serialize(catlist);
        }

        public CookieOptions CookieExpire()//Time the cookie wil expire
        {
            DateTimeOffset dt = DateTimeOffset.MaxValue;//Just give it max value 
            CookieOptions co = new CookieOptions();
            co.Expires = dt;
            return co;
        }

        public string SaveJokeInSession(Joke tempjoke, string usedjksjson)//Saves the new joke in sessions so we dont get the same joke twice
        {

            List<Joke> usedjokelist = new List<Joke>();

            if (usedjksjson != "" && usedjksjson != null)
            {
                usedjokelist = JsonSerializer.Deserialize<List<Joke>>(usedjksjson);

            }

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
            return jsonstring;//Also returns a the joke as string :O

        }

        public List<Joke> UsedJokesList(string usedjksjson)//returns the list of used jokes
        {
            List<Joke> usedjokelist = new List<Joke>();
            if (usedjksjson != "" && usedjksjson != null)
            {
                return usedjokelist = JsonSerializer.Deserialize<List<Joke>>(usedjksjson);

            }
            return usedjokelist;
        }

    }


}


