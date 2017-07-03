using UnityEngine;

[System.Serializable]
public class ShipData {
    [Header("Скорость")]
    public float speed = 20f;
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

    public Transform tr;

}
