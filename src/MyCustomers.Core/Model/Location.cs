using System;
using CSharpFunctionalExtensions;

namespace Intercom.MyCustomers.Model
{
	public class Location : ValueObject<Location>
	{
		private readonly double latitude;
		private readonly double longitude;
		private const double ToleranceToCompare = 0.000000000000000001;

		public double Latitude => latitude;

		public double Longitude => longitude;

		public Location(double latitude, double longitude)
		{
			this.latitude = latitude;
			this.longitude = longitude;
		}

		protected override bool EqualsCore(Location other) =>
			Math.Abs(other.latitude - latitude) < ToleranceToCompare &&
			Math.Abs(other.longitude - longitude) < ToleranceToCompare;

		protected override int GetHashCodeCore()
		{
			var hashCode = base.GetHashCode();
			hashCode = (hashCode * 397) ^ latitude.GetHashCode();
			hashCode = (hashCode * 397) ^ longitude.GetHashCode();
			return hashCode;
		}

		public Distance DistanceTo(double lat, double lon) => Distance.Between(this, new Location(lat, lon));
	}
}
