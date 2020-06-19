using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UserProfilesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(System.IO.Directory.GetCurrentDirectory());
                    //webBuilder.UseKestrel();
                    //webBuilder.ConfigureKestrel((context, serverOptions) =>
                    //{
                    //    // Set properties and call methods on serverOptions  
                    //    serverOptions.Limits.MaxConcurrentConnections = 100;
                    //    serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                    //    //serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
                    //    //serverOptions.Limits.MinRequestBodyDataRate =
                    //    //    new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    //    //serverOptions.Limits.MinResponseDataRate =
                    //    //    new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));

                    //});
                    webBuilder.UseIISIntegration();                                
                    webBuilder.UseStartup<Startup>();
                   

                });
    }
}
