﻿using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Product_Details.Interface;
using Product_Details.Model;


namespace DotNet8WebAPI.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly AppSettings _appSetting;
        //RequestDelegate is a function its process to http requestand pointer to next middleware
        public JwtMiddleware(RequestDelegate next,  IOptions<AppSettings> app)
        {
            _Next = next;
            _appSetting = app.Value;
        }
        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token =
            context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last(); 


            if (token != null)
                await attachUserToContext(context, userService, token);
            await _Next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)

        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
                tokenHandler.ValidateToken(token, new
                TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clock skew to zero so tokens expire exactly at token expiration time(instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                //Attach user to context on successful JWT validation 
                context.Items["User"] = await userService.GetById(userId);
            }
            catch
            {
                //Do nothing if JWT validation fails 
                // user is not attached to context so the request won't 
                // have access to secure routes
            }
        }


    }

}