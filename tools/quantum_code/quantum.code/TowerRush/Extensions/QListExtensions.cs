using Photon.Deterministic;
using Quantum.Collections;

public static class QListExtensions
{
	public unsafe static void Shuffle<T>(this QList<T> @this, RNGSession* rng) where T : unmanaged
	{
		var n = @this.Count - 1;
	
		while (n > 0)
		{
			var k     = rng->Next(0, n);
			var value = @this[k];
	
			@this[k] = @this[n];
			@this[n] = value;
	
			--n;
		}
	}
}
