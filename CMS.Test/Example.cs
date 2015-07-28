using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CMS.Test
{
    public class Example
    {
        ITestOutputHelper output;

        public Example(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestThis()
        {
            output.WriteLine("I'm inside the test!");
        }
    }
}
