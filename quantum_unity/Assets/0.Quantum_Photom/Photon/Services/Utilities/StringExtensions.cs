namespace Quantum.Services
{
	using System.Runtime.CompilerServices;

	public static partial class StringExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasValue(this string @this)
		{
			return string.IsNullOrEmpty(@this) == false;
		}
	}
}
