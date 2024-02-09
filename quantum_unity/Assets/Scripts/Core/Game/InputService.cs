namespace TowerRush
{
	using Photon.Deterministic;
	using TowerRush.Core;
	using UnityEngine;

	public class InputService : MonoBehaviour
	{
		public void Initialize()
		{
			QuantumCallback.Subscribe(this, (Quantum.CallbackPollInput callback) => PollInput(callback));
		}

		public void Deinitialize()
		{
			QuantumCallback.UnsubscribeListener<Quantum.CallbackPollInput>(this);
		}

		public void Update_Internal(SceneContext context)
		{
		}

		public void PollInput(Quantum.CallbackPollInput callback)
		{
	#if UNITY_EDITOR
			CheckPlayerSwap();

			if (Entities.LocalPlayer != callback.Player)
				return;
	#endif

			var input  = new Quantum.Input();

			callback.SetInput(input, DeterministicInputFlags.Repeatable);
		}

		private void CheckPlayerSwap()
		{
			if (Input.GetKeyDown(KeyCode.Keypad1) == true)
			{
				Entities.Instance.SetLocalPlayer(0);
			}
			else if (Input.GetKeyDown(KeyCode.Keypad2) == true)
			{
				Entities.Instance.SetLocalPlayer(1);
			}
			else if (Input.GetKeyDown(KeyCode.Keypad3) == true)
			{
				Entities.Instance.SetLocalPlayer(2);
			}
			else if (Input.GetKeyDown(KeyCode.Keypad4) == true)
			{
				Entities.Instance.SetLocalPlayer(3);
			}
		}
	}
}