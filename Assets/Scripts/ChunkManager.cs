using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public GameObject rightDoor, leftDoor, topDoor, bottomDoor;
    [SerializeField]
    private List<GameObject> _dropPlatforms;
    //[SerializeField]
    private float _playerHeightOffset = 0.735f;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        //if (_dropPlatforms != null && _dropPlatforms.Count > 0)
        {
            //print(_dropPlatforms.Count);
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
                    print(_player.position.y + ", " + x.transform.position.y);
                    x.GetComponentInChildren<TilemapCollider2D>().enabled = false;
                }
            }
        }
    }
}