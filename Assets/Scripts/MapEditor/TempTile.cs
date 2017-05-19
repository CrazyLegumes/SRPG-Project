using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TempTile : MonoBehaviour
{

    private Renderer rend;
    private MaterialPropertyBlock block;
    public bool MouseOn;
    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        block = new MaterialPropertyBlock();
        rend = GetComponent<Renderer>();



    }

    void OnMouseEnter()
    {
        MouseOn = true;
    }

    void OnMouseExit()
    {
        MouseOn = false;
    }

    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (block != null)
        {
            rend.GetPropertyBlock(block);
            if (MouseOn)
            {

                block.SetFloat("CanFade", 1);
            }
            if (!MouseOn)
            {

                block.SetFloat("CanFade", 0);
            }
            rend.SetPropertyBlock(block);
        }
    }
}
