using Domian.Common;
using Domian.Entities;
using System;
using System.Collections.Generic;

namespace Domian.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; set; }

        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        protected override IEnumerable<object> GetComparisionValues()
        {
            yield return Amount;
            yield return CurrencyId;
        }
    }
}
