public static class StringExtensions
{
	public static bool IsNullOrEmpty(this string @this)
	{
		return string.IsNullOrEmpty(@this);
	}

	public static bool HasValue(this string @this)
	{
		return string.IsNullOrEmpty(@this) == false;
	}
}
