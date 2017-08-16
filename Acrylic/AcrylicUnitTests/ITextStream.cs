using System;
using System.Collections.Generic;
using System.Text;

namespace AcrylicUnitTests
{
    public interface ITextStream
    {
    }

    public class TextStream : ITextStream { }
    public class AsciiStream : ITextStream { }
}
