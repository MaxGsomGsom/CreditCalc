using System;
using Xunit;
using CreditCalc;
using CreditCalc.Models;
using System.IO;
using CreditCalc.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;

namespace UnitTests
{
    public class PercentsBinderTests
    {
        PercentsBinder binder = new PercentsBinder();

        Mock<ModelBindingContext> CreateMockedContext(string modelName, string value)
        {
            var mock = new Mock<ModelBindingContext>();
            mock.Setup(obj => obj.ModelName).Returns(modelName);
            mock.Setup(obj => obj.ValueProvider.GetValue(modelName))
                .Returns(new ValueProviderResult(value));
            mock.Setup(obj => obj.ModelState).Returns(new ModelStateDictionary());
            mock.SetupProperty(obj => obj.Result);

            return mock;
        }

        [Fact]
        public void BindStringWithPercent()
        {
            var modelName = "TestModel";
            var val = "23%";
            var expected = 0.23;
            var mock = CreateMockedContext(modelName, val);

            binder.BindModelAsync(mock.Object);

            Assert.True(mock.Object.ModelState[modelName].RawValue as string == val);
            Assert.Equal((double)mock.Object.Result.Model, expected);
        }

        [Fact]
        public void BindStringWithPoint()
        {
            var modelName = "TestModel";
            var val = "23.2";
            var expected = 23.2;
            var mock = CreateMockedContext(modelName, val);

            binder.BindModelAsync(mock.Object);

            Assert.True(mock.Object.ModelState[modelName].RawValue as string == val);
            Assert.Equal((double)mock.Object.Result.Model, expected);
        }

        [Fact]
        public void BindStringWithComma()
        {
            var modelName = "TestModel";
            var val = "23,2";
            var expected = 23.2;
            var mock = CreateMockedContext(modelName, val);

            binder.BindModelAsync(mock.Object);

            Assert.True(mock.Object.ModelState[modelName].RawValue as string == val);
            Assert.Equal((double)mock.Object.Result.Model, expected);
        }

        [Fact]
        public void BindWrongString()
        {
            var modelName = "TestModel";
            var val = "2wqe3,2";
            var mock = CreateMockedContext(modelName, val);

            binder.BindModelAsync(mock.Object);

            Assert.True(mock.Object.ModelState[modelName].RawValue as string == val);
            Assert.Null(mock.Object.Result.Model);
        }

        [Fact]
        public void BindEmptyString()
        {
            var modelName = "TestModel";
            var val = "";
            var mock = CreateMockedContext(modelName, val);

            binder.BindModelAsync(mock.Object);

            Assert.True(mock.Object.ModelState[modelName].RawValue as string == val);
            Assert.Null(mock.Object.Result.Model);
        }

        [Fact]
        public void BindNullString()
        {
            var modelName = "TestModel";
            string val = null;
            var mock = CreateMockedContext(modelName, val);

            binder.BindModelAsync(mock.Object);

            Assert.Null(mock.Object.ModelState[modelName]);
        }

    }
}
