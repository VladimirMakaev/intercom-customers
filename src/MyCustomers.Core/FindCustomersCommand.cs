using System.Threading.Tasks;
using Intercom.MyCustomers.Configuration;
using Intercom.MyCustomers.Model;
using Intercom.MyCustomers.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Intercom.MyCustomers
{
	public class FindCustomersCommand
	{
		private readonly IOptions<FindCustomersConfiguration> options;
		private readonly ICustomerSource inputSource;
		private readonly ICustomerDestination destination;
		private readonly ILogger<FindCustomersCommand> logger;

		public FindCustomersCommand(IOptions<FindCustomersConfiguration> options, ICustomerSource inputSource,
			ICustomerDestination destination, ILogger<FindCustomersCommand> logger)
		{
			this.options = options;
			this.inputSource = inputSource;
			this.destination = destination;
			this.logger = logger;
		}

		public async Task Execute()
		{
			var maximumDistance = Distance.From(options.Value.Distance, options.Value.Unit);

			var officeLocation = new Location(options.Value.OfficeLatitude, options.Value.OfficeLongitude);


			logger.LogInformation(
				"Checking for customers that are located within {Distance} {UnitType} of {OfficeLatitude}, {OfficeLongitude}",
				options.Value.Distance, options.Value.Unit, officeLocation.Latitude, officeLocation.Longitude);


			var result = await inputSource.FindCustomers(
				c => officeLocation.DistanceTo(c.Latitude, c.Longitude) < maximumDistance,
				orderBy: c => c.UserId);

			logger.LogInformation("Found {Customers} customers", result.Count);

			await destination.WriteCustomers(result);

			logger.LogInformation("Done");
		}
	}
}
