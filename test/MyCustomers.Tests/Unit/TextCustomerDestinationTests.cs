using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Intercom.MyCustomers.Model;
using Intercom.MyCustomers.Providers.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Intercom.MyCustomers.Tests.Unit
{
	public class TextCustomerDestinationTests
	{
		private const int DefaultSize = 1000;

		[Fact]
		public async Task Customers_are_correctly_formatted_in_outputfile()
		{
			var subject = new TestSubject(DefaultSize);

			await subject.WriteCustomers(new[]
			{
				new Customer(1, "Vladimir Makayev", 10.5, 99.4), new Customer(2, "John Doe", 10, 9)
			});


			Assert.Equal(LineUtility.CombineWithLineBreak("1 Vladimir Makayev", "2 John Doe"), subject.GetContents());
		}

		/*
		 * We use this approach as a way to stub output file for PlainTextCustomerDestination
		 * Ideally the lifetime of the stream should be controlled withing the PlainTextCustomerDestination
		 * This way we can both unit test it and encapsulate the stream from being injected publicly
		 */
		private class TestSubject : PlainTextCustomerDestination
		{
			private readonly byte[] buffer;
			private readonly MemoryStream stream;


			public TestSubject(int size) : base("testoutput.txt", new NullLogger<PlainTextCustomerDestination>())
			{
				buffer = new byte[size];
				stream = new MemoryStream(buffer);
			}

			public string GetContents() => Encoding.UTF8.GetString(buffer, 0, Array.FindIndex(buffer, c => c == 0));

			protected override Stream OpenStream() => stream;
		}
	}
}
