using CreditCalc.JsonConverters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CreditCalc
{
    /// <summary>
    /// Calculated credit payments
    /// </summary>
    public class PaymentsParams
    {
        const double MaxMoney = 1000000000000;
        const double MinMoney = 0.01;

        /// <summary>
        /// Monthly payment
        /// </summary>
        [BindRequired]
        [Range(MinMoney, MaxMoney)]
        [JsonProperty("monthly payment")]
        [JsonConverter(typeof(MoneyJsonConverter))]
        public double MonthlyPayment { get; set; }

        /// <summary>
        /// Total amount of interest payments
        /// </summary>
        [BindRequired]
        [Range(MinMoney, MaxMoney)]
        [JsonProperty("total interest")]
        [JsonConverter(typeof(MoneyJsonConverter))]
        public double TotalInterest { get; set; }

        /// <summary>
        /// Total credit payments amount
        /// </summary>
        [BindRequired]
        [Range(MinMoney, MaxMoney)]
        [JsonProperty("total payment")]
        [JsonConverter(typeof(MoneyJsonConverter))]
        public double TotalPayment { get; set; }
    }
}
