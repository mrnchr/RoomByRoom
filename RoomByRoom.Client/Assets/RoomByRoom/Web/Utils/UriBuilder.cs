using System.Collections.Generic;
using System.Linq;

namespace RoomByRoom.Web.Utils
{
    public class UriBuilder
    {
        private readonly List<string> _routes;

        public UriBuilder(IEnumerable<string> routes)
        {
            _routes = routes.ToList();
        }

        public void Add(params string[] routes)
        {
            _routes.AddRange(routes);
        }

        public void Remove(string route)
        {
            _routes.Remove(route);
        }

        public string Build()
        {
            return string.Join("/", _routes);
        }
    }
}