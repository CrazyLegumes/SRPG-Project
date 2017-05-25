using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonReader : MonoBehaviour {

    private string json;
    private string itempath;
    private string otherpath;
    private static JsonData itemData;



	// Use this for initialization
	void Awake () {
        itempath = File.ReadAllText(Application.dataPath + "/Resources/LibData/Items.json");
        itemData = JsonMapper.ToObject(itempath);
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static JsonData RetrieveItemInfo(string type, int id)
    {
        
        for(int i = 0; i < itemData[type].Count; i++)
        {
            if (itemData[type][i]["id"].ToString() == id.ToString())
                return itemData[type][i];

        }


        return null;
    }
}
