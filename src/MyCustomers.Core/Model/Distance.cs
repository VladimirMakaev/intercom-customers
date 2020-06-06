using System;
using CSharpFunctionalExtensions;

namespace Intercom.MyCustomers.Model
{
	public class Distance : ValueObject<Distance>
	{
		private const double ToleranceToCompare = 0.000000000000000001;
		private const double EarthRadius = 6371008.7714; //  https://en.wikipedia.org/wiki/Earth_radius
		private readonly double internalValueInMeters;

		private Distance(double internalValueInMeters)
		{
			this.internalValueInMeters = internalValueInMeters;
		}

		public static Distance From(double value, UnitType unit) =>
			unit switch
			{
				UnitType.M => new Distance(value),
				UnitType.Km => new Distance(1000 * value),
				_ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
			};

		public static Distance Between(Location loc1, Location loc2)
		{
			var phi1 = Utility.ToRadians(loc1.Latitude);
			var phi2 = Utility.ToRadians(loc2.Latitude);
			var deltaLambda = Utility.ToRadians(loc1.Longitude - loc2.Longitude);
			var deltaSigma = Math.Acos(Math.Sin(phi1) * Math.Sin(phi2) +
			                           Math.Cos(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda));

			return Distance.From(deltaSigma * EarthRadius, UnitType.M);
		}

		public static bool operator <(Distance a, Distance b) => a.internalValueInMeters < b.internalValueInMeters;

		public static bool operator >(Distance a, Distance b) => b < a;

		public override string ToString()
		{
			if (internalValueInMeters < 1000)
			{
				return $"{internalValueInMeters:F4} m";
			}

			return $"{(internalValueInMeters / 1000):F4} km";
		}

		protected override bool EqualsCore(Distance other) =>
			Math.Abs(other.internalValueInMeters - internalValueInMeters) < ToleranceToCompare;


		protected override int GetHashCodeCore()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ internalValueInMeters.GetHashCode();
			}
		}
	}
}
