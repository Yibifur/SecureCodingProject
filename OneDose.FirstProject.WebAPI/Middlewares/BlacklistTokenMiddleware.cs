using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using OneDose.FirstProject.DataAccessLayer.Concrete;
using OneDose.FirstProject.WebAPI.Security;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using OneDose.FirstProject.WebAPI.Security.Abstract;
using OneDose.FirstProject.WebAPI.Model;
using System.Net;

namespace OneDose.FirstProject.WebAPI.Middlewares
{
    
        public class BlacklistTokenMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ITokenBlackListService _tokenBlacklistService;

            public BlacklistTokenMiddleware(RequestDelegate next, ITokenBlackListService tokenBlacklistService)
            {
                _next = next;
                _tokenBlacklistService = tokenBlacklistService;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                if (context.Request.Headers.TryGetValue("Authorization", out var token))
                {
                    var jwtToken = token.ToString().Replace("Bearer ", "");

                    // Eğer token kara listedeyse
                    if (await _tokenBlacklistService.IsTokenBlacklistedAsync(jwtToken))
                    {
                    var errorResponse = CustomResponseDTO<string>.Fail("Token's session is not valid",
                (int)HttpStatusCode.Unauthorized);
               
            

                    // Yanıtı JSON olarak yazdır
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };

                    var jsonResponse = JsonSerializer.Serialize(errorResponse, options);
                    await context.Response.WriteAsync(jsonResponse);
                    return;
                }
                }

                await _next(context);
            }
        }

    }

