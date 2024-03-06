using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    private const int maxSize = 30;
    public List<Room> rooms = new List<Room>();
    private Room rootRoom = new Room(0, 0, 100, 100);
    System.Random rnd = new System.Random();

    void Start()
    {
        bool roomSplit = true;
        rooms.Add(rootRoom);
        do
        {
            roomSplit = false;
            foreach (Room room in rooms.ToArray())
            {
                if (room.left == null && room.right == null)
                {
                    if (room.rWidth > maxSize || room.rHeight > maxSize || rnd.Next(1, 10) > 2.5)
                    {
                        if (room.Split())
                        {
                            rooms.Add(room.left);
                            rooms.Add(room.right);
                            roomSplit = true;
                        }
                    }
                }
            }
        } while (roomSplit);
        rootRoom.CreateRooms();
    }

}