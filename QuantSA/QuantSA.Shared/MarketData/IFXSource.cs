﻿using QuantSA.General;
using QuantSA.Shared.Dates;
using QuantSA.Shared.MarketObservables;
using QuantSA.Shared.Primitives;

namespace QuantSA.Shared.MarketData
{
    /// <summary>
    /// A general object to provide current spot and forecasts.  Currently there is no support for 
    /// settlement conventions.  The rate it returns is the rate observed on the date supplied.  It will 
    /// in general apply for some later settlement date.
    /// </summary>
    public interface IFXSource
    {
        Currency GetBaseCurrency();
        Currency GetCounterCurrency();
        CurrencyPair GetCurrencyPair();

        /// <summary>
        /// Get the exchange rate at the supplied date.  No accommodation is made for settlement conventions and 
        /// the rate will in general apply at a later date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        double GetRate(Date date);
    }
}