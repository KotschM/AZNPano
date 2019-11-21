using System;
using AZNPano.DBController;

namespace AZNPano
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            PanoDBService dbservice = new PanoDBService();
            
            dbservice.Init("AZNPano.db");
             
        }
    }
}
