using System.Net;
using System.Text;

namespace BasicDotNet6API.Helpers
{
    public class BasicAuthHelpers
    {
        public (string user, string password)Parse(HttpRequest httpRequest)
        {
            var authorization = httpRequest.Headers["Authorization"];

            var userValue = Encoding.UTF8.GetString(Convert.FromBase64String(authorization.ToString()[6..])).Split(":");

            return (userValue[0], userValue[1]);
        }
    }
}
