using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMaster
{
    public class MyClass
    {
        private MyClass() { }
        public static int counter;
        public static int IncrementCounter()
        {
            return ++counter;
        }
    }

    class MainClass
    {
        static void Main()
        {
            // If you uncomment the following statement, it will generate
            // an error because the constructor is inaccessible:
            // MyClass myObject = new MyClass();   // Error
            MyClass.counter = 100;
            MyClass.IncrementCounter();
            Console.WriteLine("New count: {0}", MyClass.counter);
        }
    }
}
