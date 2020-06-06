using Intercom.MyCustomers.Model;

namespace Intercom.MyCustomers.Configuration
{
	public class FindCustomersConfiguration
	{
		public double Distance { get; set; } = 100;
		public UnitType Unit { get; set; } = UnitType.Km;
		public double OfficeLongitude { get; set; } = -6.257664;
		public double OfficeLatitude { get; set; } = 53.339428;
	}
}
