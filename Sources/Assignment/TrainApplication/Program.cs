using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainApplication;

namespace TrainApplication
{
    

    class Program
    {
        static void Main(string[] args)
        {
            var d = new Dictionary<int>(10);
            for(int i = 0; i<30;i++)
            {
                d.Add(i.ToString(), i);
            }

            Console.Read();
        }
    }
}
