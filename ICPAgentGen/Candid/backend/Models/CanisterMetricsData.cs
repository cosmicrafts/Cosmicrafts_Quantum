using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using System.Collections.Generic;
using System;

namespace Cosmicrafts.backend.Models
{
	[Variant]
	public class CanisterMetricsData
	{
		[VariantTagProperty]
		public CanisterMetricsDataTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public CanisterMetricsData(CanisterMetricsDataTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterMetricsData()
		{
		}

		public static CanisterMetricsData Daily(List<DailyMetricsData> info)
		{
			return new CanisterMetricsData(CanisterMetricsDataTag.Daily, info);
		}

		public static CanisterMetricsData Hourly(List<HourlyMetricsData> info)
		{
			return new CanisterMetricsData(CanisterMetricsDataTag.Hourly, info);
		}

		public List<DailyMetricsData> AsDaily()
		{
			this.ValidateTag(CanisterMetricsDataTag.Daily);
			return (List<DailyMetricsData>)this.Value!;
		}

		public List<HourlyMetricsData> AsHourly()
		{
			this.ValidateTag(CanisterMetricsDataTag.Hourly);
			return (List<HourlyMetricsData>)this.Value!;
		}

		private void ValidateTag(CanisterMetricsDataTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterMetricsDataTag
	{
		[CandidName("daily")]
		Daily,
		[CandidName("hourly")]
		Hourly
	}
}