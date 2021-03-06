using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.RollingFileAlternative;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePocs.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
				var configuration = new ConfigurationBuilder()
				   .SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json")
				   .AddJsonFile($"appsettings.{environment ?? "Production"}.json", true, true)
				   .Build();

				Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(configuration)
					.CreateLogger();


				//Log.Logger = new LoggerConfiguration()
				//.MinimumLevel.Debug()
				//.WriteTo.Seq("http://localhost:5341")
				//.Enrich.WithMachineName()
				//.Enrich.WithThreadId()
				//.Enrich.WithProperty("ApplicationName", "NetCorePocs.API")
				//.Enrich.With(new PropertyEnricher("Environment", environment))
				////.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(@"Logs\Info\Info-{Date}.log"))
				////		.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(@"Logs\Debug\Debug-{Date}.log"))
				////		.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(@"Logs\Warning\Warning-{Date}.log"))
				////		.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(@"Logs\Error\Error-{Date}.log"))
				////		.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(@"Logs\Fatal\Fatal-{Date}.log"))
				////		.WriteTo.RollingFile(@"Logs\Verbose\Verbose-{Date}.log")
				//.WriteTo.Debug(new RenderedCompactJsonFormatter())
				////.WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
				//.CreateLogger();

				Log.Information("Starting Web Host");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Host terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
