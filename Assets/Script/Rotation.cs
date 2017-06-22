using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    #region events

    #endregion
    #region variables

    #endregion
    #region propertes           
    #endregion
    #region field  
    private Transform _transform;
    private float maxSpeedVaterfool;
    #endregion
    #region method
    private void Start()
    {
        _transform = GetComponent<Transform>();
        maxSpeedVaterfool = CenterControll.Instanse._maxSpeedVaterpoll;
    }
    private void Update()
    {
        rotationVaterPool(maxSpeedVaterfool);   
    }
    private void rotationVaterPool(float speed)
    {
        Quaternion rotate = Quaternion.AngleAxis(speed, Vector3.up);
        transform.rotation *= rotate;
    }
    
    #endregion
}
