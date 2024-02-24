using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    private const int maxSize = 30;
    public List<Room> rooms = new List<Room>();
    private Room rootRoom = new Room(0, 0, 80, 50);

    void Start()
    {
        int counter = 0;
        bool roomSplit = true;
        rooms.Add(rootRoom);
        do
        {
            counter += 1;
            roomSplit = false;
            foreach (Room room in rooms.ToArray())
            {
                if (room.left == null && room.right == null)
                {
                    if (room.rWidth > maxSize || room.rHeight > maxSize || UnityEngine.Random.Range((float)0.1, 1) > 0.25)
                    {
                        bool test = room.Split();
                        if (test)
                        {
                            
                            rooms.Add(room.left);
                            rooms.Add(room.right);
                            roomSplit = true;
                        }
                        Debug.Log(roomSplit);
                    }
                }
            }
        } while (counter < 100);
        rootRoom.CreateRooms();
        //foreach (Room room in rooms)
        //{
        //    Debug.Log(room.x);
        //    Debug.Log(room.y);
        //    Debug.Log(string.Format("width {0}, height {1}", room.rWidth, room.rHeight));
        //    Debug.Log("\n");
        //}
    }

}
