using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CreditCalc.Models;

namespace IntergrationTests
{
    public class PaymentsTests : IClassFixture<WebApplicationFactory<CreditCalc.Startup>>
    {
        readonly WebApplicationFactory<CreditCalc.Startup> _factory;
        readonly string url = "/api/credit/payments";


        public PaymentsTests(WebApplicationFactory<CreditCalc.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("5.5%")]
        [InlineData(" 5.5 % ")]
        [InlineData("0.055")]
        [InlineData(" 0.055 ")]
        public async void SendCorrectParams(string interest)
        {
            var keyValues = new Dictionary<string, string>()
            {
                { "amount", "100000" },
                { "interest", interest },
                { "downpayment", "20000" },
                { "term", "30" }
            };

            var client = _factory.CreateClient();
            var content = new FormUrlEncodedContent(keyValues);

            var postResult = await client.PostAsync(url, content);
            var getResult = await client.GetAsync(url + "?" + await content.ReadAsStringAsync());
            JToken postJson = JToken.Parse(await postResult.Content.ReadAsStringAsync());
            JToken getJson = JToken.Parse(await getResult.Content.ReadAsStringAsync());

            Assert.Equal(postJson.ToString(), getJson.ToString());
            Assert.Equal("458.70", postJson["monthly payment"]);
            Assert.Equal("65132.94", postJson["total interest"]);
            Assert.Equal("165132.94", postJson["total payment"]);
        }

        [Fact]
        public async void SendCorrectParamsWithoutDownpayment()
        {
            var keyValues = new Dictionary<string, string>()
            {
                { "amount", "100000" },
                { "interest", "5.5%" },
                { "term", "30" }
            };

            var client = _factory.CreateClient();
            var content = new FormUrlEncodedContent(keyValues);

            var postResult = await client.PostAsync(url, content);
            var getResult = await client.GetAsync(url + "?" + await content.ReadAsStringAsync());
            JToken postJson = JToken.Parse(await postResult.Content.ReadAsStringAsync());
            JToken getJson = JToken.Parse(await getResult.Content.ReadAsStringAsync());

            Assert.Equal(postJson.ToString(), getJson.ToString());
            Assert.Equal("573.38", postJson["monthly payment"]);
            Assert.Equal("106416.17", postJson["total interest"]);
            Assert.Equal("206416.17", postJson["total payment"]);
        }


        [Theory]
        [InlineData("amount", "0", "interest", "5.5%", "downpayment", "20000", "term", "30")]
        [InlineData("amount", "100000", "interest", "5.h5%", "downpayment", "20000", "term", "30")]
        [InlineData("amount", "100000", "interest", "5.5%", "downpayment", "999999", "term", "30")]
        [InlineData("amount", "100000", "interest", "5.5%", "downpayment", "20000", "term", "0")]
        [InlineData("amount", "100000", "interest", "5.5%", "downpayment", "-20000", "term", "30")]
        [InlineData("amount", "100s000", "interest", "5.5%", "downpayment", "20000", "term", "30")]
        [InlineData("amount", "100000", "interest", "40000", "downpayment", "20000", "term", "30")]
        [InlineData("amount", "100000", "interest", "5.5%", "downpayment", "20000")]
        [InlineData]
        public async void SendIncorrectParams(params string[] obj)
        {
            var keyValues = new Dictionary<string, string>();
            for (int i = 0; i < obj.Length; i+=2)
                keyValues.Add(obj[i], obj[i + 1]);

            var client = _factory.CreateClient();
            var content = new FormUrlEncodedContent(keyValues);

            var postResult = await client.PostAsync(url, content);
            var getResult = await client.GetAsync(url + "?" + await content.ReadAsStringAsync());
            JToken postJson = JToken.Parse(await postResult.Content.ReadAsStringAsync());
            JToken getJson = JToken.Parse(await getResult.Content.ReadAsStringAsync());

            Assert.Equal(postJson.ToString(), getJson.ToString());
            Assert.NotNull(postJson["error"]);
        }
    }
}
