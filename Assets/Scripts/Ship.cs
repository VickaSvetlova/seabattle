using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour{

    [Header("Класс с данными корабля")]
    public ShipData data;
    [Header("Данные для качки")]
    [Tooltip("Время смены наклона")]
    public float timeToChange = 1f;
    [Tooltip("Угол наклона крен")]
    public float maxAngleX = 15f;

    private Quaternion oldRotation = Quaternion.identity;
    private Quaternion newRotation = Quaternion.identity;
    private float timer = 0f;
    private bool side_s = false;

    void Start()
    {
        data.tr = transform.parent;
        newRotation = Quaternion.Euler(Random.Range(-maxAngleX, maxAngleX), 0f, 0f);
        timer = Random.Range(0f, timeToChange);
    }

    private void Update()
    {
        if (timer >= timeToChange)
        {
            oldRotation = newRotation;
            newRotation = Quaternion.Euler((side_s ? -maxAngleX : maxAngleX), 0f, 0f);
            side_s = !side_s;
            timer = 0f;
        }
        data.tr.localRotation = Quaternion.Slerp(oldRotation, newRotation, timer/timeToChange);
        timer += Time.deltaTime;
        
    }
}
