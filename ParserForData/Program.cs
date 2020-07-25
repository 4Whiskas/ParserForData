using System;

namespace ParserForData
{
    class Program
    {
        static void Main(string[] args)
        {
            News.SendNews();
            Schedule.ParseSchedule();
            
            //Console.ReadKey();
        }
    }
}
