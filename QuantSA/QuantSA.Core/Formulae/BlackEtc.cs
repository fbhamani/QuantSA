﻿using System;
using MathNet.Numerics.Distributions;

namespace QuantSA.Core.Formulae
{
    public enum PutOrCall
    {
        Call = 1,
        Put = -1
    }

    public class BlackEtc
    {
        /// <summary>
        /// The Black the scholes formula
        /// </summary>
        /// <param name="putOrCall">put or call.</param>
        /// <param name="K">The strike.</param>
        /// <param name="T">The time to maturity in years.</param>
        /// <param name="S">The underlying spot price.</param>
        /// <param name="vol">The lognormal volatility of the underlying price.</param>
        /// <param name="rate">The continuous rate used for drifting the underlying and discounting the payoff.</param>
        /// <param name="div">The continuous dividend yield that decreased the drift on the underlying.</param>
        /// <returns></returns>
        public static double BlackScholes(PutOrCall putOrCall, double K, double T, double S, double vol, double rate,
            double div)
        {
            var dist = new Normal();
            var sigmaSqrtT = vol * Math.Sqrt(T);
            var d1 = 1 / sigmaSqrtT * (Math.Log(S / K) + rate - div + 0.5 * vol * vol);
            var d2 = d1 - sigmaSqrtT;
            var F = S * Math.Exp((rate - div) * T);
            return Math.Exp(-rate * T) * (F * dist.CumulativeDistribution(d1) - K * dist.CumulativeDistribution(d2));
        }

        /// <summary>
        /// The Black formula
        /// </summary>
        /// <param name="putOrCall">put or call.</param>
        /// <param name="strike">The strike.</param>
        /// <param name="T">The time to maturity in years from the value date to the exercise date.</param>
        /// <param name="forward">The forward at the option exercise date.</param>
        /// <param name="vol">The lognormal volatility of the underlying price.</param>
        /// <param name="discountFactor">The discount factor from the value date to the settlement date of the option.</param>
        /// <returns></returns>
        public static double Black(PutOrCall putOrCall, double strike, double T, double forward, double vol, double discountFactor)
        {
            var dist = new Normal();
            var sigmaSqrtT = vol * Math.Sqrt(T);
            var d1 = 1 / sigmaSqrtT * (Math.Log(forward / strike) + 0.5 * vol * vol);
            var d2 = d1 - sigmaSqrtT;
            return discountFactor * (forward * dist.CumulativeDistribution(d1) - strike * dist.CumulativeDistribution(d2));
        }
    }
}