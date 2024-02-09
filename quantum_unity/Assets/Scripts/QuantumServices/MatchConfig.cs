namespace Quantum.Services
{
    public sealed partial class MatchConfig
    {
		public RuntimeConfig RuntimeConfig;

		partial void Serialize(ref object data)
		{
			data = RuntimeConfig.ToByteArray(RuntimeConfig);
		}

		partial void Deserialize(ref object data)
		{
			if (data == null)
			{
				RuntimeConfig = default;
			}
			else
			{
				RuntimeConfig = RuntimeConfig.FromByteArray((byte[])data);
			}
		}
	}
}
