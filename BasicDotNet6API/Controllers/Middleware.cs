using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReformAPI.Middleware
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
            string requestBody = await GetRequestBodyAsync(context.Request);

            string method = context.Request.Method;
            string path = context.Request.Path;

            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var problemDetails = await GetProblemDetailsFromResponseAsync(context.Response);

            if (problemDetails is not null)
            {
                string message = $"{problemDetails.Title} {Environment.NewLine} detail:{problemDetails.Detail}";
            }

            int statusCode = context.Response.StatusCode;


            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            try
            {
                request.EnableBuffering();
                var stream = new StreamReader(request.Body);
                string bodyText = await stream.ReadToEndAsync();
                request.Body.Seek(0, SeekOrigin.Begin);
                return bodyText;
            }
            catch (Exception ex)
            {
                return $"LogMiddleware get request body error!!{Environment.NewLine} Message: {ex.Message}, {Environment.NewLine} StackTrace: {Environment.NewLine} {ex.StackTrace}";
            }
        }
        private async Task<ProblemDetails> GetProblemDetailsFromResponseAsync(HttpResponse response)
        {
            string bodyText = string.Empty;
            try
            {
                if (response.StatusCode is StatusCodes.Status500InternalServerError)
                {
                    response.Body.Seek(0, SeekOrigin.Begin);

                    bodyText = await new StreamReader(response.Body).ReadToEndAsync();

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        IgnoreNullValues = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    options.Converters.Add(new JsonStringEnumConverter());

                    return JsonSerializer.Deserialize<ProblemDetails>(bodyText, options);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new ProblemDetails()
                {
                    Title = "LogMiddleware get response ProblemDetails error!!",
                    Detail = $"Original response body:{bodyText}, {ex.Message}\nStackTrace: \n{ex.StackTrace}",
                };
            }
            finally
            {
                response.Body.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
