using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
//using System;
//using System.Linq;


// https://stackoverflow.com/questions/44356052/minimal-footprint-bare-bones-asp-net-core-webapi?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa

namespace AppAccountingService
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //    }
    //}


//namespace TinyWebApi
//    {
        class Program
        {
            static readonly IWebHost _host;
            static readonly string[] _urls = { "http://localhost:8080" };

            static Program()
            {
                IConfiguration _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
                _host = BuildHost(new WebHostBuilder(), _configuration, _urls);
            }

            static void Main(string[] args)
            {
                _host.Run();
            }

            static IWebHost BuildHost(
                IWebHostBuilder builder, IConfiguration configuration, params string[] urls)
            {
                return builder
                    .UseKestrel(options =>
                    {
                        //options.NoDelay = true;
                    })
                    .UseConfiguration(configuration)
                    .UseUrls(urls)
                    .Configure(app =>
                    {
                        app.Map("/echo", EchoHandler);
                    })
                    .Build();
            }

            static void EchoHandler(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync(
                        JsonConvert.SerializeObject(new
                        {
                            StatusCode = (string)context.Response.StatusCode.ToString(),
                            PathBase = (string)context.Request.PathBase.Value.Trim('/'),
                            Path = (string)context.Request.Path.Value.Trim('/'),
                            Method = (string)context.Request.Method,
                            Scheme = (string)context.Request.Scheme,
                            ContentType = (string)context.Request.ContentType,
                            ContentLength = (long?)context.Request.ContentLength,
                            QueryString = (string)context.Request.QueryString.ToString(),
                            Query = context.Request.Query
                                .ToDictionary(
                                    _ => _.Key,
                                    _ => _.Value,
                                    StringComparer.OrdinalIgnoreCase)
                        })
                    );
                });
            }
        }
 //   }


}
