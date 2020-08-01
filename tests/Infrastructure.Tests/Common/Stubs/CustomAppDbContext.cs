using Application.Interfaces.CommonServices;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Common.Stubs
{
    public class CustomAppDbContext : AppDbContext
    {
        public CustomAppDbContext(
            DbContextOptions<AppDbContext> options,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IMediator mediator) : base(options, currentUser, dateTime, mediator)
        {
        }

        public DbSet<CustomAuditableEntity> Auditable { get; set; }
        public DbSet<CustomSoftDeleteEntity> SoftDelete { get; set; }
    }
}