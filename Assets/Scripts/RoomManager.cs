using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private List<Vector2> _roomCoords, _blockedCoords, _tempCoords;
    private List<GameObject> _rooms;
    private float _coordOffset, _iterations = 5f; // Change in inspector
    private float _spawnRate = 0.5f;

    void Start()
    {
        StartCoroutine(DetermineLayout());
    }

    IEnumerator DetermineLayout()
    {
        //Start Room
        _roomCoords.Add(new Vector2(0, 0));
        if (Random.value < 0.5f)
        {
            _roomCoords.Add(new Vector2(1, 0));
            _blockedCoords.Add(new Vector2(-1, 0));
        }
        else
        {
            _roomCoords.Add(new Vector2(-1, 0));
            _blockedCoords.Add(new Vector2(1, 0));
        }
        _blockedCoords.Add(new Vector2(0, 1));
        _blockedCoords.Add(new Vector2(0, -1));

        //Iterate
        foreach (Vector2 x in _roomCoords)
        {
            if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.up))
            {
                _tempCoords.Add(x + Vector2.up);
            }
            if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.down))
            {
                _tempCoords.Add(x + Vector2.up);
            }
            if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.left))
            {
                _tempCoords.Add(x + Vector2.up);
            }
            if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.right))
            {
                _tempCoords.Add(x + Vector2.up);
            }
        }
        // Add temps to roomCoords
        // Clear temps
        // iterate 5 times
        yield return null;
    }

    IEnumerator SpawnRooms()
    {
        // for each in roomcoords, spawn at roomcoords[x] * coordoffset
        // add object to list of _rooms
        // if roomCoords (up, down, left, right) is taken, despawn room door
        yield return null;
    }

    IEnumerator AddDirt()
    {
        // find upper X and Y in roomcoords
        // loop over ever coord combo, check if taken by room, spawn dirt obj
        yield return null;
    }

    private bool CheckEmptyCoords(Vector2 x)
    {
        foreach (Vector2 y in _roomCoords)
        {
            if (x == y)
                return false;
        }
        foreach (Vector2 y in _blockedCoords)
        {
            if (x == y)
                return false;
        }
        foreach (Vector2 y in _tempCoords)
        {
            if (x == y)
                return false;
        }
        return true;
    }

    private bool CheckRoomCoords(Vector2 x)
    {
        foreach (Vector2 y in _roomCoords)
        {
            if (x == y)
                return false;
        }
        return true;
    }
}