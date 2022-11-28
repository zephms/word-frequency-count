using System;
using System.Collections.Generic;

namespace BlindSearchAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Algo.readtxt(@"C:\Users\mh\Documents\Tencent Files\2598229473\FileRecv\校花的贴身高手.txt");
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            Algo.BlindSearch(s, 4,5);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            Console.WriteLine("base耗时:" + ts2.Subtract(ts1).Duration().TotalSeconds.ToString());
        }
    }
}
