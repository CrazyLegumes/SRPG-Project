using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{


    public Canvas unitCanvas;
    public GameObject optionsMenu;
    public GameObject inventoryMenu;
    public BaseUnit myunit;
    public Image cursor;
    bool attackDisabled;


    string[] options = { "Attack", "Items", "Stay", "Cancel" };

    [SerializeField]
    Image[] inventory;

    int optionID;
    public string selected;

    // Use this for initialization
    void Start()
    {
        ResetCursor();

        myunit = GetComponent<BaseUnit>();
        HideAll();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventory();

    }

    public void ShowOptions()
    {
        if (optionsMenu != null && !optionsMenu.activeSelf)
        {
            ResetCursor();   
            optionsMenu.SetActive(true);
            inventoryMenu.SetActive(false);

        }
    }

    public void ShowInventory()
    {

        if (inventoryMenu != null && !inventoryMenu.activeSelf)
        {
            inventoryMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
    }

    public void HideAll()
    {
        if (inventoryMenu != null)
            inventoryMenu.SetActive(false);
        if (optionsMenu != null)
            optionsMenu.SetActive(false);
    }


    public void MoveCursorOptions(Vector3 dir)
    {
        dir.y = dir.z = 0;
        optionID += (int)dir.x;
        if (optionID < 0)
            optionID = options.Length - 1;
        if (optionID >= options.Length)
            optionID = 0;

        cursor.rectTransform.anchoredPosition = new Vector2(optionID - 2, cursor.rectTransform.anchoredPosition.y);
        selected = options[optionID];

    }

    void UpdateInventory()
    {
        for (int i = 0; i < myunit.inventory.Count; i++)
        {
            if (myunit.inventory[i] != null)
            {
                inventory[i].sprite = myunit.inventory[i].sprite;
                inventory[i].GetComponentInChildren<Text>().text = myunit.inventory[i].itemname;
                inventory[i].gameObject.SetActive(true);
            }
            
        }

        for(int i =0; i < inventory.Length; i++)
        {
            if (inventory[i].sprite == null)
                inventory[i].gameObject.SetActive(false);
        }
    }

    void ResetCursor()
    {
        optionID = 0;
        selected = options[optionID];
        cursor.rectTransform.anchoredPosition = new Vector2(optionID - 2, cursor.rectTransform.anchoredPosition.y);

    }
}
