using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseUnit : MonoBehaviour {
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

    */


    public int moveRange;
    public int attackRange;
    public bool active;
    public bool selected;
    public Vector3 cancelPos;
    public Material mat;
    public Team team;
    //doot

    public PlayerPathing unitPathing;
	
	void Start () {
        active = true;
        unitPathing = GetComponent<PlayerPathing>();
        mat = GetComponent<Renderer>().material;
        if (team == Team.allies)
            mat.SetFloat("_enemy", 0);
        else if (team == Team.enemies)
            mat.SetFloat("_enemy", 1);
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
