using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
//using Microsoft.Extensions.Configuration;

namespace AppDirectoryService
{

    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-2.1

    public class Startup
    {
        public IHostingEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }
        public IConfiguration Configuration2 { get; }

        static public string ConxString { get; set; }   // dont know how to get this at runtime...
        static public string ConxFileString { get; set; }   // dont know how to get this at runtime...
        static public string ServerFileString { get; set; }   // dont know how to get this at runtime...

        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            HostingEnvironment = env;
            Configuration = config;

            var _Configuration2 = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("Properties\\appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"Properties\\appsettings.{env.EnvironmentName}.json", optional: true)
                .Build();
            ;
            //Configuration2 = _Configuration2;
            //var foo = _Configuration2.GetSection("ConnectionStrings:AccountingDb");
            ConxString = _Configuration2.GetConnectionString("AccountingDb");
            ConxFileString = env.ContentRootPath + @"\Properties\AppSettings.json";
            ServerFileString = env.ContentRootPath + @"\Properties\ServerList.json";
            //var foocx = Configuration2["ConnectionStrings:AccountingDb"]?.ToString();
            //    _host = BuildHost(new WebHostBuilder(), _configuration, _urls);
            ;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, 
            //    Microsoft.AspNetCore.Http.HttpContextAccessor>();

            //services.Configure<Settings>(Configuration.GetSection("Settings"));
            //services.AddSingleton<IConfiguration>(Configuration);

            //    services.AddDbContext<BloggingContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("BloggingDatabase")));
            services.AddOptions();

            //var sliist = services.ToList();

            ;
            //services.Configure<ServiceSettings>(Configuration2.GetSection("ConnectionStrings"));
            //var fox = services.OfType<ServiceSettings>()
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/what", WhatHandler);
            app.Map("/report", ReportHandler);
            app.Map("/echo", EchoHandler);
            app.Map("/server", ServerHandler);

            app.Run(async (context) =>
            {
                //await context.Response.WriteAsync(HelloMessage(context));
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        RemoteIP = context.Connection.RemoteIpAddress.ToString(),
                        Now = DateTimeOffset.UtcNow.ToString("o"),
                        About = 
                        @"{ '/what?': 'what ran recently', 
                            '/report?': 'heres what just ran',
                            '/server?': 'where do i report in'}"
                        //Servers = QryServers(context.Request.Query, context.Connection, ConxString)
                    }));
            });
            ;
        }

        //static string HelloMessage(HttpContext cx)
        //{
        //    var msg = @"{{""ip"":""{0}"",""utc"":""{1}""}}";
        //    var resp = String.Format(msg, 
        //                    cx.Connection.RemoteIpAddress.ToString(), 
        //                    DateTimeOffset.UtcNow.ToString("o"));
        //    return resp;
                
        //}

        //[Route("/echo")]
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
                                StringComparer.OrdinalIgnoreCase) //,
                        //Foo = AppDirectoryService.Startup
                    })
                );
            });
        }

        //[Route("/server")]
        static void ServerHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        //StatusCode = (string)context.Response.StatusCode.ToString(),
                        //PathBase = (string)context.Request.PathBase.Value.Trim('/'),
                        //Path = (string)context.Request.Path.Value.Trim('/'),
                        //Method = (string)context.Request.Method,
                        //Scheme = (string)context.Request.Scheme,
                        //ContentType = (string)context.Request.ContentType,
                        //ContentLength = (long?)context.Request.ContentLength,
                        //QueryString = (string)context.Request.QueryString.ToString(),
                        //Query = context.Request.Query
                        //    .ToDictionary(
                        //        _ => _.Key,
                        //        _ => _.Value,
                        //        StringComparer.OrdinalIgnoreCase), //,
                        //Foo = AppDirectoryService.Startup
                        Servers = QryServers(context.Request.Query, context.Connection, ConxString)
                    })
                );
            });
        }


        static object QryServers(IQueryCollection query, ConnectionInfo connection, string ConxString)
        {
            object jsonObject;
            try
            {
                if (System.IO.File.Exists(ServerFileString))
                {
                    string allText = System.IO.File.ReadAllText(ServerFileString);
                    jsonObject = JsonConvert.DeserializeObject(allText);
                }
                else
                {
                    jsonObject = new System.IO.FileNotFoundException("Server File not found");
                }
            }
            catch (Exception ex)
            {
                jsonObject = ex; // JsonConvert.DeserializeObject(allText);
            }
            return jsonObject;
        }

        //[Route("/what")]
        static void WhatHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        //StatusCode = (string)context.Response.StatusCode.ToString(),
                        //PathBase = (string)context.Request.PathBase.Value, //.Trim('/'),
                        //Path = (string)context.Request.Path.Value.Trim('/'),
                        //Method = (string)context.Request.Method,
                        //Scheme = (string)context.Request.Scheme,
                        //ContentType = (string)context.Request.ContentType,
                        //ContentLength = (long?)context.Request.ContentLength,
                        //QueryString = (string)context.Request.QueryString.ToString(),
                        //Query = context.Request.Query
                        //    .ToDictionary(
                        //        _ => _.Key,
                        //        _ => _.Value,
                        //        StringComparer.OrdinalIgnoreCase),
                        //Result = qhistory(context.Request.QueryString.ToString()),
                        RecentHistory = (new History()).QryHistory(context.Request.Query, context.Connection, ConxString)
                    })
                );
            });
        }




        //[Route("/report")]
        static void ReportHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        //StatusCode = (string)context.Response.StatusCode.ToString(),
                        //PathBase = (string)context.Request.PathBase.Value.Trim('/'),
                        //Path = (string)context.Request.Path.Value.Trim('/'),
                        //Method = (string)context.Request.Method,
                        //Scheme = (string)context.Request.Scheme,
                        //ContentType = (string)context.Request.ContentType,
                        //ContentLength = (long?)context.Request.ContentLength,
                        //QueryString = (string)context.Request.QueryString.ToString(),
                        //Query = context.Request.Query
                        //    .ToDictionary(
                        //        _ => _.Key,
                        //        _ => _.Value,
                        //        StringComparer.OrdinalIgnoreCase),
                        InsertResults = (new History()).AddHistory(context.Request.Query, context.Connection, ConxString)
                    })
                );
            });
        }







    }
}
