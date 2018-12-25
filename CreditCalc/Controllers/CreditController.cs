using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditCalc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CreditCalc
{
    [Route("api/[controller]/[action]")]
    public class CreditController : Controller
    {
        /// <summary>
        /// Calculates payments based on credit parameters
        /// </summary>
        /// <param name="credit">Credit parameters</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public IActionResult Payments([BindRequired]CreditParams credit)
        {
            //Check model
            if (!ModelState.IsValid)
            {
                var res = Json(new { Error = BadRequest(ModelState).Value });
                res.StatusCode = 400;
                return res;
            }
            //Check downpayment
            else if (credit.Downpayment >= credit.Amount)
            {
                var res = Json(new { Error = "Downpayment sould be less than credit amount" });
                res.StatusCode = 400;
                return res;
            }

            //Calculate payments
            double withoutDownpayment = credit.Amount - credit.Downpayment;
            double percentsPow = Math.Pow(1d + credit.Interest, credit.Term);
            double yearPayment = withoutDownpayment * credit.Interest * (1d + 1d / (percentsPow - 1d));

            var payments = new PaymentsParams();
            payments.MonthlyPayment = yearPayment / 12d;
            payments.TotalPayment = yearPayment * credit.Term;
            payments.TotalInterest = payments.TotalPayment - credit.Amount;
            return Json(payments);
        }
    }
}
