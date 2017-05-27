using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem  {
    public enum Options
    {
        equip,
        use,
        discard
    }

    public int id;
    public string itemname;
    public string itemtype;
    public Sprite sprite;
    public List<Options> actions;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
