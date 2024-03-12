namespace TowerRush
{
	public class Signal
	{
		private System.Action m_Signal;

		public void Connect(System.Action action)
		{
			m_Signal += action;
		}

		public void Disconnect(System.Action action)
		{
			m_Signal -= action;
		}

		public void Emit()
		{
			if (m_Signal == null)
				return;

			m_Signal();
		}
	}

	public class Signal<T1>
	{
		private System.Action<T1> m_Signal;

		public void Connect(System.Action<T1> action)
		{
			m_Signal += action;
		}

		public void Disconnect(System.Action<T1> action)
		{
			m_Signal -= action;
		}

		public void Emit(T1 arg0)
		{
			if (m_Signal == null)
				return;

			m_Signal(arg0);
		}
	}

	public class Signal<T1, T2>
	{
		private System.Action<T1, T2> m_Signal;

		public void Connect(System.Action<T1, T2> action)
		{
			m_Signal += action;
		}

		public void Disconnect(System.Action<T1, T2> action)
		{
			m_Signal -= action;
		}

		public void Emit(T1 arg0, T2 arg1)
		{
			if (m_Signal == null)
				return;

			m_Signal(arg0, arg1);
		}
	}

	public class Signal<T1, T2, T3>
	{
		private System.Action<T1, T2, T3> m_Signal;

		public void Connect(System.Action<T1, T2, T3> action)
		{
			m_Signal += action;
		}

		public void Disconnect(System.Action<T1, T2, T3> action)
		{
			m_Signal -= action;
		}

		public void Emit(T1 arg0, T2 arg1, T3 arg2)
		{
			if (m_Signal == null)
				return;

			m_Signal(arg0, arg1, arg2);
		}
	}
}
