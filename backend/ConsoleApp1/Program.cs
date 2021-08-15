using System;
using NetCoreBackend.Neo4j;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var test = new CQLOperation();
            test.ExcuteQuery();
        }
    }
}