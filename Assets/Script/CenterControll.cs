using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tory.seabatlle;
using System;

public class CenterControll : MonoBehaviour {

    #region events
    public Action<float> _changeSpeed;
    #endregion
    #region variables
    [Range(1, 100)]
    public float tempSpeedVariableShip;
    private float _tempSpeedVariableShip;
    public float _maxSpeedVaterpoll;
    #endregion
    #region propertes           
    #endregion
    #region field   
    #endregion
    #region method 
    private void Awake()
    {
        _tempSpeedVariableShip = tempSpeedVariableShip;
        Instanse = this;
    }
    private void Update()
    {
        if (tempSpeedVariableShip != _tempSpeedVariableShip)
        {
            _changeSpeed.Invoke(tempSpeedVariableShip);
        }

    }
    public static CenterControll Instanse { get; set; }
    #endregion
}
