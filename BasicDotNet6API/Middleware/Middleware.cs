namespace BasicDotNet6API.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("input");

            await _next(context);

            Console.WriteLine("output");
        }
    }
}
