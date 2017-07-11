using UnityEngine;

[System.Serializable]
public class ShipData {
    [Header("Скорость")]
    [Tooltip("Замедление от скорости водоворота")]
    public float speed = -5f;
    public float accelerate = 10f;
    public float brake = 15f;
    [Header("Максимальные значения")]
    public float health_body_max = 100f;
    public float health_team_max = 100f;
    public float health_control_max = 100f;
    [Header("Текущие значения")]
    public float health_body = 100f;
    public float health_team = 100f;
    public float health_control = 100f;

    [Tooltip("Трансформ корабля")]
    public Transform tr_ship;
    [Tooltip("Трансформ пушки")]
    public Transform tr_cannon;
    [Tooltip("ID пушки")]
    public int cannonID;

    [Header("Данные для качки")]
    [Tooltip("Время смены наклона")]
    public float timeToChange = 1f;
    [Tooltip("Угол наклона крен")]
    public float maxAngleX = 15f;
}
