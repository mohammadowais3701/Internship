using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MongoDBPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient db = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase dbList = db.GetDatabase("Students");
            IMongoCollection<Students> collection = dbList.GetCollection<Students>("Records");
       //     Students obj= new Students();
            try
            {
               
          //      collection.InsertOne(obj.ToBsonDocument());


               //var myupdatingclass = collection.Find(x => x.str == "Owais").First();
               //var Maths= myupdatingclass.score.First(s=>s.Subject=="Maths");
               //Maths.Marks += 80;
               //collection.ReplaceOne(x=>x.str=="Owais",myupdatingclass);
                 FilterDefinition<Students> filter = Builders<Students>.Filter.Eq(x=>x.str,"Owais") & Builders<Students>.Filter.ElemMatch(x => x.score, Builders<Score>.Filter.Eq(x => x.Subject, "Maths"));
                  List<Students> documents = collection.Find(filter).ToList();
                //  Console.WriteLine(documents[0].score[0].Marks);
                  UpdateDefinition<Students> update = Builders<Students>.Update.Inc("score.$.Marks",7);
             collection.UpdateOne(filter, update);
              //  Console.WriteLine("Inserted Successfully");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
    class Students {
     // public  BsonArray barray;

        public Object _id { get; set; }
   //   public List<BsonDocument> score;
      public string str
      {
          get;
          set;
      }
      public Score[] score { get; set; }
   //   public Score score{get;set;}


     /* public Students()
        {
            this.str = "Owais";
            this.score = new List<BsonDocument>(){
            new BsonDocument{{"Subject","English"},{"Marks",20}},
            new BsonDocument{{"Subject","Maths"},{"Marks",10}},
            };
        {
            
          

        };
        }*/
    }
    class Score {
        public String Subject
        {
            get;
            set;


        }
        public int Marks
        {
            get;
            set;
        }
    
    }
  
  
}
