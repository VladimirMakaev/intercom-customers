using System.IO;
using System.Threading.Tasks;
using Intercom.MyCustomers.Configuration;
using Intercom.MyCustomers.Providers.Json;
using Intercom.MyCustomers.Providers.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Intercom.MyCustomers.Tests.Integration
{
	public class FindCustomerCommandTests
	{
		private readonly IOptions<FindCustomersConfiguration> defaultConfiguration =
			new OptionsWrapper<FindCustomersConfiguration>(new FindCustomersConfiguration());

		[Fact]
		public async Task Customers_within_100km_should_be_written_to_output()
		{
			var input = new JsonFileCustomerSource(GetTestLocationDirectory("data/customers.txt"));
			var destination = new PlainTextCustomerDestination(GetTestLocationDirectory("output.txt"),
				new NullLogger<PlainTextCustomerDestination>());

			var subject = new FindCustomersCommand(defaultConfiguration, input, destination, new NullLogger<FindCustomersCommand>());

			await subject.Execute();

			Assert.Equal(LineUtility.CombineWithLineBreak(
					"4 Ian Kehoe",
					"5 Nora Dempsey",
					"6 Theresa Enright",
					"8 Eoin Ahearn",
					"11 Richard Finnegan",
					"12 Christina McArdle",
					"13 Olive Ahearn",
					"15 Michael Ahearn",
					"17 Patricia Cahill",
					"23 Eoin Gallagher",
					"24 Rose Enright",
					"26 Stephen McArdle",
					"29 Oliver Ahearn",
					"30 Nick Enright",
					"31 Alan Behan",
					"39 Lisa Ahearn"),
				File.ReadAllText(GetTestLocationDirectory("output.txt")));
		}


		//This trickery is required to make sure tests are passing on both Windows and Linux

		private static string GetTestLocationDirectory(string relativeTestFile) =>
			Path.Combine(relativeTestFile, Path.GetDirectoryName(typeof(FindCustomerCommandTests).Assembly.FullName));
	}
}
