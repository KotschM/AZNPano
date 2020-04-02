using System;
using AZNPano.DBController;

namespace AZNPano.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Programmstart");
            Console.WriteLine("");
            PanoDBService dbservice = new PanoDBService();
            
            dbservice.Init("AZNPano.db");
             
        }
    }
}
