
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
    private const int minSize = 20;

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
            Vector2 roomSize = new Vector2(rand.Next(5, rWidth - 3), rand.Next(5, rHeight - 3));
            Vector2 roomPos = new Vector2(rand.Next(2, rWidth - (int)roomSize.x - 2), rand.Next(2, rHeight - (int)roomSize.y - 2));
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = new Vector3((x + roomPos.x) * 10, 0, (y + roomPos.y) * 10);
            plane.transform.localScale = new Vector3(roomSize.x, 1, roomSize.y);
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
        corridors = new List<GameObject>();

        Vector2 p1 = new Vector2(rand.Next((int)GetLeft(l) + 1, (int)GetRight(l) - 2), rand.Next((int)GetBottom(l) + 1, (int)GetTop(l) - 2));
        Vector2 p2 = new Vector2(rand.Next((int)GetLeft(r) + 1, (int)GetRight(r) - 2), rand.Next((int)GetBottom(r) + 1, (int)GetTop(r) - 2));
        //Debug.Log(String.Format("The l x; {0}, y: {1}", l.transform.position.x, l.transform.position.z));
        //Debug.Log(String.Format("The r x; {0}, y: {1}", r.transform.position.x, r.transform.position.z));
        //Debug.Log(String.Format("The p1.x: {0}, p1.y {1}", p1.x, p1.y));
        //Debug.Log(String.Format("The p2.x: {0}, p2.y {1}", p2.x, p2.y));

        float w = (p2.x - p1.x);
        float h = (p2.y - p1.y);



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
        return p.transform.position.x + bounds.extents.x;
    }

    private float GetTop(GameObject p)
    {
        Mesh mesh = p.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        return (int)p.transform.position.z + bounds.extents.z;
    }

    private float GetLeft(GameObject p)
    {
        Mesh mesh = p.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        return p.transform.position.x - bounds.extents.x;
    }

    private float GetBottom(GameObject p)
    {
        Mesh mesh = p.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        return p.transform.position.z - bounds.extents.z;
    }

    private GameObject GetPlaneObj(float p1, float p2, float w, float h)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ChangePlaneColor(plane, UnityEngine.Color.green);
        plane.transform.position = new Vector3(p1, 0, p2);
        if (w > h)
        {
            w /= 5;
            h /= 2;
        }
        else if (h > w)
        {
            h /= 5;
            w /= 2;
        }
        Debug.Log(String.Format("The w; {0}, h: {1}", w, h));
        plane.transform.localScale = new Vector3(w, 1, h);
        return plane;
    }
    void ChangePlaneColor(GameObject plane, UnityEngine.Color color)
    {
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        Material newMaterial = new Material(planeRenderer.sharedMaterial);
        newMaterial.color = color;
        planeRenderer.material = newMaterial;
    }
}