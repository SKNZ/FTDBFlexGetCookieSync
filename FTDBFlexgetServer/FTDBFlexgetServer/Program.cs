using System;
using System.Net;
using System.Collections.Generic;
using System.Configuration;
using Owin;
using Fos;
using Fos.Owin;
using Nancy;
using Nancy.Owin;

namespace FTDBFlexgetServer
{
    class Program
    {
        private static void ConfigureOwin(IAppBuilder builder)
        {
            var statisticsPipeline = builder.New();
            statisticsPipeline.UseStatisticsLogging(FosServer, new TimeSpan(0, 30, 0));

            var statisticsMapping = new Dictionary<string, IAppBuilder>() { { "/_stats", statisticsPipeline } };
            builder.Use<ShuntMiddleware>(statisticsMapping);

            builder.UseNancy();
        }

        private static FosSelfHost FosServer;
        public static void Main(string[] args)
        {
            using (FosServer = new FosSelfHost(ConfigureOwin))
            {
                StaticConfiguration.DisableErrorTraces = false;
                FosServer.Bind(IPAddress.Loopback, Int32.Parse(ConfigurationManager.AppSettings["port"])); // Loopback is forced, as it is not mean to be front-facing server
                FosServer.Start(false);
            }
        }
    }
}
