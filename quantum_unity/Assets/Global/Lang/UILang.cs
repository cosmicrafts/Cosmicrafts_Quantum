using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * This is a UI instance used to show a translated text
 * Requires the TEXT component and the key of the string
 */

[RequireComponent(typeof(TMP_Text))]
public class UILang : MonoBehaviour
{
    //The key of the string
    public string ID = "None";
    //Defines if the text needs to update when gets enable
    //bool okstart = false;

    //Loads and shows the string when begings
    void Start()
    {
        SetMyText();
        //okstart = true;
    }

    //Loads and shows the string
    public void SetMyText()
    {
        TMP_Text tmp = GetComponent<TMP_Text>();
        if(tmp){tmp.text = Lang.GetText(ID);}
        
        Text text = GetComponent<Text>();
        if(text){text.text = Lang.GetText(ID);}

        if (!text & !tmp)
        {
            Debug.Log("El objeto: " + gameObject.name+ " No tiene el campo de texto para asignar el valor");
        }
    }

    //Loads and shows the string if is required
    void OnEnable()
    {
        SetMyText();
    }
}
