using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMaker : MonoBehaviour
{
    public GameObject wall;

    public int roomSize = 5;
    public float offset = 0.5f;
    Vector2 whereToSpawn;
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnStartRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SpawnStartRoom()
    {
        for (int x = 0; x < roomSize; x++)
        {
            whereToSpawn = new Vector2((x - roomSize) + offset, 1 + offset); 
            Instantiate(wall, whereToSpawn, Quaternion.identity);
            whereToSpawn = new Vector2((x + roomSize) + offset, 1 + offset);
            Instantiate(wall, whereToSpawn, Quaternion.identity);
        }


    }

}
