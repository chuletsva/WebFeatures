using Application.Interfaces.Services;
using System;

namespace Infrastructure.Services
{
    internal class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
