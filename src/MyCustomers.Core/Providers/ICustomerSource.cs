using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intercom.MyCustomers.Model;

namespace Intercom.MyCustomers.Providers
{
	public interface ICustomerSource
	{
		Task<IReadOnlyList<Customer>> FindCustomers<TKey>(Func<Customer, bool> customerPredicate,
			Func<Customer, TKey> orderBy);
	}
}
