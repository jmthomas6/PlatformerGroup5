using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public GameObject rightDoor, leftDoor, topDoor, bottomDoor;
    [SerializeField]
    private List<GameObject> _dropPlatforms;

    private float _playerHeightOffset = 0.74f;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        foreach (GameObject x in _dropPlatforms)
        {
            if (_player.position.y - _playerHeightOffset > x.transform.position.y)
            {
                x.GetComponentInChildren<TilemapCollider2D>().enabled = true;
            }
            else
            {
                x.GetComponentInChildren<TilemapCollider2D>().enabled = false;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                x.GetComponentInChildren<TilemapCollider2D>().enabled = false;
            }
        }
    }
}