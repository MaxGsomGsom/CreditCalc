using System;
using Xunit;
using CreditCalc;
using CreditCalc.Models;
using CreditCalc.JsonConverters;
using Newtonsoft.Json;
using System.IO;

namespace CreditCalc.UnitTests
{
    public class MoneyJsonConverterTests
    {
        MoneyJsonConverter converter = new MoneyJsonConverter();

        [Fact]
        public void CanConvertFromDouble()
        {
            Assert.True(converter.CanConvert(typeof(double)));
        }

        [Fact]
        public void WriteJsonFromDouble()
        {
            double val = 15.55555;
            var textWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter);
            var serializer = new JsonSerializer();

            converter.WriteJson(jsonWriter, val, serializer);
            Assert.Equal("15.56", textWriter.ToString());
        }
    }
}
