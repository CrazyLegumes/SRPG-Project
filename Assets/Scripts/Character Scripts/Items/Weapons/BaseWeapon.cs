using System.Collections;
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
        bow,
        tome
    }
    public enum damageType
    {
        Phyiscal,
        Magical
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

    [SerializeField]
    public damageType dtype;

    public JsonData myData;
  



    // Use this for initialization
    public BaseWeapon(int idnum)
    {
        actions = new List<Options>();
        actions.Add(Options.equip);
        actions.Add(Options.discard);
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
            switch (myData["damagetype"].ToString())
            {
                case "Phyiscal":
                    dtype = damageType.Phyiscal;
                    break;
                case "Magical":
                    dtype = damageType.Magical;
                    break;
                default:
                    break;
            }
        }


    }

    // Update is called once per frame

}
