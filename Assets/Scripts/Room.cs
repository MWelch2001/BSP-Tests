
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Drawing;
using UnityEngine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.ParticleSystemJobs;
using System.Reflection;
using System.Security.Cryptography;

public class Room
{
    private const int minSize = 14;

    public int rHeight, rWidth, x, y;
    public Room left, right;
    public List<Room> roomList;
    public GameObject plane;
    private System.Random rand = new System.Random();
    private GameObject lPlane;
    private GameObject rPlane;
    List<GameObject> corridors;

    public Room(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        rHeight = height;
        rWidth = width;
    }

    public bool Split()
    {
        rand.Next();
        bool splitVertical = (rand.Next(1, 10) > 5);
        if (left != null || right != null)
        {
            return false;
        }
        splitVertical = GetSplitDirection();
        int maxSize = GetMaxSize(splitVertical);
        if (maxSize <= minSize)
        {
            return false;
        }
        int splitLoc = rand.Next(minSize, maxSize);


        if (splitVertical)
        {
            left = new Room(x, y, splitLoc, rHeight);
            right = new Room(x + splitLoc, y, rWidth - splitLoc, rHeight);

        }
        else
        {
            left = new Room(x, y, rWidth, splitLoc);
            right = new Room(x, y + splitLoc, rWidth, rHeight - splitLoc);
        }

        return true;
    }

    private int GetMaxSize(bool splitVertical)
    {
        int size;
        if (splitVertical)
        {
            return size = rWidth - minSize;
        }
        return size = rHeight - minSize;
    }

    private bool GetSplitDirection()
    {
        bool split = (rand.Next(1, 10) > 5);
        if (rWidth > rHeight && rWidth / rHeight >= 1.25)
        {
            split = true;
        }
        else if (rHeight > rWidth && rHeight / rWidth >= 1.25)
        {
            split = false;
        }
        return split;
    }

    public void CreateRooms()
    {
        if (left != null || right != null)
        {
            if (left != null)
            {
                left.CreateRooms();
            }
            if (right != null)
            {
                right.CreateRooms();
            }
            if (left != null && right != null)
            {
                CreateCorridor(left.GetPlane(), right.GetPlane());
            }
        }
        else
        {
            Vector2 roomSize = new Vector2(rand.Next(3, rWidth - 2), rand.Next(3, rHeight - 2));
            Vector2 roomPos = new Vector2(rand.Next(1, rWidth - (int)roomSize.x - 1), rand.Next(1, rHeight - (int)roomSize.y - 1));
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = new Vector3(x + roomPos.x, 0, y + roomPos.y);
            plane.transform.localScale = new Vector3(roomSize.x / 11, 1, roomSize.y / 11);
        }
    }

    public GameObject GetPlane()
    {
        if (plane != null)
        {
            return plane;
        }
        else
        {

            if (left != null)
            {
                lPlane = left.GetPlane();
            }
            if (right != null)
            {
                rPlane = right.GetPlane();
            }
            if (lPlane == null && rPlane == null)
            {
                return null;
            }
            else if (rPlane == null)
            {
                return lPlane;
            }
            else if (lPlane == null)
            {
                return rPlane;
            }
            else if (rand.Next(1, 10) > 5)
            {
                return lPlane;
            }
            else
            {
                return rPlane;
            }
        }
    }

    public void CreateCorridor(GameObject l, GameObject r)
    {
        //Debug.Log("i work first");
        corridors = new List<GameObject>();

        Vector2 p1 = new Vector2(rand.Next((int)l.transform.position.x + 1, (int)GetRight(l) - 2), rand.Next((int)l.transform.position.z + 1, (int)GetTop(l) - 2));
        Vector2 p2 = new Vector2(rand.Next((int)r.transform.position.x + 1, (int)GetRight(r) - 2), rand.Next((int)r.transform.position.z + 1, (int)GetTop(r) - 2));

        float w = (p2.x - p1.x) / 44;
        float h = (p2.y - p1.y) / 44;

        if (w < 0)
        {
            if (h < 0)
            {
                if (rand.Next(1, 10) > 5)
                {
                    corridors.Add(GetPlaneObj(p2.x, p1.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p2.x, p2.y, 1, Math.Abs(h)));
                }
                else
                {
                    corridors.Add(GetPlaneObj(p2.x, p2.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p1.x, p2.y, 1, Math.Abs(h)));
                }
            }
            else if (h > 0)
            {
                if (rand.Next(1, 10) > 5)
                {
                    corridors.Add(GetPlaneObj(p2.x, p1.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p2.x, p1.y, 1, Math.Abs(h)));
                }
                else
                {
                    corridors.Add(GetPlaneObj(p2.x, p2.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p1.x, p1.y, 1, Math.Abs(h)));
                }
            }
            else
            {
                corridors.Add(GetPlaneObj(p2.x, p2.y, Math.Abs(w), 1));
            }
        }
        else if (w > 0)
        {
            if (h < 0)
            {
                if (rand.Next(1, 10) > 5)
                {
                    corridors.Add(GetPlaneObj(p1.x, p2.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p1.x, p2.y, 1, Math.Abs(h)));
                }
                else
                {
                    corridors.Add(GetPlaneObj(p1.x, p1.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p2.x, p2.y, 1, Math.Abs(h)));
                }
            }
            else if (h > 0)
            {
                if (rand.Next(1, 10) > 5)
                {
                    corridors.Add(GetPlaneObj(p1.x, p1.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p2.x, p1.y, 1, Math.Abs(h)));
                }
                else
                {
                    corridors.Add(GetPlaneObj(p1.x, p2.y, Math.Abs(w), 1));
                    corridors.Add(GetPlaneObj(p1.x, p1.y, 1, Math.Abs(h)));
                }
            }
            else
            {
                corridors.Add(GetPlaneObj(p1.x, p1.y, Math.Abs(w), 1));
            }
        }
        else
        {
            if (h < 0)
            {
                corridors.Add(GetPlaneObj(p2.x, p2.y, 1, Math.Abs(h)));

            }
            else if (h > 0)
            {
                corridors.Add(GetPlaneObj(p1.x, p1.y, 1, Math.Abs(h)));
            }
        }
    }

    private float GetRight(GameObject p)
    {
        Mesh mesh = p.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        //Debug.Log(String.Format("The xBound vibes: {0}", bounds.size.x));
        //Debug.Log(String.Format("The xPos vibes: {0}", p.transform.position.x));
        return p.transform.position.x + bounds.max.x;
    }

    private float GetTop(GameObject p)
    {
        Mesh mesh = p.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        //Debug.Log(String.Format("The zBound vibes: {0}", bounds.size.z));
        //Debug.Log(String.Format("The zPos vibes: {0}", p.transform.position.z));
        return (int)p.transform.position.z + bounds.max.z;
    }

    private GameObject GetPlaneObj(float p1, float p2, float p3, float p4)
    {
        //Debug.Log("I am working!");
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ChangePlaneColor(plane, UnityEngine.Color.green);
        plane.transform.position = new Vector3(p1, 0, p2);
        plane.transform.localScale = new Vector3(p3, 1, p4);
        return plane;
    }
    void ChangePlaneColor(GameObject plane, UnityEngine.Color color)
    {
        // Access the Renderer component
        Renderer planeRenderer = plane.GetComponent<Renderer>();

        // Create a new material to avoid modifying the shared material
        Material newMaterial = new Material(planeRenderer.sharedMaterial);

        // Set the color of the new material
        newMaterial.color = color;

        // Assign the new material to the plane
        planeRenderer.material = newMaterial;
    }
}