using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;



public class TokenAnimation : MonoBehaviour
{
    public Button triggerButton;
    public RectTransform endPoint;
    public GameObject tokenPrefab;
    public Image sourceImage;

    public float duration = 1.0f;
    private Canvas canvas;
    private List<GameObject> tokens = new List<GameObject>();

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (triggerButton != null && endPoint != null && tokenPrefab != null && canvas != null)
        {
            triggerButton.onClick.AddListener(() => StartAnimation(triggerButton.GetComponent<RectTransform>(), endPoint, duration));
        }
        else
        {
            Debug.LogError("Some components are not assigned or Canvas is not found.");
        }
    }

    public void StartAnimation(RectTransform start, RectTransform end, float duration)
    {
        StartCoroutine(AnimateTokens(start, end, duration, 8));
    }

    IEnumerator AnimateTokens(RectTransform start, RectTransform end, float duration, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject token = Instantiate(tokenPrefab, canvas.transform, false);
            tokens.Add(token);
            RectTransform tokenRect = token.GetComponent<RectTransform>();

            // Find the child object by name and get the Image component from it.
            // Replace "ChildName" with the actual name of your child object.
            Transform childTransform = token.transform.Find("Image");
            if (childTransform != null)
            {
                Image tokenChildImage = childTransform.GetComponent<Image>();
                if (sourceImage != null && tokenChildImage != null)
                {
                    tokenChildImage.sprite = sourceImage.sprite;
                }
            }

            // Convert start and end points to canvas space
            Vector2 startScreenPoint = RectTransformUtility.WorldToScreenPoint(null, start.position);
            Vector2 endScreenPoint = RectTransformUtility.WorldToScreenPoint(null, end.position);

            Vector2 startCanvasPoint, endCanvasPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, startScreenPoint, null, out startCanvasPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, endScreenPoint, null, out endCanvasPoint);

            // Set starting position
            tokenRect.anchoredPosition = startCanvasPoint;

            LeanTween.move(tokenRect, endCanvasPoint, duration).setEase(LeanTweenType.easeOutQuad);
            LeanTween.scale(token, Vector3.one * 0.25f, duration / 2).setLoopPingPong(1);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
