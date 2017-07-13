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
    [Tooltip("Флаг перезарядки")]
    public bool cannonReload = false;
    [Tooltip("Флаг переключения")]
    public bool cannonSwitch = false;
    [Tooltip("Таймер перезарядки")]
    public float cannonReloadTimer = 0f;
    [Tooltip("Таймер переключения")]
    public float cannonSwitchTimer = 0f;
//    [Tooltip("")]

    [Header("Данные для качки")]
    [Tooltip("Время смены наклона")]
    public float timeToChange = 1f;
    [Tooltip("Угол наклона крен")]
    public float maxAngleX = 15f;

    public void ReloadCannon()
    {
        cannonReload = true;
        cannonReloadTimer = WeaponData.Instance.cannons[cannonID].reloadTime;
    }

    public void SwitchCannon(int id)
    {
        cannonSwitch = true;
        cannonSwitchTimer = WeaponData.Instance.cannons[cannonID].switchTime;
    }

    public void ProcessTimers(float t)
    {
        if (cannonReload)
        {
            cannonReloadTimer -= t;
            if (cannonReloadTimer <= 0f)
                cannonReload = false;
        }
        if (cannonSwitch)
        {
            cannonSwitchTimer -= t;
            if (cannonSwitchTimer <= 0f)
                cannonSwitch = false;
        }
    }

    public void Damage(int _ammo)
    {
        AmmoItem ai = WeaponData.Instance.ammos[_ammo];
        float base_dmg = ai.damage * ai.target.body;
        float team_dmg = ai.damage * ai.target.team;
        float ctrl_dmg = ai.damage * ai.target.control;
        bool isMiss = (Random.Range(0f, 1f) <= ai.missChance) ? true : false;
        bool isCrit = (Random.Range(0f, 1f) <= ai.critChance) ? true : false;
        if (isCrit)
        {
            base_dmg *= ai.critMultiplier;
            team_dmg *= ai.critMultiplier;
            ctrl_dmg *= ai.critMultiplier;
            Debug.Log("Crit!");
        }
        if (!isMiss)
        {
            health_body = Mathf.Clamp(health_body - base_dmg, 0f, health_body_max);
            health_team = Mathf.Clamp(health_team - team_dmg, 0f, health_team_max);
            health_control = Mathf.Clamp(health_control - ctrl_dmg, 0f, health_control_max);
        }
    }
}
