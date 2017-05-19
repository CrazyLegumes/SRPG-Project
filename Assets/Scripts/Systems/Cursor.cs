using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    [SerializeField]
    Transform cursor;
    public bool canMove;
    public Vector3 cursorpos;

    [SerializeField]
    public BaseUnit highlighted;

    // Use this for initialization
    void Start()
    {
        cursor = transform;
        InputController.moveEvent += InputController_moveEvent;

    }

    private void InputController_moveEvent(object sender, InputEvents<Vector3> e)
    {
        if (canMove)
        {
            if (GameMachine.instance.mapobj != null)
            {
                if (cursor != null)
                {
                    Vector3 cursorpos = cursor.transform.position;

                    cursorpos += e.info;

                    cursorpos.x = Mathf.Clamp(cursorpos.x, 0, GameMachine.instance.mapobj.mapWidth - 1);
                    cursorpos.z = Mathf.Clamp(cursorpos.z, 0, GameMachine.instance.mapobj.mapLength - 1);
                    cursor.position = new Vector3(cursorpos.x, .01f, cursorpos.z);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.name);
        if (col.GetComponent<BaseUnit>() != null)
        {
            highlighted = col.GetComponent<BaseUnit>();
            if (highlighted.active)
                highlighted.selected = true;

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (GameMachine.instance.selectedUnit != highlighted )
        {
            highlighted.selected = false;
            highlighted = null;
        }
    }

    void CursorInput()
    {
        if (GameMachine.instance.mapobj != null)
        {
            if (cursor != null)
            {
                Vector3 cursorpos = cursor.transform.position;

                if (Input.GetKeyDown(KeyCode.UpArrow))
                    cursorpos.z += 1;

                if (Input.GetKeyDown(KeyCode.DownArrow))
                    cursorpos.z -= 1;

                if (Input.GetKeyDown(KeyCode.RightArrow))
                    cursorpos.x += 1;

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    cursorpos.x -= 1;

                cursorpos.x = Mathf.Clamp(cursorpos.x, 0, GameMachine.instance.mapobj.mapWidth - 1);
                cursorpos.z = Mathf.Clamp(cursorpos.z, 0, GameMachine.instance.mapobj.mapLength - 1);
                cursor.position = new Vector3(cursorpos.x, .01f, cursorpos.z);




            }
        }
    }
}
