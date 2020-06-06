using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Intercom.MyCustomers.Providers;
using Intercom.MyCustomers.Providers.Json;
using Intercom.MyCustomers.Providers.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Intercom.MyCustomers
{
	public class Program
	{
		private static CommandLineOptions _options;

		public static Task Main(string[] args) =>
			BuildCommandLineParser()
				.UseHost(CreateHostBuilder)
				.UseDefaults()
				.Build()
				.InvokeAsync(args);

		private static CommandLineBuilder BuildCommandLineParser()
		{
			var root = new RootCommand()
			{
				new Option<string>(new[] {"-i", "--input"},
					description: "input file path with customer data") {Name = "InputFile", Required = true},
				new Option<string>(new[] {"-o", "--output"},
					description: "output file path to write data to") {Name = "OutputFile", Required = true}
			};
			root.Handler = CommandHandler.Create<CommandLineOptions, IHost>(Run);
			return new CommandLineBuilder(root);
		}

		private static async Task Run(CommandLineOptions options, IHost host)
		{
			_options = options;
			var command = host.Services.GetRequiredService<FindCustomersCommand>();
			await command.Execute();
		}


		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(builder =>
				{
					builder.Sources.Clear();

					/*
					 * Here we apply convention over configuration principle having the app "just work" without configuring anything
					 * But at the same time everything can be overriden by environment variables and/or configuration file
					 */
					builder.AddInMemoryCollection(new Dictionary<string, string>
					{
						{"Logging:MinimumLevel", "Information"},
						{"Logging:MinimumLevel:Override:Microsoft", "Warning"},
						{"Logging:MinimumLevel:Override:System", "Warning"}
					});
					builder.AddEnvironmentVariables();
					builder.AddJsonFile("appsettings.json", true);
				})
				.ConfigureLogging((context, builder) =>
				{
					builder.ClearProviders();
					builder.AddSerilog(new LoggerConfiguration()
						.WriteTo.ColoredConsole()
						.ReadFrom.Configuration(context.Configuration, sectionName: "Logging")
						.CreateLogger());
				})
				.ConfigureServices(ComposeServices);


		private static void ComposeServices(HostBuilderContext context, IServiceCollection services)
		{
			services.AddTransient<FindCustomersCommand>();

			/*
			 * This is done as the cli library isn't fully supporting GenericHost pattern  https://github.com/dotnet/command-line-api/issues/918
			 * Transient dependencies are not created at this time will be deferred after Options static variable is set
			 */
			services.AddTransient(p => _options);

			services.AddTransient<ICustomerSource, JsonFileCustomerSource>(p =>
				new JsonFileCustomerSource(p.GetRequiredService<CommandLineOptions>().InputFile));

			services.AddTransient<ICustomerDestination, PlainTextCustomerDestination>(p =>
				new PlainTextCustomerDestination(p.GetRequiredService<CommandLineOptions>().OutputFile,
					p.GetRequiredService<ILogger<PlainTextCustomerDestination>>()));
		}
	}
}
