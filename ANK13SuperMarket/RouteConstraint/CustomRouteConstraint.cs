using System.Text.RegularExpressions;

namespace ANK13SuperMarket.RouteConstraint
{
    public class CustomRouteConstraint : IRouteConstraint
    {
       
        public bool Match(HttpContext? httpContext, IRouter? route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(parameterName, out object parameterValue))
            {
                string value = parameterValue.ToString();

                
                bool isValid = Regex.IsMatch(value, @"^[A-Za-z0-9]{1}[A-Za-z0-9]{1}[A-Za-z0-9]{1}[A-Za-z0-9]{1}[A-Za-z0-9]{1}$")
                            || Regex.IsMatch(value, @"^[0-9]{1}[A-Za-z0-9]{1}[0-9]{1}[A-Za-z0-9]{1}[0-9]{1}$");

                return isValid;
            }

            return false;
        }
    }
}
