namespace OneDose.FirstProject.WebAPI.Middlewares
{
    

    using Microsoft.AspNetCore.Http;
    using System.IO;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using OneDose.FirstProject.WebAPI.Model;
    using OneDose.FirstProject.WebAPI.Security.Abstract;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.IdentityModel.Tokens;
    using OneDose.FirstProject.WebAPI.Security;
    using Azure;

    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlackListService _tokenBlacklistService;
        public ApiResponseMiddleware(RequestDelegate next, ITokenBlackListService tokenBlacklistService)
        {
            _next = next;
            _tokenBlacklistService = tokenBlacklistService;
        }
        private static async Task<object> ParseResponseBodyAsync(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin); // Stream'in başına dön

            using var reader = new StreamReader(bodyStream);
            var responseBody = await reader.ReadToEndAsync();
            try
            {
                // JSON string'ini deserialize et
                var resultObject = JsonSerializer.Deserialize<object>(responseBody);
                return resultObject;
            }
            catch (JsonException ex)
            {
                // Hata durumunda error message döndür
                return null;
            }
            /* try
             {
                 // JSON string'ini deserialize ederek nesneye çeviriyoruz

                 var jsonDoc = JsonDocument.Parse(responseBody);
                 var root = jsonDoc.RootElement;
                 if (root.TryGetProperty("errors", out JsonElement errorElement))
                 {



                     var cleanedJson = errorElement.ToString().Replace("\\r", "").Replace("\\n", "").Replace("\\", "");

                     // JSON string'ini deserialize ederek nesneye çevir
                     var errorObject = JsonSerializer.Deserialize<object>(cleanedJson);
                     return errorObject;
                 }
                 if (root.TryGetProperty("result", out JsonElement resultElement))
                 {
                     return resultElement;   
                 }
                 return null;
             }
             catch (JsonException)
             {
                 // JSON formatı hatalıysa, string olarak döneriz
                 return responseBody;
             }*/
        }
        public async Task InvokeAsync(HttpContext context)
        {
           
            var originalBodyStream = context.Response.Body;

            using (var newBodyStream = new MemoryStream())
            {
                context.Response.Body = newBodyStream;

                
                await _next(context);

                newBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await ParseResponseBodyAsync(newBodyStream);
                if (context.Request.Headers.TryGetValue("Authorization", out var token) &&
    token.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var jwtToken = token.ToString().Replace("Bearer ", "");

                    // Kara liste kontrolünü önbellek (cache) ile optimize edin.
                    if (await _tokenBlacklistService.IsTokenBlacklistedAsync(jwtToken))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        var apiResponseBlack = BuildApiResponse(
                            true,
                            null,
                            "Token's session is not valid."
                        );

                        context.Response.Body = originalBodyStream;
                        context.Response.ContentType = "application/json";
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };

                        string jsonResponse = JsonSerializer.Serialize(apiResponseBlack, options);
                        await context.Response.WriteAsync(jsonResponse);
                        return;
                    }
                }
                var errorMessage = context.Response.StatusCode >= 400
                ? responseBody
                : null;
                if(context.Response.StatusCode >= 400)
                {
                    var apiResponse1 = BuildApiResponse( true, null, errorMessage);
                    context.Response.Body = originalBodyStream;
                    context.Response.ContentType = "application/json";
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    string jsonResponse = JsonSerializer.Serialize(apiResponse1, options);

                    await context.Response.WriteAsync(jsonResponse);
                    return;
                }
                else
                {
                    var apiResponse = BuildApiResponse( false, responseBody, errorMessage);
                    
                    context.Response.Body = originalBodyStream;
                    context.Response.ContentType = "application/json";


                    await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse));

                    return;
                }
                

                
            }
        }

        private static  ApiResponse<object> BuildApiResponse(bool hasError, object responseBody,object errorMessage)
        {
             return new ApiResponse<object>(
                hasError,
                responseBody,
                errorMessage
            );  
        }
    }


}
