using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int ammoID = 0;
    public Transform parent;

	//void OnCollisionEnter(Collision __collision) {}
	void OnTriggerEnter(Collider _col)
    {
        _col.gameObject.GetComponent<Ship>().data.Damage(ammoID);
        Destroy(this.gameObject);
        // запускаем анимашку взрыва
        //
    }

	//void Awake(){}
	//void OnEnable(){}
	//void OnDisable(){}
	//void OnDestroy(){}
	//void OnGUI(){}
	//void Start () {}
	//void Update () {}
	//void LateUpdate () {}
	void FixedUpdate ()
    {
        if (Vector3.SqrMagnitude(parent.position - transform.position) > 400f)
        {
            Destroy(this.gameObject);
            // запускаем анимашку взрыва
            //
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
