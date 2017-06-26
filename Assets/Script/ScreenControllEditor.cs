using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControllEditor : MonoBehaviour 
{
    #region Enums
    #endregion

    #region Delegates
    #endregion

    #region Structures
    #endregion

    #region Classes
    #endregion

    #region Fields
    public int touchCount = 0;
    private bool isClick = false;
    private Vector2 start;
    private Vector3 last;
    public Vector2 deltaPosition;
    #endregion

    #region Events
    #endregion

    #region Properties
    #endregion

    #region Methods
    void Update()
    {
        if (Input.GetMouseButton(0) && isClick)
        {
            deltaPosition = Input.mousePosition - last;
            last = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && !isClick)
        {
            touchCount = 1;
            start = Input.mousePosition;
            last = start;
        }
        if (!Input.GetMouseButton(0))
        {
            isClick = false;
            touchCount = 0;
        }
    }
    #endregion

    #region Event Handlers
    #endregion
}
