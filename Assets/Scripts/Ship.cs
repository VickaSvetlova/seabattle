using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour{

    [Header("Класс с данными корабля")]
    public ShipData data;

    private Quaternion oldRotation = Quaternion.identity;
    private Quaternion newRotation = Quaternion.identity;
    private float timer = 0f;

    void Start()
    {
        data.tr_ship = transform.parent;
        data.tr_cannon = transform.GetChild(0);
        timer = Random.Range(0f, data.timeToChange);
        oldRotation = Quaternion.Euler(data.maxAngleX, 0f, 0f);
        newRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void Update()
    {
        if (timer >= data.timeToChange)
        {
            timer = 0f;
        }
        float coeff = (Mathf.Sin(2 * Mathf.PI * timer / data.timeToChange - Mathf.PI / 2) + 1f) / 2f;
        data.tr_ship.localRotation = Quaternion.Lerp(oldRotation, newRotation, coeff);
        timer += Time.deltaTime;
        
    }
}
