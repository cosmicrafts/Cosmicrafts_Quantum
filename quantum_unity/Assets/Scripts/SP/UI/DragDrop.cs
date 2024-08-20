using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour,
                      IPointerDownHandler,
                      IBeginDragHandler,
                      IEndDragHandler,
                      IDragHandler,
                      IDropHandler
{
    

    //To move graphics
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [SerializeField] private Canvas canvas;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //Events
    public void OnBeginDrag(PointerEventData eventData)
    {
      Debug.Log("OnBeginDrag");
      canvasGroup.blocksRaycasts = false;
      canvasGroup.alpha = .6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
      Debug.Log("OnDrag");
      //movement delta is the amount the mouse moves / select canvas to scalablesize
      rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      Debug.Log("OnEndDrag");
      canvasGroup.blocksRaycasts = true;
      canvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
      Debug.Log("OnDrop");
    }
        public void OnPointerDown(PointerEventData eventData)
    {
      Debug.Log("OnPointerDown");
    }
}