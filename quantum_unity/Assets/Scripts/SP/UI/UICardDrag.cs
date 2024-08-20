using UnityEngine;
using UnityEngine.UI;

public class UICardDrag : MonoBehaviour
{
    /* 
     * This class represents the current draging card
     * for the deck collection menu
     */

    //The icon of the current draging card
    public Image Icon;
    public Canvas canvas;
    Vector2 pos;

    // Update is called once per frame
    void Update()
    {
        //Always follow the mouse
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }
}
