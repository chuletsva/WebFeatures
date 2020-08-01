using System;
using Application.Common.Interfaces.CommonServices;

namespace Infrastructure.CommonServices
{
    internal class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
