using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace JokeApi
{
    public class JokeManager
    {
        Jokes jk = new Jokes();
        Session_jokes session_Jokes = new Session_jokes();
        //public Joke RndJoke()//Gets a random joke
        //{
        //    Array values = Enum.GetValues(typeof(category));//Category from enum list
        //    Random rnd = new Random();
        //    category cat = (category)values.GetValue(rnd.Next(values.Length));

        //    return JokeByCat(cat.ToString());
        //}

        public Joke JokeByCat(string category,List<Joke> usedjokes)//Gets a joke by category
        {
            Joke safetyjoke = new Joke();
            if (Enum.IsDefined(typeof(category), category))
            {
                List < Joke > list = jokelist();
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
        public List<Joke> jokelist()//Convert items in array to joke and add them to list
        {

            string[,] jokearray = jk.jkarray;//Getting 2dim array from data
            List<Joke> list = new List<Joke>();
            Random rnd = new Random();

            for (int i = 0; i < jokearray.GetLength(0); i++)//Going thru 2dim array
            {
                for (int j = 0; j < jokearray.GetLength(1); j++)
                {
                    Joke newjk = new Joke()
                    {
                        //id = i + j + rnd.Next(1, 20),
                        id = 0,
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

        public Joke NoMoreJks()
        {
           
            return session_Jokes.EmptyJoke(); ;
        }
        public string CategoryList()
        {
           List<string> catlist = Enum.GetNames(typeof(category)).ToList();

           return JsonSerializer.Serialize(catlist);
        }

       
    }
}


