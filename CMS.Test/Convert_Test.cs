using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace CMS.Test
{
    public class Convert_Test
    {
        [Fact]
        public void DataConvert_Test()
        {
            var guid = Guid.NewGuid();
            var castedGuid = Get<Guid>(guid.ToString());

            castedGuid.ShouldBe(guid);
        }

        public T Get<T>(string val)
        {
            if (!string.IsNullOrWhiteSpace(val))
                return (T) TypeDescriptor.GetConverter(typeof (T)).ConvertFromInvariantString(val);
            return default(T);
        }
    }


}
