using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMapGen : MonoBehaviour
{
    // Start is called before the first frame update
    public TileBase tile;

    void Start()
    {
        //int counter = 0;
        GameObject map = GameObject.Find("Dungeon");
        Tilemap tileMap = map.GetComponent<Tilemap>();
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            //Debug.Log(string.Format("rooms: {0}", verts.Length));
            foreach (Vector3 vertex in GetVertices(room)) {
                //Debug.Log(string.Format("x: {0}, z: {1}", (int)room.transform.position.x + (int)vertex.x, (int)room.transform.position.z + (int)vertex.z));
                //Debug.Log(string.Format("rooms: {0}", counter += 1));
                tileMap.SetTile(new Vector3Int( (int)room.transform.position.x + (int)vertex.x, 0, (int)room.transform.position.z + (int)vertex.z), tile);
            }   
            //Destroy(room);
        } 
    }

    private Vector3[] GetVertices(GameObject plane)
    {
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
        return mesh.vertices;
    }
}
