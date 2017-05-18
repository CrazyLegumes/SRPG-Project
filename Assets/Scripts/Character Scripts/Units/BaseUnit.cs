using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseUnit : MonoBehaviour {

    /*
        Unit Specifications:
        Move Range
        Attack Range
        Active


        -------Stats-------

    */


    public int moveRange;
    public int attackRange;
    public bool active;
    public bool selected;
    public Vector3 cancelPos;
    public Material mat;
    //doot

    public PlayerPathing unitPathing;
	
	void Start () {
        active = true;
        unitPathing = GetComponent<PlayerPathing>();
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMaterial();
		
	}


    void UpdateMaterial()
    {
        float inactive = active ? 0 : 1;
        float highlight = selected ? 1 : 0;
        mat.SetFloat("_highlight", highlight);
        mat.SetFloat("_inactive", inactive);


    }
}
