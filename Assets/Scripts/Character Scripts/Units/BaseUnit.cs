﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseUnit : MonoBehaviour
{
    public enum Team
    {
        allies,
        enemies,
        //neutral //Questionable

    }

    /*
        Unit Specifications:
        Move Range
        Attack Range
        Active


        -------Stats-------
        Level - LV
        Health - HP
        Strength - STR 
        Endurance - END
        Intelligence - INT
        Resolve - RES
        Agility - AGI
        Luck - LUK

    */


    public int LV, HP, MHP, STR, END, INT, RES, AGI, LUK;

    public int moveRange;
    public int attackRange;
    public int minARange;
    public int maxARange;



    public bool active;
    public bool selected;
    public Vector3 cancelPos;
    public Material mat;
    public BaseWeapon equipped;
    public List<BaseItem> inventory;
    public List<BaseUnit> targetList;
    public List<BaseWeapon.weaponType> equippable = new List<BaseWeapon.weaponType>();
    public Team team;
    //doot

    public PlayerPathing unitPathing;

    void Start()
    {

        HP = MHP;
        targetList = new List<BaseUnit>();
        inventory = new List<BaseItem>();
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] != null && inventory[i].itemtype == "Weapons" && equippable.Contains(((BaseWeapon)inventory[i]).type))
            {
                equipped = (BaseWeapon)inventory[i];
                found = true;
            }
        }
        if (!found)
            equipped = null;

        AddItem("Weapons", 1);
        AddItem("Weapons", 0);


        if (equipped == null)
            Debug.Log("No Item Equipped");

        active = true;
        unitPathing = GetComponent<PlayerPathing>();
        mat = GetComponent<Renderer>().material;
        if (team == Team.allies)
            mat.SetFloat("_enemy", 0);
        else if (team == Team.enemies)
            mat.SetFloat("_enemy", 1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaterial();
        UpdateWeapon();

    }


    void UpdateMaterial()
    {
        float inactive = active ? 0 : 1;
        float highlight = selected ? 1 : 0;
        mat.SetFloat("_highlight", highlight);
        mat.SetFloat("_inactive", inactive);




    }


    void UpdateWeapon()
    {
        if (equipped != null)
        {
            minARange = equipped.minRange;
            maxARange = equipped.maxRange;

        }
        else
            minARange = maxARange = 0;
    }

    void AddItem(string type, int id)
    {
        switch (type)
        {
            case "Weapons":
                BaseWeapon add = new BaseWeapon(id);
                if (add.myData != null && inventory.Count < 5)
                    inventory.Add(add);
                   
                break;

            default:
                break;

        }

    }
}
