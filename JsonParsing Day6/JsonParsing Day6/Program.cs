using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace JsonParsing_Day6
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {

                List<A> a = new List<A>();
                a.Add(new A { Name = "M.Owais", Year = "4th", CGPA = 3.3 });
                a.Add(new A { Name = "Ali", Year = "4th" });


                string info = JsonConvert.SerializeObject(a);
                Console.WriteLine(info);

                A[] des = JsonConvert.DeserializeObject<A[]>(info);
                Console.WriteLine(des[1].Name);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            
            }
         //   JObject r = JObject.Parse(json);
           // Console.WriteLine(r["Year"]);
            Console.ReadKey();
        }
    }

    class A {
       public string Name
        {
            get;
            set;


        }
   public     string Year
        {

            get;
            set;

        }

   public     Double CGPA
        {
            get;
            set;


        }
    
    
    }
}
