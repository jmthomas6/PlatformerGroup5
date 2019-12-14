using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    private int _numberOfKills;
    private UIController _gc;

    [SerializeField]
    private int killsNeeded;
    [SerializeField]
    private GameObject gate;

    public int NumberOfKills
    {
        get
        {
            return _numberOfKills;
        }
        set
        {
            _numberOfKills = value;
            if (_numberOfKills >= killsNeeded)
            {
                gate.SetActive(false);
                // _gc.GateText();
            }
        }
    }

    private void Start()
    {
        _gc = FindObjectOfType<UIController>();
    }
}