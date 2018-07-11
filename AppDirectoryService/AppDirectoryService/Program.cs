using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AppDirectoryService
{
    public class Program
    {

        //static readonly IWebHost _host;
        static readonly string[] _urls = {  "http://localhost",
                                            "http://localhost:50889" };

        //static Program()
        //{
        //    IConfiguration _configuration = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .Build();
        //    _host = BuildHost(new WebHostBuilder(), _configuration, _urls);
        //}

        public static void Main(string[] args)
        {
            BuildWebHost(args, _urls).Run();
        }

        public static IWebHost BuildWebHost(string[] args, string[] urls)
        {

            var wh = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(urls)
                .Build()
                ;
            return wh;
        }

        //static IWebHost BuildHost(
        //    IWebHostBuilder builder, IConfiguration configuration, params string[] urls)
        //{
        //    var wh = builder
        //        .UseKestrel(options =>
        //        {
        //            //options.NoDelay = true;
        //        })
        //        .UseConfiguration(configuration)
        //        .UseUrls(urls)
        //        .Configure(app =>
        //        {
        //            //app.Map("/echo", EchoHandler);
        //            //app.Map("/what", WhatHandler);
        //            //app.Map("/report", ReportHandler);
        //        })
        //        .Build();
        //    return wh;
        //}





    }
}
