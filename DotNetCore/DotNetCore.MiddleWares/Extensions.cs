using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNetCore.MiddleWares
{
    public static class Extensions
    {
        public static IApplicationBuilder RunHelloWorld(this IApplicationBuilder builder)
        {
            //只会完成自己对HTTP请求的处理，并不会将请求传至下一层的Middleware。
            builder.Run(context =>
            {
                return context.Response.WriteAsync("Hello World");
            });

            return builder;
        }

        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<MyUseMiddleWare>();
            return builder;
        }

        public static IApplicationBuilder MapMyMiddleWare(this IApplicationBuilder builder)
        {
            builder.Map("/branch1", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Map branch 1");
                });
            });

            builder.Map("/branch2", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Map branch 2");
                });
            });
            return builder;
        }

        public static IApplicationBuilder MapMyMapWhenMiddleWare(this IApplicationBuilder builder)
        {
            string pattern = @"^\/MP_verify_(.+)\.txt$";

            builder.MapWhen(
                context =>
                {
                    return Regex.IsMatch(context.Request.Path, pattern, RegexOptions.IgnoreCase);
                },
                app =>
                {
                    app.Run(async ctx =>
                    {
                        await ctx.Response.WriteAsync("MP terminated", encoding: Encoding.UTF8);
                    });
                }
             );
            return builder;
        }
    }

    public class MyUseMiddleWare
    {
        private readonly RequestDelegate _next;
        public MyUseMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Invoke方法可以忽略里面的_next调用，并返回一个响应；
            //也可以调用_next.Invoke()把请求发送到管道的下一站。
            if (context.Request.Path == "/stop/use")
            {
                await context.Response.WriteAsync("I am a Use Middleware!\n");
            }
            else
            {
                await _next?.Invoke(context);
            }
        }
    }

    public static class IpLogMiddlewareExtension
    {
        public static IApplicationBuilder UseIpLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            })
            .UseMiddleware<IpLogMiddleware>();
        }
    }

    public class IpLogMiddleware
    {
        private readonly RequestDelegate _next;

        public IpLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            string ip = context.Connection.RemoteIpAddress.ToString();
            string originIp = string.Empty;
            if (context.Request.Headers.ContainsKey(RemoteIpHeaderKey))
            {
                originIp = context.Request.Headers[RemoteIpHeaderKey].ToString();
            }
            using (LogContext.PushProperty("IP", ip))
            using (LogContext.PushProperty("OriginIP", originIp))
                await _next(context);
        }

        const string RemoteIpHeaderKey = "X-Forwarded-For";
    }

}
