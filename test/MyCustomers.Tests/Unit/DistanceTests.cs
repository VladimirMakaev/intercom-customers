using Intercom.MyCustomers.Model;
using Xunit;

namespace Intercom.MyCustomers.Tests.Unit
{
	public class DistanceTests
	{
		private static readonly Location DublinSpire = new Location(53.349802, -6.260253);
		private static readonly Location GuinnessStorehouse = new Location(53.341854, -6.286290);
		private static readonly Location Moscow = new Location(55.753162, 37.621987);
		private static readonly Location NewYork = new Location(40.757974, -73.985554);


		[Fact]
		public void Distance_created_from_different_units_is_correctly_converted()
		{
			var value1 = Distance.From(1234, UnitType.Km);
			var value2 = Distance.From(1234000, UnitType.M);
			Assert.Equal(value1, value2);
		}

		[Fact]
		public void Distance_are_not_always_equal()
		{
			var value1 = Distance.From(2352, UnitType.Km);
			var value2 = Distance.From(2352001, UnitType.M);
			Assert.NotEqual(value1, value2);
		}

		[Fact]
		public void Distance_between_DublinSpire_and_GuinnessStorehouse_is_between_194and195()
		{
			var distance = Distance.Between(DublinSpire, GuinnessStorehouse);
			//1.94km
			Assert.True(distance > Distance.From(1.94, UnitType.Km) && distance < Distance.From(1.95, UnitType.Km));
		}

		[Fact]
		public void Distance_between_Moscow_and_NewYork_is_between_7505and7506()
		{
			var distance = Distance.Between(Moscow, NewYork);
			//1.94km
			Assert.True(distance > Distance.From(7500, UnitType.Km) && distance < Distance.From(7510, UnitType.Km));
		}
	}
}
