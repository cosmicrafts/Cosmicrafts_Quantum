using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using System;

namespace Cosmicrafts.backend.Models
{
	[Variant]
	public class Category
	{
		[VariantTagProperty]
		public CategoryTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Category(CategoryTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Category()
		{
		}

		public static Category Avatar()
		{
			return new Category(CategoryTag.Avatar, null);
		}

		public static Category Chest()
		{
			return new Category(CategoryTag.Chest, null);
		}

		public static Category Trophy()
		{
			return new Category(CategoryTag.Trophy, null);
		}

		public static Category Unit(Unit info)
		{
			return new Category(CategoryTag.Unit, info);
		}

		public Unit AsUnit()
		{
			this.ValidateTag(CategoryTag.Unit);
			return (Unit)this.Value!;
		}

		private void ValidateTag(CategoryTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CategoryTag
	{
		Avatar,
		Chest,
		Trophy,
		Unit
	}
}