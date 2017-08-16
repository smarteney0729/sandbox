using System;
using System.Collections.Generic;
using System.Text;

namespace AcrylicUnitTests
{
    internal interface ICalculator
    {
        T Add<T>(T a, T b) where T : struct;
    }




    public interface ITextStream
    {
    }

    public class TextStream : ITextStream { }
    public class AsciiStream : ITextStream { }

    public abstract class BaseClass
    {
        abstract public void Method();

        protected virtual int MethodA(int a, int b)
        {
            return a + b;
        }
    }

    public class ConcreteClass : BaseClass
    {
        public override void Method()
        {
            Console.WriteLine("Implemented Method");
        }
    }
}
