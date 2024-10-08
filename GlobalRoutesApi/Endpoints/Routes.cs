namespace GlobalRoutes.Api.Endpoints
{
    public class Routes
    {
        private const string ApiRoot = "v{version:apiVersion}";

        public static class Account
        {
            public const string PostUser = ApiRoot + "/account/users";
        }
    }
}
