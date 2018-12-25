using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditCalc.ModelBinders;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CreditCalc
{
    /// <summary>
    /// Basic parameters of credit
    /// </summary>
    public class CreditParams
    {
        const double MaxMoney = 1000000000000;
        const double MinMoney = 0.01;

        /// <summary>
        /// Amount of credit
        /// </summary>
        [BindRequired]
        [Range(MinMoney, MaxMoney)]
        public double Amount { get; set; }

        /// <summary>
        /// Interest of credit in percents
        /// </summary>
        [BindRequired]
        [Range(MinMoney, 1000)]
        [ModelBinder(typeof(PercentsBinder))]
        public double Interest { get; set; }

        /// <summary>
        /// Downpayment of credit
        /// </summary>
        [Range(0, MaxMoney)]
        public double Downpayment { get; set; }

        /// <summary>
        /// Term of credit in years
        /// </summary>
        [BindRequired]
        [Range(1, 100)]
        public double Term { get; set; }
    }

}
