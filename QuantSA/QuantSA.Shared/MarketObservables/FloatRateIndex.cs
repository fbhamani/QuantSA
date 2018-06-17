﻿using System;
using QuantSA.General;
using QuantSA.Shared.Dates;
using QuantSA.Shared.Primitives;

namespace QuantSA.Shared.MarketObservables
{
    /// <summary>
    /// An object to describe a floating rate index such as 3 Month Jibar.
    /// </summary>
    [Serializable]
    public class FloatRateIndex : MarketObservable
    {
        private string name;
        private readonly string toString;

        private FloatRateIndex(Currency currency, string name, Tenor tenor)
        {
            this.currency = currency;
            this.name = name;
            this.tenor = tenor;
            toString = currency + ":" + name.ToUpper() + ":" + tenor;
        }

        public Currency currency { get; }
        public Tenor tenor { get; }

        public override string ToString()
        {
            return toString;
        }

        #region Stored Indices        

        /// When you add new indices here remember to add them to: 
        /// ExcelUtilties.GetFloatingIndices0D 
        /// 
        public static FloatRateIndex JIBAR3M = new FloatRateIndex(Currency.ZAR, "Jibar", Tenor.Months(3));

        public static FloatRateIndex JIBAR6M = new FloatRateIndex(Currency.ZAR, "Jibar", Tenor.Months(6));
        public static FloatRateIndex JIBAR1M = new FloatRateIndex(Currency.ZAR, "Jibar", Tenor.Months(1));
        public static FloatRateIndex PRIME1M_AVG = new FloatRateIndex(Currency.ZAR, "Prime1MonthAvg", Tenor.Months(1));
        public static FloatRateIndex LIBOR3M = new FloatRateIndex(Currency.USD, "Libor", Tenor.Months(3));
        public static FloatRateIndex LIBOR6M = new FloatRateIndex(Currency.USD, "Libor", Tenor.Months(6));
        public static FloatRateIndex LIBOR1M = new FloatRateIndex(Currency.USD, "Libor", Tenor.Months(1));
        public static FloatRateIndex EURIBOR3M = new FloatRateIndex(Currency.EUR, "Euribor", Tenor.Months(3));
        public static FloatRateIndex EURIBOR6M = new FloatRateIndex(Currency.EUR, "Euribor", Tenor.Months(6));

        #endregion
    }
}