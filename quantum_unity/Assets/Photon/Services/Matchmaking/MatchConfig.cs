namespace Quantum.Services
{
	public sealed partial class MatchConfig
	{
		//========== PUBLIC METHODS ===================================================================================

		public object GetData()
		{
			object data = null;
			Serialize(ref data);
			return data;
		}

		public void SetData(object data)
		{
			Deserialize(ref data);
		}

		//========== PARTIAL METHODS ==================================================================================

		partial void Serialize(ref object data);
		partial void Deserialize(ref object data);
	}
}
