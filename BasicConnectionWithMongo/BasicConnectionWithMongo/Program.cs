using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;


namespace BasicConnectionWithMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MongoClient db = new MongoClient("mongodb://127.0.0.1:27017");
                IMongoDatabase dbList = db.GetDatabase("Products");
                IMongoCollection<BsonDocument> collection = dbList.GetCollection<BsonDocument>("Products Details");
                FilterDefinition <BsonDocument> filter= Builders<BsonDocument>.Filter.Eq("Product_Qty",8);
                List<BsonDocument> documents = collection.Find(filter).ToList();
                foreach (BsonDocument b in documents) {
                    Console.WriteLine(b.ToString());
                }
                products product = new products();
                product.Product_Name = "TableTenis";
                product.Product_Qty = 4;
                product.price = 5000;
                product.Batch = 2018;
                product.code = "HXCAS12";
                BsonDocument doc = product.ToBsonDocument();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("Price", "5000");
                dic.Add("Product_Name", "TableTenis");
                dic.Add("Product_Qty", "10");
                UpdateDefinition<BsonDocument> update;
                foreach (KeyValuePair<String, String> element in dic) {
                    int n;
                    bool isNumber = int.TryParse(element.Value, out n);
                    if (isNumber)
                    {
                      update = Builders<BsonDocument>.Update.Set(element.Key, Convert.ToInt32(element.Value));
                    }
                    else {
                         update = Builders<BsonDocument>.Update.Set(element.Key, element.Value);
                    }
                      collection.UpdateMany(filter, update);
                }
                Console.WriteLine("Updated Successfully");
                
           try  {
                    collection.InsertOne(doc);
                    Console.WriteLine("Inserted One Record Successfully");
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
           try
           {
               List<BsonDocument> docs = new List<BsonDocument>()
               {
                  
                    new BsonDocument{{"Product_Name","ABC"},{"Product_Qty",8},{"Batch",2021},{"Price",129}},
                    new BsonDocument{{"Product_Name","DEF"},{"Product_Qty",9},{"Batch",2021},{"Price",129}},
                    new BsonDocument{{"Product_Name","GHI"},{"Product_Qty",10},{"Batch",2021},{"Price",129}},
                    new BsonDocument{{"Product_Name","JKL"},{"Product_Qty",11},{"Batch",2021}},

                  
               };
               collection.InsertMany(docs);
               Console.WriteLine("Inserted Many Records");
           }
           catch (Exception ex) {
               Console.WriteLine(ex.Message);
           
           }

             
              
                
            }
            catch (Exception ex)
            {

            }

        }
    }
    class products {
     public   string Product_Name { get; set; }
     public   int Product_Qty { get; set; }
     public    int Batch { get; set; }
     public   int price { get; set; }
     public   string code { get; set; }
    
    }
}
