namespace TowerRush
{
	using Quantum;

	public class GameplayInfo
	{
		public QuantumRunner.StartParameters StartParams;
		public string                        ClientID;
		public string                        SceneName;
		public byte                          Level;
		public CardInfo[]                    Cards;
	}
}