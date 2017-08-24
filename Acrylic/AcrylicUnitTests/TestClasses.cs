using Acrylic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcrylicUnitTests
{
    public interface ICalculator
    {
        T Add<T>(T a, T b) where T : struct;
    }

    public class Calculator : ICalculator
    {
        public Calculator() { }
        public T Add<T>(T a, T b) where T : struct
        {
            return a;
        }
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
        public int Capacity { get; private set; }
        public double Precision { get; private set; }

        public ConcreteClass()
           : this(10,2)
        {         
        }
        public ConcreteClass(int capacity, double precision)
            : base()
        {
            Capacity = capacity;
            Precision = precision;
        }
        public override void Method()
        {
            Console.WriteLine("Implemented Method");
        }
    }

    public class InternalConstructorClass : BaseClass
    {
        internal InternalConstructorClass() { }
        public override void Method()
        {
            throw new NotImplementedException();
        }
    }

    public interface IEmail { }
    public class EmailService : IEmail {
        public EmailService(StringBuilder builder)
        {
            //Just creating a dependency on an unregistered simple type.
            Console.WriteLine(builder.ToString());
        }
    }
    public class Controller
    {

    }
    public class ViewController : Controller
    {
        private IEmail _emailService;
        private ICalculator _calculator;
        public ViewController(IContainer iocContainer )
        {
            _emailService = iocContainer.Resolve<IEmail>();
            _calculator = iocContainer.Resolve<ICalculator>();
        }

        public ViewController(IEmail email, ICalculator calculator)
        {
            _emailService = email;
            _calculator = calculator;
        }

        public IEmail EmailService => _emailService;
        public ICalculator Calculator => _calculator;
    }


}
