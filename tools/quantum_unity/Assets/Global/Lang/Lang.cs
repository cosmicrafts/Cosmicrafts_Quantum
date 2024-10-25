using System.Collections;
using System.Xml;
using UnityEngine;
/*
 * This is the game language controller
 * Loads the strings from the Xmls and returns the vaules for the current game language
*/
//All the available languages
public enum Language
{
    English,
    Spanish,
    Arabian,
    Chinese,
    German,
    French,
    Hindu,
    Japanese,
    Portuguese,
    Russian,
    Vietnamese,
    Korean
}

public class Lang : MonoBehaviour
{
    //The names of each language file
    public static string[] FilesNames = new string[12]
    {
        "ENG","ESP","ARA","CHA","DEU","FRA","HIN","JPN","POR","RUS","VIE","KOR"
    };
    //Save the current language data
    public static Hashtable UIS;
    //Initialize the game language
    public static void InitLanguage(Language language)
    {
        if (UIS != null)
            return;

        SetLang(language);
    }
    //Set the game language
    public static void SetLang(Language newlang)
    {
        //Load the xml data from resources
        TextAsset textAsset = (TextAsset)Resources.Load($"XML/Langs/LNG_{FilesNames[(int)newlang]}");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);
        UIS = new Hashtable();
        //Load Strings
        //Save the data in the memory
        XmlNodeList elements = xml.SelectNodes($"/Strings/string");
        if (elements != null)
        {
            IEnumerator elemEnum = elements.GetEnumerator();
            while (elemEnum.MoveNext())
            {
                XmlElement xmlItem = (XmlElement)elemEnum.Current;
                UIS.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
            }

            foreach (UILang lg in FindObjectsOfType<UILang>())
            {
                lg.SetMyText();
            }
        }
        else
        {
            Debug.LogError($"The specified language does not exist: {newlang}");
        }
    }
    //Returns a specific text associated to the key
    public static string GetText(string key)
    {
        if (UIS == null)
            return string.Empty;

        if (!UIS.ContainsKey(key))
            return "[NO FOUND]";

        return UIS[key].ToString();
    }
    //Returns the name of a specific entitie key (unit, spell or character)
    public static string GetEntityName(string key)
    {
        if (UIS == null)
            return string.Empty;

        if (!UIS.ContainsKey(key))
            return "[NO FOUND]";

        string data = UIS[key].ToString();

        return data.Substring(0, data.IndexOf(':'));
    }
    //Returns the description of a specific entitie key (unit, spell or character)
    public static string GetEntityDescription(string key)
    {
        if (UIS == null)
            return string.Empty;

        if (!UIS.ContainsKey(key))
            return "[NO FOUND]";

        string data = UIS[key].ToString();

        return data.Substring(data.IndexOf(':')+1);
    }
}
