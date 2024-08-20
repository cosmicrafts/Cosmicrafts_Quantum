using System.Collections;
using System.Globalization;
using UnityEngine;

public class UIScroll : MonoBehaviour
{
    private float h = Screen.height;
    private float w = Screen.width; 
    private float Ref = 768f;
    public float timeAnimation = 0.5f;

    private float column = 0;
    private float row = 0;
    private int indexUI = 0;
    
    public GameObject[] sections;
    
    public void MoveTo(string item = "0,0,0")
    {
        string[] split = item.Split(",");
        column = float.Parse(split[0], CultureInfo.InvariantCulture);
        row = float.Parse(split[1], CultureInfo.InvariantCulture);
        indexUI = int.Parse(split[2]);
        Debug.Log("Column: "+column+"  Row: "+ row);
           
        h = Screen.height;
        w = Screen.width;
        StopAllCoroutines();
        StartCoroutine(AnimationCoRoutine(column,row, indexUI));
    }
       
    private IEnumerator AnimationCoRoutine(float column, float row, int indexUI)
    {
        sections[indexUI].SetActive(true);
        
        Vector3 start = transform.localPosition;
        float targetX = (w / h) * Ref * column;
        float targetY =  Ref * row;
        Vector3 targetPosition = new Vector3(targetX, targetY, transform.localPosition.z);
   
        // animate over 1/2 second
        for (float accumTime = Time.deltaTime; accumTime <= timeAnimation; accumTime += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(start, targetPosition, accumTime / timeAnimation);
            yield return null;
        }

        foreach (var section in sections)
        {
            section.SetActive(false);
        }
        sections[indexUI].SetActive(true);
           
        transform.localPosition = targetPosition;
    }
    
    // Resizing functions here
    private void ScreenResized() {
        Vector3 start = transform.localPosition;
        float targetX = (w / h) * Ref * column;
        float targetY =  Ref * row;
        Vector3 targetPosition = new Vector3(targetX, targetY, transform.localPosition.z);
        
        transform.localPosition = targetPosition;
    }

    // On every screen refresh
    void Update()
    {
        if ((Screen.width != w) || (Screen.height != h)) {
            w = Screen.width;
            h = Screen.height;
            ScreenResized();
        }
    }
    
    
    
    
}



