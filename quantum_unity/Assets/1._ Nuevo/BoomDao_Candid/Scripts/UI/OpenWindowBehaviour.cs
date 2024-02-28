using Boom.UI;
using UnityEngine;

public class OpenWindowBehaviour : MonoBehaviour
{
    [SerializeField] string windowName;
    [SerializeField] int sortingOrder = 0;
    [SerializeField] bool onpenOnStart;
    [SerializeField, ShowOnly] Window window;

    private void Start()
    {
        if (onpenOnStart)
        {
            Open();
        }
    }
    private void OnDestroy()
    {
        if (onpenOnStart)
        {
            Close();
        }
    }

    public void Open()
    {
        if (window) return;
        window = WindowManager.Instance.OpenWindow(windowName, null, sortingOrder);
    }
    public void Close()
    {
        if (window == null) return;
        window.Close();
    }
}
