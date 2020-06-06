using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Intercom.MyCustomers.Model;
using Newtonsoft.Json;

namespace Intercom.MyCustomers.Providers.Json
{
	public class JsonFileCustomerSource : ICustomerSource
	{
		private readonly string filePath;

		public JsonFileCustomerSource(string filePath)
		{
			this.filePath = filePath;
		}

		public async Task<IReadOnlyList<Customer>> FindCustomers<TKey>(
			Func<Customer, bool> customerPredicate,
			Func<Customer, TKey> orderBy)
		{
			var listAsync = await FindCustomersInternal().Where(customerPredicate)
				.OrderBy(orderBy)
				.ToListAsync()
				.ConfigureAwait(false);

			return new ReadOnlyCollection<Customer>(listAsync);
		}

		private async IAsyncEnumerable<Customer> FindCustomersInternal()
		{
			var stream = OpenStream();
			using var reader = new StreamReader(stream);
			while (!reader.EndOfStream)
			{
				var line = await reader.ReadLineAsync().ConfigureAwait(false);
				var nextCustomer = JsonConvert.DeserializeObject<Customer>(line);
				yield return nextCustomer;
			}
		}

		protected virtual Stream OpenStream()
		{
			if (!File.Exists(filePath))
			{
				throw new InvalidOperationException($"Specified file path {filePath} doesn't exist");
			}

			return File.OpenRead(filePath);
		}
	}
}
