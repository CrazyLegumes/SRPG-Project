using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Tile : MonoBehaviour
{
    public bool walkable;
    public bool start;
    public int x, y, f, g, h, cost;
    public Vector2 position;
    public Tile parent;
    public Text costText;

    public override bool Equals(object other)
    {
        Tile oth = other as Tile;
        if (oth == null)
            return false;


        return (oth.x == x && oth.y == y);
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static int Distance(Tile a, Tile b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    /*
    public Tile(int a, int b, bool walk = true)
    {

        walkable = walk;
        x = a;
        y = b;
        cost = 1;


        position = new Vector2(x, y);
        parent = null;
        f = g = h = 0;

    }
    */

        void Start()
    {
        f = g = h = 0;
        parent = null;
        position = new Vector2(x, y);
        if(walkable)
        costText = GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();

    }

    void Update()
    {
        if(walkable)
        costText.text = "" + cost;
    }
    public void ResetParent()
    {
        this.parent = null;
    }


}

