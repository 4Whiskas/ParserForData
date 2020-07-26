using System;

namespace ParserForData
{
    class Program
    {
        static void Main(string[] args)
        {
            Teachers.SendTeachersAsync();
            News.SendNews();
            Schedule.ParseSchedule();
            
            //Console.ReadKey();
        }
    }
}
