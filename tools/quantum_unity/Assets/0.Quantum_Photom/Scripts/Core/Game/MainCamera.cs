namespace TowerRush
{
	using UnityEngine;
	using System.Collections.Generic;
	using UnityEngine.Rendering.Universal;

	public class MainCamera : MonoBehaviour
	{
		// PUBLIC MEMBERS

		public Camera Camera;

		// PRIVATE MEMBERS

		private static MainCamera m_Instance;

		private List<OverlayCamera>           m_OverlayCameras = new List<OverlayCamera>(2);
		private UniversalAdditionalCameraData m_CustomData;

		// PUBLIC METHODS

		public void AddOverlayCamera(Camera camera, int order)
		{
			var overlay =  new OverlayCamera() { Camera = camera, Order = order };
			m_OverlayCameras.Add(overlay);

			m_OverlayCameras.Sort((lhs, rhs) =>
			{
				return lhs.Order.CompareTo(rhs.Order);
			});

			m_CustomData.cameraStack.Clear();

			for (int idx = 0, count = m_OverlayCameras.Count; idx < count; idx++)
			{
				m_CustomData.cameraStack.Add(m_OverlayCameras[idx].Camera);
			}
		}

		public void RemoveOverlayCamera(Camera camera)
		{
			m_OverlayCameras.RemoveAll(obj => obj.Camera == camera);
			m_CustomData.cameraStack.Remove(camera);
		}

		// MonoBehavior INTERFACE

		private void Awake()
		{
			if (m_Instance != null)
			{
				DestroyImmediate(gameObject);
				return;
			}

			m_Instance   = this;
			m_CustomData = Camera.GetUniversalAdditionalCameraData();
		}

		private void OnDestroy()
		{
			if (m_Instance != this)
				return;

			m_Instance = null;
		}

		// HELPERS

		private class OverlayCamera
		{
			public Camera Camera;
			public int    Order;
		}
	}
}