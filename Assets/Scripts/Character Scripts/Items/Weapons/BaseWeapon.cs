﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[System.Serializable]
public class BaseWeapon : BaseItem
{
    public enum weaponType
    {
        sword,
        axe,
        spear,
        bow
    }


    /* item stats:
        range
        attack power
        type
        description
        name

    */


   
    public int minRange;
    
    public int maxRange;

    public int baseDamage;
    
    
    [SerializeField]
    public weaponType type;

    public JsonData myData;
  



    // Use this for initialization
    public BaseWeapon(int idnum)
    {

        id = idnum;

        myData = JsonReader.RetrieveItemInfo("Weapons", id);
        if (myData != null)
        {
            itemtype = "Weapons";
            Debug.Log("GOt it coach");
            itemname = myData["name"].ToString();
            minRange = int.Parse(myData["minRange"].ToString());
            maxRange = int.Parse(myData["maxRange"].ToString());
            baseDamage = int.Parse(myData["damage"].ToString());
            type = (weaponType)int.Parse(myData["type"].ToString());
            switch (type)
            {
                case weaponType.axe:
                    sprite = Resources.Load<Sprite>("Art/UIStuff/AxeIcon");
                    break;
                case weaponType.sword:
                    sprite = Resources.Load<Sprite>("Art/UIStuff/SwordIcon");
                    break;
                case weaponType.spear:
                    sprite = Resources.Load<Sprite>("Art/UISuff/PolearmIcon");
                    break;
                default:
                    break;
            }
        }


    }

    // Update is called once per frame

}
