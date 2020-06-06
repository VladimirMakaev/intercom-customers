using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intercom.MyCustomers.Providers.Json;
using Xunit;

namespace Intercom.MyCustomers.Tests.Unit
{
	public class JsonFileCustomerProviderTests
	{
		[Fact]
		public async Task Customer_information_can_be_correctly_deserialized()
		{
			var stubJson = LineUtility.CombineWithLineBreak(
				"{\"latitude\": \"54.0894797\", \"user_id\": 8, \"name\": \"Eoin Ahearn\", \"longitude\": \"-6.18671\"}",
				"{\"latitude\": \"52.366037\", \"user_id\": 16, \"name\": \"Ian Larkin\", \"longitude\": \"-8.179118\"}");

			var subject = new TestSubject(stubJson);

			var customers = await subject.FindCustomers(c => c.Name.StartsWith("Ian"), c => c.UserId);
			Assert.Equal(new[] {"Ian Larkin"}, customers.Select(x => x.Name));
		}

		[Fact]
		public async Task Customer_information_can_be_searched_deserialized()
		{
			var stubJson = LineUtility.CombineWithLineBreak(
				"{\"latitude\": \"52.366037\", \"user_id\": 16, \"name\": \"Ian Larkin\", \"longitude\": \"-8.179118\"}",
				"{\"latitude\": \"54.0894797\", \"user_id\": 8, \"name\": \"Eoin Ahearn\", \"longitude\": \"-6.18671\"}");

			var subject = new TestSubject(stubJson);

			var customers = await subject.FindCustomers(c => c.Name.StartsWith("Ian"), c => c.UserId);
			Assert.Equal(new[] {"Ian Larkin"}, customers.Select(x => x.Name));
		}


		/*
		 * We use this approach as a way to stub data in JsonFileCustomerSource
		 * Ideally the lifetime of the stream should be controlled withing the JsonFileCustomerSource
		 * This way we can both unit test it and encapsulate the stream from being injected publicly
		 */
		private class TestSubject : JsonFileCustomerSource
		{
			private readonly string stubJson;

			public TestSubject(string stubJson) : base("testFile.json")
			{
				this.stubJson = stubJson;
			}

			protected override Stream OpenStream() => new MemoryStream(Encoding.UTF8.GetBytes(stubJson));
		}
	}
}
