using System;
using Xunit;
using CreditCalc;
using CreditCalc.Models;
using CreditCalc.JsonConverters;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace CreditCalc.UnitTests
{
    public class CreditControllerTests
    {
        [Fact]
        public void PaymentsCorrect()
        {
            var credit = new CreditParams() {
                Amount = 100000,
                Interest = 0.055,
                Downpayment = 20000,
                Term = 30
            };

            var controller = new CreditController();
            var actionResult = controller.Payments(credit) as JsonResult;
            var payments = actionResult.Value as PaymentsParams;

            Assert.Equal("458.70", payments.MonthlyPayment.ToString("F2", CultureInfo.InvariantCulture));
            Assert.Equal("65132.94", payments.TotalInterest.ToString("F2", CultureInfo.InvariantCulture));
            Assert.Equal("165132.94", payments.TotalPayment.ToString("F2", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void PaymentsWithoutDownpayment()
        {
            var credit = new CreditParams()
            {
                Amount = 100000,
                Interest = 0.055,
                Downpayment = 0,
                Term = 30
            };

            var controller = new CreditController();
            var actionResult = controller.Payments(credit) as JsonResult;
            var payments = actionResult.Value as PaymentsParams;

            Assert.Equal("573.38", payments.MonthlyPayment.ToString("F2", CultureInfo.InvariantCulture));
            Assert.Equal("106416.17", payments.TotalInterest.ToString("F2", CultureInfo.InvariantCulture));
            Assert.Equal("206416.17", payments.TotalPayment.ToString("F2", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void PaymentsInvalidModel()
        {
            var credit = new CreditParams();

            var controller = new CreditController();
            controller.ModelState.AddModelError("some", "error");
            var actionResult = controller.Payments(credit) as JsonResult;
            var errorField = actionResult.Value.GetType().GetProperty("Error");

            Assert.NotNull(errorField);
        }

        [Fact]
        public void PaymentsIncorrectDownpayment()
        {
            var credit = new CreditParams()
            {
                Amount = 100000,
                Interest = 0.055,
                Downpayment = 110000,
                Term = 30
            };

            var controller = new CreditController();
            var actionResult = controller.Payments(credit) as JsonResult;
            var errorField = actionResult.Value.GetType().GetProperty("Error");

            Assert.NotNull(errorField);
        }
    }
}
