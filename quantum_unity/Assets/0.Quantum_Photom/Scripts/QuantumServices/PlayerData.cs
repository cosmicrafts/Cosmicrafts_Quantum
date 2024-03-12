namespace Quantum.Services
{
    public sealed partial class PartyPlayerData
    {
		public int Level { get { return _level; } set { _level = value; Synchronize(); } }

		private int _level;

		partial void Initialize()
		{
		}

		partial void Deinitialize()
		{
		}

		partial void Serialize(ref object data)
		{
			data = new object[] { _level };
		}

		partial void Deserialize(ref object data)
		{
			if (data == null)
			{
				_level = default;
			}
			else
			{
				_level = (int)((object[])data)[0];
			}
		}
	}

    public sealed partial class LobbyPlayerData
    {
		public string Scene { get { return _scene; } set { _scene = value; Synchronize(); } }

		private string _scene;

		partial void Initialize()
		{
		}

		partial void Deinitialize()
		{
		}

		partial void Serialize(ref object data)
		{
			data = new object[] { _scene };
		}

		partial void Deserialize(ref object data)
		{
			if (data == null)
			{
				_scene = default;
			}
			else
			{
				_scene = (string)((object[])data)[0];
			}
		}
    }
}
