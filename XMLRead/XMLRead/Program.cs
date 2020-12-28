using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace XMLRead
{
    class Program
    {
        static void Main(string[] args)
        {
         
            XmlDocument doc = new XmlDocument();
         
                String path;
                Console.WriteLine(" Enter File Name");
                path=Console.ReadLine();
                path += ".xml";
                while(!File.Exists(path)){
               
                Console.WriteLine("File Not Exist, Enter File Name");
                path=Console.ReadLine();
                path += ".xml";

                
                }
                 doc.Load(path);

            
            
            XmlNodeList nodes = doc.SelectNodes("//bookstore/book[price>35]");// | //book[price>35]/price | //book[price>35]/author| //book[price>35]/year ");
         
          foreach (XmlNode node in nodes)
          {
              string[] str = node.OuterXml.Split('>');
              foreach (string s in str)
              {
                  
                  Console.WriteLine(s + ">");


              }
              Console.WriteLine();
          }
           
          
          
        }
    }
}
