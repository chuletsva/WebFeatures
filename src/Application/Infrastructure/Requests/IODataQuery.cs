using System.Linq;

namespace Application.Infrastructure.Requests
{
    public interface IODataQuery<T> : IQuery<IQueryable<T>>
    {
        public int? Top { get; set; }
        public int? Skip { get; set; }
        public string Filter { get; set; }
        public string OrderBy { get; set; }
    }

    public class ODataQuery<T> : IODataQuery<T>
    {
        public int? Top { get; set; }
        public int? Skip { get; set; }
        public string Filter { get; set; }
        public string OrderBy { get; set; }
    }
}
