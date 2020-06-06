using System.Collections.Generic;
using System.Threading.Tasks;
using Intercom.MyCustomers.Model;

namespace Intercom.MyCustomers.Providers
{
	public interface ICustomerDestination
	{
		Task WriteCustomers(IEnumerable<Customer> customers);
	}
}
