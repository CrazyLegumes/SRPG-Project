using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    public Transform target;
    public bool following;

    [SerializeField]
    Vector3 offset;

    Camera me;
	// Use this for initialization
	void Start () {
        me = GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
        if (target != null && following)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, 6 * Time.deltaTime);
        }

        
		
	}

    void ZoomIn()
    {

    }

    void ZoomOut()
    {

    }
}
