using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.app.tests
{
    public class test_Kommandozeilenportal
    {
        public static void Main(string[] args)
        {
            var sut = new Kommandozeilenportal(args);
            Console.WriteLine("{0}, {1}, {2}", sut.Starthaltestellenname, sut.Zielhaltestellenname, sut.Startzeit);

            Console.Write("Press any key to quit"); Console.ReadKey();
        }
    }
}
