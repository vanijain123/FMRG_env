using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Object_Json
{
    public string object_type;
    public string position;
}

//[System.Serializable]
//public class Objects_Json
//{
//    public Object_Json[] objects;
//}

[System.Serializable]
public class Time_Json
{
    public string timestamp_id;
    public Object_Json[] objects;
}

//[System.Serializable]
//public class Times_Json
//{
//    public Time_Json[] time;
//}

[System.Serializable]
public class Site_Json
{
    public string site_name;
    public Time_Json[] time;
}

[System.Serializable]
public class Sites_Json
{
    public Site_Json[] sites;
}

public class JsonReader : MonoBehaviour
{
    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        //Sites_Json sitesInJson = JsonUtility.FromJson<Sites_Json>(jsonFile.text);

        //foreach (Site_Json s in sitesInJson.sites)
        //{
        //    string site_name = s.site_name;
        //    foreach (Time_Json t in s.time)
        //    {
        //        string ts = t.timestamp_id;
        //        Debug.Log("Site: " + site_name + " Timestamp: " + ts);
        //    }
        //}
    }

    public int getNumberOfTimestamps(string siteName)
    {
        int num = 0;

        Sites_Json sitesInJson = JsonUtility.FromJson<Sites_Json>(jsonFile.text);

        foreach (Site_Json s in sitesInJson.sites)
        {
            string site_name = s.site_name;
            Debug.Log("site_name: " + site_name + " siteName: " + siteName);
            if (site_name == siteName)
            {
                foreach (Time_Json t in s.time)
                {
                    Debug.Log(num);
                    num++;
                }
                break;
            }
        }

        return num;
    }
}
