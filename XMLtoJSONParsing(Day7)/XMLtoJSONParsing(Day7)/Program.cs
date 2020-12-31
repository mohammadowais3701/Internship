using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
namespace XMLtoJSONParsing_Day7_
{
    class Program
    {
        static void Main(string[] args)
        {

            string xmlString =@"<?xml version='1.0'?>
                        <Description>
                        <Name>
                        <FirstName > Muhammad</FirstName>
                        <LastName> Owais</LastName>
                        </Name>
                        <Hobby>Travelling</Hobby>
                        <Hobby>To Explore new places</Hobby>

                        <University campus='main'>UOK</University>

                        <Department >Computer Science</Department>


                    </Description>";


            XmlDocument myxml = new XmlDocument();
            try
            {
                myxml.LoadXml(xmlString);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            
            }
            string jsonText = JsonConvert.SerializeXmlNode(myxml);
            Console.WriteLine(jsonText);
            Console.WriteLine();
            //Deserialize a jsonText into xml
            myxml = JsonConvert.DeserializeXmlNode(jsonText);
           // Console.WriteLine(myxml.InnerXml);

            XmlNodeList nodes = myxml.SelectNodes("//Description");///University[@campus='main']");
            
            foreach (XmlNode node in nodes) {

         Console.WriteLine(node.InnerXml);
            
            }
            Console.WriteLine();
        }
    }
}
