using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour {


    public Canvas unitCanvas;
    public GameObject optionsMenu;
    public GameObject inventoryMenu;
    public BaseUnit myunit;
    public Image cursor;
    bool attackDisabled;

    string[] options = { "Attack", "Items", "Stay", "Cancel" };
    int optionID = 0;
    public string selected;

	// Use this for initialization
	void Start () {
        myunit = GetComponent<BaseUnit>();
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ShowOptions()
    {
        if (!optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(true);
            inventoryMenu.SetActive(false);

        }
    }

    public void ShowInventory()
    {
        if (!inventoryMenu.activeSelf)
        {
            inventoryMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
    }

    public void HideAll()
    {
        inventoryMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
}
