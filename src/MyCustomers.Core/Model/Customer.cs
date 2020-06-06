using Newtonsoft.Json;

namespace Intercom.MyCustomers.Model
{
	public class Customer
	{
		public Customer()
		{
		}

		public Customer(long userId, string name, double latitude, double longitude)
		{
			Latitude = latitude;
			UserId = userId;
			Name = name;
			Longitude = longitude;
		}


		[JsonProperty("latitude")]
		public double Latitude { get; private set; }

		[JsonProperty("user_id")]
		public long UserId { get; private set; }

		[JsonProperty("name")]
		public string Name { get; private set; }

		[JsonProperty("longitude")]
		public double Longitude { get; private set; }
	}
}
