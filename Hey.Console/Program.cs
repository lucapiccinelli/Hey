using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Service;

namespace Hey.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new HeyService();
            service.Start(new string[0]);
            System.Console.WriteLine("Press any key to stop...");
            System.Console.ReadKey();
        }
    }
}
