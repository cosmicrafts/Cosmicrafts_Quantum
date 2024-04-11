using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelPositioner : MonoBehaviour, IPointerClickHandler
{
    public RectTransform panelRectTransform;  // Assign the RectTransform of your panel in the inspector.

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the panel is active, and if not, activate it.
        if (!panelRectTransform.gameObject.activeInHierarchy)
        {
            panelRectTransform.gameObject.SetActive(true);
        }

        // Set the position of the panel to the click position.
        // Convert the screen position of the mouse click to world point in the canvas plane.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        panelRectTransform.localPosition = localPoint;
    }
}
