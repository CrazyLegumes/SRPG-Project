using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;





[CustomEditor(typeof(MapEditor))]
public class MapEditorInspector : Editor
{

    [MenuItem("Create/Tileset")]
    private static void createTileset()
    {
        var asset = CreateInstance<TileSet>();
        string path = "Assets/Resources/Prefabs/Tiles/";
        AssetDatabase.CreateAsset(asset, path + "Tileset.asset");
        EditorUtility.FocusProjectWindow();
    }

    
    private MapEditor editor;
    SerializedProperty mapData;
    SerializedObject mapObj;
    SerializedProperty SetData;
    SerializedProperty listData;
    DataMap data;
    TileSet mySet;
   
    private List<Tile> map = new List<Tile>();
  

    private List<string> prefabs;
    string tilepath;
    int oldindex = 0;

    void OnEnable()
    {
        editor = (MapEditor)target;
        mapObj = new SerializedObject(target);
        editor.tempTile = Resources.Load<GameObject>("Prefabs/Tiles/TempTile");
        mapData = mapObj.FindProperty("mapdata");
        SetData = mapObj.FindProperty("tileSet");
        listData = mapObj.FindProperty("TileList");
        
        // GetTiles();


    }


    void GetTiles()
    {

        tilepath = Application.dataPath + "/Resources/Prefabs/Tiles/";
        string[] files = Directory.GetFiles(tilepath);
        prefabs = new List<string>();
        foreach (string x in files)
        {
            if (!x.Contains(".meta"))
                prefabs.Add(Path.GetFileName(x));

        }

        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabs[i] = prefabs[i].Replace(".prefab", "");

        }

    }


    public void ClearMap()
    {
        foreach(Tile a in map.ToArray())
        {
            DestroyImmediate(a.gameObject);
        }
    }

    public override void OnInspectorGUI()
    {
        

        //Grid Lines and Stuff

        mapObj.Update();
        EditorUtility.SetDirty(editor);
        editor.mapLength = Mathf.Clamp(editor.mapLength, 5, 40);
        editor.mapWidth = Mathf.Clamp(editor.mapWidth, 5, 40);
        editor.mapLength = EditorGUILayout.IntField("Map Length", editor.mapLength);
        editor.mapWidth = EditorGUILayout.IntField("Map Width", editor.mapWidth);


        GUILayout.Label("Tile Set Data");

       
        mySet = (TileSet)EditorGUILayout.ObjectField(SetData.objectReferenceValue, typeof(TileSet), false);
        GameObject currentTile = (GameObject)EditorGUILayout.ObjectField("Current Tile", editor.currentTile, typeof(GameObject), false);

        if (mySet != null)
        {
            EditorGUI.BeginChangeCheck();
            editor.tileSet = mySet;
            string[] tiles = new string[editor.tileSet.tiles.Length];
            int[] values = new int[tiles.Length];

            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = editor.tileSet.tiles[i].name;
                values[i] = i;
            }

            int index = EditorGUILayout.IntPopup("Select Tile", oldindex, tiles, values);
            if (EditorGUI.EndChangeCheck())
            {
                if (oldindex != index)
                    oldindex = index;
                Undo.RecordObject(editor.currentTile, "Changed Tile");

            }
            editor.currentTile = editor.tileSet.tiles[oldindex];
        }
        

     
        GUILayout.Label("Map Data");
        data = EditorGUILayout.ObjectField(mapData.objectReferenceValue, typeof(DataMap), true) as DataMap;





       if(data == null && GUI.changed)
        {
            editor.mapset = false;
        }

        if (data != null && !editor.mapset)
        {


            editor.mapset = true;
            editor.mapdata = data;
            
            editor.map = data.data;

        }

        

        if (GUILayout.Button("Place Temp Map"))
        {
            editor.InstantiateTempTile();
           
        }

        if(GUILayout.Button("Clear Map"))
        {
            ClearMap();
        }

        if (GUILayout.Button("Save Map"))
        {



            GameObject asset = new GameObject("MapData");

            asset.AddComponent<DataMap>();
            DataMap  assetData = asset.GetComponent<DataMap>();
           
            assetData.count = 0;
            
            assetData.data = new Tile[editor.mapWidth * editor.mapLength];

            for (int i = 0; i < map.Count; i++)
            {
                Tile tile = map[i];
                assetData.data[tile.y * editor.mapWidth +  tile.x] = tile;
                
                assetData.count++;
            }


            

        }



        
        




    }

    void OnSceneGUI()
    {
        Event e = Event.current;
        int controllid = GUIUtility.GetControlID(FocusType.Passive);

        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

      
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {

           
            if (hit.transform.GetComponent<TempTile>() != null)
            {

                editor.tileToRepalce = hit.transform.gameObject;
            }
        


        }
        
        if (e.isMouse && e.type == EventType.MouseDown && hit.transform != null && e.button == 0)
        {


            GUIUtility.hotControl = controllid;
            e.Use();


            Vector3 allign = hit.transform.position;
            GameObject prefab = PrefabUtility.InstantiatePrefab(editor.currentTile) as GameObject;
            Tile tile = prefab.GetComponent<Tile>();

            tile.x = (int)allign.x;
            tile.y = (int)allign.z;

            prefab.transform.position = allign;
            prefab.transform.parent = editor.transform;
            
            if (hit.transform.GetComponent<Tile>() != null)
            {
                Tile replace = hit.transform.GetComponent<Tile>();
                map.Remove(replace);
                map.Add(tile);
            }
            else
            {
                map.Add(tile);
            }

            DestroyImmediate(hit.transform.gameObject);

        }
        else if (e.isMouse && e.type == EventType.mouseDown && hit.transform != null && e.button == 1)
        {
            GUIUtility.hotControl = controllid;
            e.Use();
            Vector3 allign = hit.transform.position;
            GameObject prefab = PrefabUtility.InstantiatePrefab(editor.tempTile) as GameObject;

            prefab.transform.position = allign;
            prefab.transform.parent = editor.transform;



            if (hit.transform.GetComponent<Tile>() != null)
            {
                Tile tile = hit.transform.GetComponent<Tile>();
                map.Remove(tile);
            }

            DestroyImmediate(hit.transform.gameObject);

        }

        if (e.isMouse && e.type == EventType.MouseUp)
        {
            GUIUtility.hotControl = 0;
        }
    }



}




