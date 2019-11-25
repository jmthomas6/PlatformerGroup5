using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> roomList;

    public float waitTime;
    public bool spawnKey;
    public bool spawnDoor;
    public GameObject key;
    public GameObject door;


     void Update()
    {
        if (waitTime <= 0 && spawnDoor == false && spawnDoor == false)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if(i == roomList.Count - 1)
                {
                    Instantiate(door, roomList[i].transform.position, Quaternion.identity);
                    spawnDoor = true;
                }
                if (i == roomList.Count / 2)
                {
                    Instantiate(key, roomList[i].transform.position, Quaternion.identity);
                    spawnKey = true;
                }
            }

        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

}
