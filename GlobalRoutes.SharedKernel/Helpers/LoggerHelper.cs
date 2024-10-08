using Ardalis.Result;
using System.Text.Json;

namespace GlobalRoutes.SharedKernel.Helpers
{
    public class LoggerHelper
    {
        public static string RequestToString(object request)
        {
            return JsonSerializer.Serialize(request).Replace("{", "[").Replace("}", "]");
        }

        public static string EndpointRequest(string endpoint, object request, string user = "")
        {
            var hasUser = user.Equals("") ? "" : "by user: " + user;

            return $"Request to consume the endpoint: {endpoint}; with the parameters: {RequestToString(request)}; {hasUser}";
        }

        public static string EndpointRequestSuccessfully(string endpoint, object request, string user = "")
        {
            var hasUser = user.Equals("") ? "" : "by user: " + user;

            return $"Request: {endpoint}; completed successfully with the parameters: {RequestToString(request)}; {hasUser}";
        }

        public static string EndpointRequestError(string endpoint, object request, string error, string user = "")
        {
            var hasUser = user.Equals("") ? "" : "by user: " + user;

            return $"An error has occurred when consuming the endpoint: {endpoint}; with the parameters: {RequestToString(request)}; for the reason: {error}; {hasUser}";
        }

        public static string EndpointWithOutRequest(string endpoint, string user = "")
        {
            var hasUser = user.Equals("") ? "" : "by user: " + user;

            return $"Request to consume the endpoint: {endpoint}; {hasUser}";
        }

        public static string EndpointWithOutRequestSuccessfully(string endpoint, object request, string user = "")
        {
            var hasUser = user.Equals("") ? "" : "by user: " + user;

            return $"Request: {endpoint}; completed successfully; {hasUser}";
        }

        public static string EndpointWithOutRequestError(string endpoint, object request, string error, string user = "")
        {
            var hasUser = user.Equals("") ? "" : "by user: " + user;

            return $"An error has occurred when consuming the endpoint: {endpoint}; for the reason: {error}; {hasUser}";
        }
    }
}
