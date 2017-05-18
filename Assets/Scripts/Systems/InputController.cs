using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InputController : MonoBehaviour {



    class Repeater
    {

        const float threshold = 0.5f;
        const float rate = .05f;
        float _next;
        bool hold;
        string _axis;

        public Repeater(string axisName)
        {
            _axis = axisName;
        }


        public int Update()
        {
            int retValue = 0;
            int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));
            if(value != 0)
            {
                if(Time.time > _next)
                {
                    retValue = value;
                    _next = Time.time + (hold ? rate : threshold);
                    hold = true;
                }
            }
            else
            {
                hold = false;
                _next = 0;
            }
            return retValue;
        }

    }


    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    string[] _buttons = { "Submit", "Cancel" };

    public static event EventHandler<InputEvents<Vector3>> moveEvent;
    public static event EventHandler<InputEvents<int>> fireEvent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        int x = _hor.Update();
        int y = _ver.Update();
        if(x != 0 || y != 0)
        {
            if (moveEvent != null)
                moveEvent(this, new InputEvents<Vector3>(new Vector3(x, .01f, y)));
        }

        for(int i = 0; i < _buttons.Length; i++)
        {
            if (Input.GetButtonDown(_buttons[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InputEvents<int>(i));
            }
        }
		
	}
}
