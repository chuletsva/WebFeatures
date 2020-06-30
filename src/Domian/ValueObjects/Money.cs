using Domian.Common;
using Domian.Entities;
using System;
using System.Collections.Generic;

namespace Domian.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Value { get; set; }

        public Currency Currency { get; set; }
        public Guid CurrencyId { get; set; }

        protected override IEnumerable<object> GetComparisionValues()
        {
            yield return Value;
            yield return CurrencyId;
        }
    }
}
