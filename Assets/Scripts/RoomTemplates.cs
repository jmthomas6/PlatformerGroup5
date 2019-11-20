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
    public bool spawnBoss;
    public GameObject boss;

     void Update()
    {
        if (waitTime <= 0 && spawnBoss == false)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if(i == roomList.Count - 1)
                {
                    Instantiate(boss, roomList[i].transform.position, Quaternion.identity);
                    spawnBoss = true;
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

}
