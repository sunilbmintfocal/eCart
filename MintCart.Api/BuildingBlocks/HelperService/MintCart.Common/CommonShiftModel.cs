

using System;

namespace MintCart.Common
{
	public class CommonShiftModel
	{
		public string? Id { get; set; }
		public string Name { get; set; }
		public TimeSpan FromTime { get; set; }
		public TimeSpan ToTime { get; set; }
	}
}
