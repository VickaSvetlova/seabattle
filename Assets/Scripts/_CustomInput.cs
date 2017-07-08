using UnityEngine;
using System.Collections;

public class _CustomInput : MonoBehaviour {

    public static _CustomInput Instance { get; private set; }
    
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Instance = this;
    }
    //void OnEnable(){}
    //void OnDisable(){}
    //void OnDestroy(){}

    // Есть ли тап/клик
    public bool isClick
    {
        get {
#if UNITY_ANDROID || UNITY_IOS
            return Input.touchCount > 0;
#else
            return Input.GetMouseButton(0);
#endif
        }
    }

    // координаты тапа/клика
    public Vector2 position
    {
        get {
#if UNITY_ANDROID || UNITY_IOS
            return Input.touchCount == 0 ? -Vector2.one : Input.GetTouch(0).position;
#else
            return Input.GetMouseButton(0) ? (Vector2)Input.mousePosition : -Vector2.one;
#endif
        }
    }
	
}
