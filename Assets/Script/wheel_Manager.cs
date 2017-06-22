using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel_Manager : MonoBehaviour 
{

    #region events
    #endregion
    #region variables
    private Ray ray;
    private RaycastHit hit;
    private Transform myTransform;
    private Touch myTouch;
    public GUIText textRotate;
    #endregion
    #region propertes           
    #endregion
    #region field
    #endregion
    #region method
    private void Start()
    {
        myTransform = transform;
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(myTouch.position);
            }
            if (Physics.Raycast(ray, out hit))
            {
                if ((hit.collider.name == "wheel_obj") & (myTouch.phase == TouchPhase.Moved))
                {
                    myTransform.Rotate(0, 0, myTouch.deltaPosition.x, Space.World);
                    //textRotate.text = (myTransform.rotation.z).ToString();
                }
            }
        }
    }
    #endregion
}
