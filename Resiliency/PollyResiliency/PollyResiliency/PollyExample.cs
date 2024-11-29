using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollyResiliency
{
    public static class PollyExample
    {
        private static int counter = 0;
        public static void SimulateOperation()
        {
            counter++;
            Console.WriteLine("Retried\n");

            //simulate operation that may throw an exception
            if(counter == 4)
            {
                counter = 0;
                Console.WriteLine("No Error");
            }
            else
            {
                throw new Exception("Simulated feature");
            }
        }

        //simulate operation that wil be called on fallback
        public static void SimulateOperationFallback()
        {
            Console.WriteLine("No Error from Fallback");
        }
    }
}
