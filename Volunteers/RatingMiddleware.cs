using BL;
using DL;
using Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volunteers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RatingMiddleware
    {
        VolunteersContext volunteerContext;
        IRatingBL ratingBL;
        private readonly RequestDelegate _next;

        public RatingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, VolunteersContext volunteerContext)
        {
            this.volunteerContext = volunteerContext;
            this.ratingBL = ratingBL;
            Rating rating = new Rating
            {
                Host = httpContext.Request.Host.Host,
                Method = httpContext.Request.Method,
                Path = httpContext.Request.Path.Value,
                UserAgent =httpContext.Request.Headers["User-Agent"].ToString(),
                Referer = httpContext.Request.Headers["Referrer"].ToString(),
                RecordDate = DateTime.Now
        };
            //ratingBL.PostRatingBLAsync(rating);
           await volunteerContext.Rating.AddAsync(rating);
            await volunteerContext.SaveChangesAsync();
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRatingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RatingMiddleware>();
        }
    }
}
