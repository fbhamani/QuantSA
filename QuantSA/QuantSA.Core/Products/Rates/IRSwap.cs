﻿using System.Collections.Generic;
using Newtonsoft.Json;
using QuantSA.General;
using QuantSA.Shared.Dates;
using QuantSA.Shared.MarketObservables;
using QuantSA.Shared.Primitives;

namespace QuantSA.Core.Products.Rates
{
    public class IRSwap : Product
    {
        private readonly double[] _accrualFractions;
        private readonly double _fixedRate;
        private readonly FloatRateIndex _index;
        private readonly Currency _ccy;
        private readonly Date[] _indexDates;
        private readonly double[] _notionals;
        private readonly double _payFixed; // -1 for payFixed, 1 for receive fixed
        private readonly Date[] _paymentDates;
        private readonly double[] _spreads;
        
        [JsonIgnore] private List<Date> _futureIndexDates;
        [JsonIgnore] private List<Date> _futurePayDates;

        // Product state
        [JsonIgnore] private double[] _indexValues;
        [JsonIgnore] private Date _valueDate;

        /// <summary>
        /// Explicit constructor for IRSwap.  When possible use one of the static constructors.
        /// </summary>
        /// <param name="payFixed"></param>
        /// <param name="indexDates"></param>
        /// <param name="payDates"></param>
        /// <param name="index"></param>
        /// <param name="spreads"></param>
        /// <param name="accrualFractions"></param>
        /// <param name="notionals"></param>
        /// <param name="fixedRate"></param>
        /// <param name="ccy"></param>
        public IRSwap(double payFixed, Date[] indexDates, Date[] payDates, FloatRateIndex index, double[] spreads,
            double[] accrualFractions,
            double[] notionals, double fixedRate, Currency ccy)
        {
            _payFixed = payFixed;
            _indexDates = indexDates;
            _paymentDates = payDates;
            _index = index;
            _spreads = spreads;
            _accrualFractions = accrualFractions;
            _notionals = notionals;
            _fixedRate = fixedRate;
            _ccy = ccy;
        }

        /// <summary>
        /// Returns the single floating rate index underlying this swap.
        /// </summary>
        /// <returns></returns>
        public FloatRateIndex GetFloatingIndex()
        {
            return _index;
        }


        /// <summary>
        /// Set the date after which all cashflows will be required.
        /// </summary>
        /// <param name="valueDate"></param>
        public override void SetValueDate(Date valueDate)
        {
            _indexValues = new double[_indexDates.Length];
            _valueDate = valueDate;
            _futurePayDates = new List<Date>();
            _futureIndexDates = new List<Date>();
            for (var i = 0; i < _paymentDates.Length; i++)
                if (_paymentDates[i] > _valueDate)
                {
                    _futurePayDates.Add(_paymentDates[i]);
                    _futureIndexDates.Add(_indexDates[i]);
                }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            _indexValues = new double[_indexDates.Length];
        }

        /// <summary>
        /// A swap only needs a single floating rate index.
        /// </summary>
        /// <returns></returns>
        public override List<MarketObservable> GetRequiredIndices()
        {
            return new List<MarketObservable> {_index};
        }

        /// <summary>
        /// The floating rate fixing dates that correspond to payment dates strictly after the value date.
        /// </summary>
        /// <param name="index">Will be the same index as returned by <see cref="GetRequiredIndices"/>.</param>
        /// <returns></returns>
        public override List<Date> GetRequiredIndexDates(MarketObservable index)
        {
            return _futureIndexDates;
        }

        /// <summary>
        /// Sets the values of the floating rates at all reset dates corresponding to payment dates after the value dates.
        /// </summary>
        /// <param name="index">Must only be called for the single index underlying the floating rate of the swap.</param>
        /// <param name="indexValues">An array of values the same length as the dates returned in <see cref="GetRequiredIndexDates(MarketObservable)"/>.</param>
        public override void SetIndexValues(MarketObservable index, double[] indexValues)
        {
            var indexCounter = 0;
            for (var i = 0; i < _paymentDates.Length; i++)
                if (_paymentDates[i] > _valueDate)
                {
                    _indexValues[i] = indexValues[indexCounter];
                    indexCounter++;
                }
        }

        /// <summary>
        /// The actual implementation of the contract cashflows.
        /// </summary>
        /// <returns></returns>
        public override List<Cashflow> GetCFs()
        {
            var cfs = new List<Cashflow>();
            for (var i = 0; i < _paymentDates.Length; i++)
                if (_paymentDates[i] > _valueDate)
                {
                    var fixedAmount = _payFixed * _notionals[i] * _accrualFractions[i] * _fixedRate;
                    var floatingAmount =
                        -_payFixed * _notionals[i] * _accrualFractions[i] * (_indexValues[i] + _spreads[i]);
                    cfs.Add(new Cashflow(_paymentDates[i], fixedAmount, _ccy));
                    cfs.Add(new Cashflow(_paymentDates[i], floatingAmount, _ccy));
                }

            return cfs;
        }

        public override List<Currency> GetCashflowCurrencies()
        {
            return new List<Currency> {_ccy};
        }

        public override List<Date> GetCashflowDates(Currency ccy)
        {
            return _futurePayDates;
        }
    }
}