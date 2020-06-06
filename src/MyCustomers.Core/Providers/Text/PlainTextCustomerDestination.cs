using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Intercom.MyCustomers.Model;
using Microsoft.Extensions.Logging;

namespace Intercom.MyCustomers.Providers.Text
{
	public class PlainTextCustomerDestination : ICustomerDestination
	{
		private readonly string outputFile;
		private readonly ILogger<PlainTextCustomerDestination> logger;

		public PlainTextCustomerDestination(string outputFile, ILogger<PlainTextCustomerDestination> logger)
		{
			this.outputFile = outputFile;
			this.logger = logger;
		}

		protected virtual Stream OpenStream()
		{
			if (File.Exists(outputFile))
			{
				logger.LogWarning("File {outputFile} already exists. It'll be overwritten", outputFile);
			}

			return File.Open(outputFile, FileMode.Create, FileAccess.Write);
		}

		public async Task WriteCustomers(IEnumerable<Customer> customers)
		{
			await using var file = OpenStream();
			await using var writer = new StreamWriter(file);
			foreach (var customer in customers)
			{
				await writer.WriteLineAsync($"{customer.UserId} {customer.Name}").ConfigureAwait(false);
			}
		}
	}
}
