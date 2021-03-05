using LotGenerator_Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LotGeneratorMainService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainLotService.createMainLotService();

                Console.WriteLine("LotGenerator Service is up and running.");
                Console.WriteLine("Press <Enter> to stop service...");
                Console.Title = "LotGenerator Service 2.4";
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
