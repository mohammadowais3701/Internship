using System;
using System.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;


using System.Threading.Tasks;


namespace JSON_Parsing_Day7_
{
    class Program
    {
        static void Main(string[] args)
        {

            string str = "{\"Name\":\"Ali\",\"Age\":23,\"Gender\":\"male\",\"ROLLNUM\":\"B1610\",\"Teachers\":{\"Name\":\"Ahmed\",\"Faculty\":\"Science\",\"Courses\":{\"Course_Name\":\"ICS\",\"C_ID\":102}}}";
          
            //Convert a JSON string into Object form with respect to Json Pattern
            try
            {
                var jsonmsg = JsonConvert.DeserializeObject<Students>(str);
                Console.WriteLine("Roll Number:"+jsonmsg.RollNUM);
                Console.WriteLine("NAme:"+jsonmsg.Name);
                Console.WriteLine("Age:"+jsonmsg.Age);
                Console.WriteLine("Gender:"+jsonmsg.Gender);
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            
            }
            try
            {
                dynamic jsonmsg = JsonConvert.DeserializeObject(str);
                Console.WriteLine("Course:"+jsonmsg.Teachers.Courses.Course_Name);
                Console.WriteLine("Teacher Name:" + jsonmsg.Teachers.Name);


            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            
            
            }
        
            Console.ReadKey();
        }
    }

    class Courses {
        public string Course_Name
        {
            get;
            set;


        }
        public int C_ID
        {
            get;
            set;


        }
    }

        class Teachers {

            public string Name
            {
                get;
                set;


            }
            public string Faculty
            {

                get;
                set;


            }

            public List<Courses> Courses
            {
                get;
                set;

            }
        
        }

        class Students {
            public string Name { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            public string RollNUM{get;set;}
            public List<Teachers> teach { get; set; }
        }



    
    
    }

