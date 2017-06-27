using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour 
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
    [Range(0.1f,1f)]
    [SerializeField]
    private float speed = 0.1F;
    #endregion

    #region Events
    #endregion

    #region Properties
    #endregion

    #region Methods
    private void Update()
    {
      Inputs(Input.touchCount);    
    }
    private void Inputs(float _input)
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Debug.Log("touch");
            Vector2 touchDeltapossition = Input.GetTouch(0).deltaPosition;
            Rotating(touchDeltapossition);
        }

    }
    private void Rotating(Vector2 DeltaPosition)
    {
        transform.Rotate(0, 0, -DeltaPosition.x * speed);
    }
    #endregion

    #region Event Handlers
    #endregion
}
