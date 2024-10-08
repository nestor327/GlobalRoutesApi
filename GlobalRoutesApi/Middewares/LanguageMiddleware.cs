namespace GlobalRoutes.Api.Middewares
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private const string LanguageCodeDefault = "en";
        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (var scope = context.RequestServices.CreateScope())
            {
                await _next.Invoke(context);
                return;
            }
        }
    }
}
