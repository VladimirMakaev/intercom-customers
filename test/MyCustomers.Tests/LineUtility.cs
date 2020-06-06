using System;

namespace Intercom.MyCustomers.Tests
{
	public class LineUtility
	{
		/// <summary>
		/// Combines lines with <see cref="Environment.NewLine"/> to make sure tests are correctly working on Windows & Linux
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static string CombineWithLineBreak(params string[] lines) =>
			string.Join(Environment.NewLine, lines) + Environment.NewLine;
	}
}
