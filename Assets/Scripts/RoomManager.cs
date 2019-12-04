using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private List<Vector2> _roomCoords, _blockedCoords, _tempCoords;
    private List<GameObject> _rooms;
    [SerializeField]
    private List<GameObject> _roomPrefabs;
    [SerializeField]
    private GameObject _startRoom, _gateRoom, _dirt;
    [SerializeField]
    private float _coordOffset, _roomCount, _spawnRate;
    private int _gateIndex, _endIndex = 0;

    void Start()
    {
        _roomCoords = new List<Vector2>();
        _blockedCoords = new List<Vector2>();
        _tempCoords = new List<Vector2>();
        _rooms = new List<GameObject>();
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

        //Iterate first group
        while (_roomCoords.Count < _roomCount)
        {
            foreach (Vector2 x in _roomCoords)
            {
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.up))
                {
                    _tempCoords.Add(x + Vector2.up);
                }
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.down))
                {
                    _tempCoords.Add(x + Vector2.down);
                }
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.left))
                {
                    _tempCoords.Add(x + Vector2.left);
                }
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.right))
                {
                    _tempCoords.Add(x + Vector2.right);
                }
            }
            foreach (Vector2 x in _tempCoords)
            {
                _roomCoords.Add(x);
            }
            _tempCoords.Clear();
        }

        // GATE ROOM
        while (_gateIndex == 0)
        {
            foreach (Vector2 x in _roomCoords)
            {
                int surroundingCount = 0;
                if (!CheckRoomCoords(x + Vector2.up))
                    surroundingCount++;
                if (!CheckRoomCoords(x + Vector2.down))
                    surroundingCount++;
                if (surroundingCount < 2 && !CheckRoomCoords(x + Vector2.left))
                    surroundingCount++;
                if (surroundingCount < 2 && !CheckRoomCoords(x + Vector2.right))
                    surroundingCount++;
                if (surroundingCount < 2)
                {
                    _gateIndex = _roomCoords.IndexOf(x);
                }
            }
            if (_gateIndex == 0)
            {
                if (Random.value < 0.5f && CheckEmptyCoords(_roomCoords[_roomCoords.Count - 1] + Vector2.left))
                {
                    _tempCoords.Add(_roomCoords[_roomCoords.Count - 1] + Vector2.left);
                    _roomCoords.Add(_tempCoords[0]);
                    _tempCoords.Clear();
                }
                else if (CheckEmptyCoords(_roomCoords[_roomCoords.Count - 1] + Vector2.right))
                {
                    _tempCoords.Add(_roomCoords[_roomCoords.Count - 1] + Vector2.right);
                    _roomCoords.Add(_tempCoords[0]);
                    _tempCoords.Clear();
                }
            }
        }
        if (!CheckRoomCoords(_roomCoords[_gateIndex] + Vector2.up))
        {
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.left);
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.right);
            CloseFirstArea(_roomCoords[_gateIndex] + Vector2.down);
        }
        else if (!CheckRoomCoords(_roomCoords[_gateIndex] + Vector2.down))
        {
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.left);
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.right);
            CloseFirstArea(_roomCoords[_gateIndex] + Vector2.up);
        }
        else if (!CheckRoomCoords(_roomCoords[_gateIndex] + Vector2.left))
        {
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.up);
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.down);
            CloseFirstArea(_roomCoords[_gateIndex] + Vector2.right);
        }
        else if (!CheckRoomCoords(_roomCoords[_gateIndex] + Vector2.right))
        {
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.up);
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.down);
            CloseFirstArea(_roomCoords[_gateIndex] + Vector2.left);
        }

        
        //Iterate second group
        while (_roomCoords.Count < (_roomCount * 2))
        {
            foreach (Vector2 x in _roomCoords)
            {
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.up))
                {
                    _tempCoords.Add(x + Vector2.up);
                }
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.down))
                {
                    _tempCoords.Add(x + Vector2.down);
                }
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.left))
                {
                    _tempCoords.Add(x + Vector2.left);
                }
                if (Random.value < _spawnRate && CheckEmptyCoords(x + Vector2.right))
                {
                    _tempCoords.Add(x + Vector2.right);
                }
            }
            foreach (Vector2 x in _tempCoords)
            {
                _roomCoords.Add(x);
            }
            _tempCoords.Clear();
        }

        // END ROOM
        float roomDist = 0;
        Vector2 furthestRoom;
        foreach (Vector2 x in _roomCoords)
        {
            if (Vector2.Distance(Vector2.zero, x) > roomDist)
            {
                roomDist = Vector2.Distance(Vector2.zero, x);
                furthestRoom = x;
            }
        }
        /*
        if (!CheckRoomCoords(_roomCoords[_gateIndex] + Vector2.left))
        {
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.up);
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.down);
            CloseFirstArea(_roomCoords[_gateIndex] + Vector2.right);
        }
        else if (!CheckRoomCoords(_roomCoords[_gateIndex] + Vector2.right))
        {
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.up);
            _blockedCoords.Add(_roomCoords[_gateIndex] + Vector2.down);
            CloseFirstArea(_roomCoords[_gateIndex] + Vector2.left);
        }*/

        StartCoroutine(SpawnRooms());
        yield return null;
    }

    IEnumerator SpawnRooms()
    {
        foreach (Vector2 x in _roomCoords)
        {
            GameObject roomObj;
            if (x == Vector2.zero)
            {
                roomObj = Instantiate(_startRoom);
                roomObj.transform.position = x;
            }
            else if (_roomCoords.IndexOf(x) == _gateIndex)
            {
                roomObj = Instantiate(_gateRoom);
                roomObj.transform.position = x;
            }
            else
            {
                int roomIndex = Random.Range(0, _roomPrefabs.Count);
                roomObj = Instantiate(_roomPrefabs[roomIndex]);
                roomObj.transform.position = x * _coordOffset;
            }
            _rooms.Add(roomObj); // Do I need this?

            if (!CheckRoomCoords(x + Vector2.up))
                roomObj.GetComponent<ChunkManager>().topDoor.SetActive(false);
            if (!CheckRoomCoords(x + Vector2.down))
                roomObj.GetComponent<ChunkManager>().bottomDoor.SetActive(false);
            if (!CheckRoomCoords(x + Vector2.left))
                roomObj.GetComponent<ChunkManager>().leftDoor.SetActive(false);
            if (!CheckRoomCoords(x + Vector2.right))
                roomObj.GetComponent<ChunkManager>().rightDoor.SetActive(false);
        }

        StartCoroutine(AddDirt());
        yield return null;
    }

    IEnumerator AddDirt()
    {
        int minX = 0;
        int maxX = 0;
        int minY = 0;
        int maxY = 0;
        foreach (Vector2 x in _roomCoords)
        {
            if (x.x < minX)
                minX = Mathf.RoundToInt(x.x);
            if (x.x > maxX)
                maxX = Mathf.RoundToInt(x.x);
            if (x.y < minY)
                minY = Mathf.RoundToInt(x.y);
            if (x.y > maxY)
                maxY = Mathf.RoundToInt(x.y);
        }
        minX--;
        maxX++;
        minY--;
        maxY++;
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (CheckRoomCoords(new Vector2(x, y)))
                {
                    GameObject dirtObj = Instantiate(_dirt);
                    dirtObj.transform.position = new Vector2(x * _coordOffset, y * _coordOffset);
                }
            }
        }
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

    private void CloseFirstArea(Vector2 exemption)
    {
        foreach (Vector2 x in _roomCoords)
        {
            if (x + Vector2.up != exemption && CheckRoomCoords(x + Vector2.up))
                _blockedCoords.Add(x + Vector2.up);
            if (x + Vector2.down != exemption && CheckRoomCoords(x + Vector2.down))
                _blockedCoords.Add(x + Vector2.down);
            if (x + Vector2.left != exemption && CheckRoomCoords(x + Vector2.left))
                _blockedCoords.Add(x + Vector2.left);
            if (x + Vector2.right != exemption && CheckRoomCoords(x + Vector2.right))
                _blockedCoords.Add(x + Vector2.right);
        }
        _roomCoords.Add(exemption);
    }
}