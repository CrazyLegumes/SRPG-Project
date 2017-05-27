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
    public bool selectingItem;
    public GameObject textOptionPrefab;
    public GameObject OptionBackgroundPrefab;

    public GameObject optionback;
    public List<GameObject> optiontext;
    public BaseItem selectedItem;


    string[] options = { "Attack", "Items", "Stay", "Cancel" };

    [SerializeField]
    Image[] inventory;

    int optionID;
    public string selected;

    // Use this for initialization
    void Start()
    {


        myunit = GetComponent<BaseUnit>();
        HideAll();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventory();
        cursor.transform.SetAsLastSibling();

    }

    public void ShowOptions()
    {
        if (optionsMenu != null && !optionsMenu.activeSelf)
        {
            ResetCursorOptions();
            cursor.transform.localRotation = Quaternion.Euler(0, 0, 0);
            cursor.gameObject.SetActive(true);
            optionsMenu.SetActive(true);
            inventoryMenu.SetActive(false);

        }
    }

    public void ShowInventory()
    {
        ResetCursorInventory();
        if (inventoryMenu != null && !inventoryMenu.activeSelf)
        {
            inventoryMenu.SetActive(true);
            cursor.transform.localRotation = Quaternion.Euler(0, 0, 90);
            optionsMenu.SetActive(false);
        }
    }

    public void HideAll()
    {
        if (inventoryMenu != null)
            inventoryMenu.SetActive(false);
        if (optionsMenu != null)
            optionsMenu.SetActive(false);
        if (cursor.gameObject != null)
            cursor.gameObject.SetActive(false);
    }


    public void ShowItemOptions()
    {
        ResetCursorInfo();
        selectingItem = true;
        optionback = Instantiate(OptionBackgroundPrefab, Vector3.zero, Quaternion.Euler(90, 0, 0), transform.GetComponentInChildren<Canvas>().transform);
        optionback.GetComponent<RectTransform>().localScale = new Vector3(optionback.GetComponent<RectTransform>().localScale.x,
            optionback.GetComponent<RectTransform>().localScale.y * selectedItem.actions.Count, 1);

        optionback.GetComponent<RectTransform>().anchoredPosition = new Vector3(3.95f, (-1 - (selectedItem.actions.Count - 1) * .3f) - optionID * .5f);
        for (int i = 0; i < selectedItem.actions.Count; i++)
        {
            GameObject texter = Instantiate(textOptionPrefab, Vector3.zero, Quaternion.Euler(90, 0, 0), transform.GetComponentInChildren<Canvas>().transform);
            optiontext.Add(texter);
            texter.GetComponent<Text>().text = selectedItem.actions[i].ToString();
            RectTransform fix = texter.GetComponent<RectTransform>();
            fix.anchoredPosition = new Vector2(3.95f, .3f + optionback.GetComponent<RectTransform>().anchoredPosition.y - i * .5f);
        }
    }

    public void MoveCursorOptions(Vector3 dir)
    {
        dir.y = dir.z = 0;
        optionID += (int)dir.x;
        if (optionID < 0)
            optionID = options.Length - 1;
        if (optionID >= options.Length)
            optionID = 0;

        cursor.rectTransform.anchoredPosition = new Vector2(optionID - 1.5f, cursor.rectTransform.anchoredPosition.y);
        selected = options[optionID];

    }


    public void DestroyItemInfo()
    {
        Destroy(optionback);
        foreach (GameObject e in optiontext.ToArray())
            Destroy(e);
        selectingItem = false;
    }

    public void MoveCursorInventory(Vector3 dir)
    {
        dir.x = dir.y = 0;
        if (myunit.inventory.Count > 0)
        {
            optionID += (int)dir.z;
            if (optionID < 0)
                optionID = myunit.inventory.Count - 1;
            if (optionID >= myunit.inventory.Count)
                optionID = 0;
            cursor.rectTransform.anchoredPosition = new Vector2(cursor.rectTransform.anchoredPosition.x, -1 - optionID * .5f);
            selectedItem = myunit.inventory[optionID];
        }
    }


    public void MoveCursorInfo(Vector3 dir)
    {
        dir.x = dir.y = 0;
        if (selectedItem.actions.Count > 0)
        {
            optionID += (int)dir.z;
            if (optionID < 0)
                optionID = selectedItem.actions.Count - 1;
            if (optionID >= selectedItem.actions.Count)
                optionID = 0;

            cursor.rectTransform.anchoredPosition = new Vector3(cursor.rectTransform.anchoredPosition.x, -1 - optionID * .5f);
            selected = selectedItem.actions[optionID].ToString();
        }
    }


    void UpdateInventory()
    {
        for (int i = 0; i < 5; i++)
        {
            if ( i < myunit.inventory.Count && myunit.inventory[i] != null)
            {
                inventory[i].sprite = myunit.inventory[i].sprite;
                inventory[i].GetComponentInChildren<Text>().text = myunit.inventory[i].itemname;
                inventory[i].gameObject.SetActive(true);
            }
            else
            {
                inventory[i].sprite = null;
                inventory[i].GetComponentInChildren<Text>().text = "";
                inventory[i].gameObject.SetActive(false);
            }

        }


    }

    public void ExecuteAction()
    {
        switch (selected)
        {
            case "equip":
                if (myunit.equippable.Contains(((BaseWeapon)selectedItem).type))
                {
                    myunit.equipped = (BaseWeapon)selectedItem;
                    selectingItem = false;
                    ResetCursorInventory();
                    DestroyItemInfo();
                }
                break;
            case "discard":
                
                myunit.inventory.Remove(selectedItem);
                selectedItem = null;
                
                selectingItem = false;

                ResetCursorInventory();
                DestroyItemInfo();


                break;
            default:
                break;
        }
    }

    public void ResetCursorOptions()
    {
        optionID = 0;
        selected = options[optionID];
        cursor.rectTransform.anchoredPosition = new Vector2(optionID - 1.5f, .7f);

    }


    public void ResetCursorInventory()
    {
        optionID = 0;
        if (myunit.inventory.Count > 0)
            selectedItem = myunit.inventory[optionID];
        cursor.rectTransform.anchoredPosition = new Vector2(.7f, -1 - optionID * .5f);

    }

    public void ResetCursorInfo()
    {
        optionID = 0;
        if (selectedItem.actions.Count > 0)
            selected = selectedItem.actions[optionID].ToString();
        cursor.rectTransform.anchoredPosition = new Vector3(3.2f, -1 - optionID * .5f);
    }
}
