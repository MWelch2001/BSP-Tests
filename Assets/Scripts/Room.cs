
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Drawing;
using UnityEngine;

public class Room
{
    private const int minSize = 14;
    
    public int rHeight, rWidth, x, y;
    public Room left, right;
    public List<Room> roomList;
    System.Random rand = new System.Random();
    public Room(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        rHeight =  height;
        rWidth = width;
    }

    public bool Split()
    {
     rand.Next();
        bool splitVertical =  (rand.Next(1, 10) > 5);
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

		} else
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
        } else
        {
            Vector2 roomSize = new Vector2(rand.Next(3, rWidth - 1), rand.Next(3, rHeight - 1));
            Vector2 roomPos = new Vector2(rand.Next(1, rWidth - (int)roomSize.x - 2), rand.Next(1, rHeight - (int)roomSize.y - 2));
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = new Vector3(x + roomPos.x, 0, y + roomPos.y);
            plane.transform.localScale = new Vector3(roomSize.x/ 10, roomSize.y / 10, 1);
        }

    }
}