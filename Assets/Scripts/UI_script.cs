using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Panel_health_config
{
    public Transform ship_1_UI;
    public Transform ship_2_UI;
    public RectTransform panel_ship1;
    public Image img_reload1;
    public Image img_body1;
    public Image img_team1;
    public Image img_ctrl1;
    public RectTransform panel_ship2;
    public Image img_reload2;
    public Image img_body2;
    public Image img_team2;
    public Image img_ctrl2;
}

[System.Serializable]
public class Weapon_config_class
{
    [Tooltip("тип визуализации")]
    public WeaponSwitchDrawing canColor = WeaponSwitchDrawing.coloring;
    [Tooltip("Цвет включенного фона")]
    public Color color_active = Color.green;
    [Tooltip("Цвет выключенного фона")]
    public Color color_inactive = Color.gray;
    [Tooltip("Фоны кнопок")]
    public Image[] bgs;
    [Tooltip("Рамки кнопок")]
    public Image[] bgs_borders;
}

public enum WeaponSwitchDrawing
{
    coloring = 0,  // раскраска фона
    bordering = 1, // включение рамок
    color_border = 2 // рамки с раскраской
}

public class UI_script : MonoBehaviour {

    // ссылка на себя, типа как синглтон, но это чтобы не искать его в других скриптах
    public static UI_script Instance { get; private set; }

#region панели здоровья
    [Header("Панели здоровья кораблей")]
    public Panel_health_config panel_health;
    public enum ShipParent
    {
        Player = 0,
        Enemy = 1
    }
    public enum ShipHealth
    {
        Body = 0,
        Team = 1,
        Control = 2
    }
    public void SetHealthShip(ShipParent sp, ShipData sd)
    {
        switch (sp)
        {
            case ShipParent.Player:
            {
                    panel_health.img_reload1.fillAmount = sd.cannonReloadTimer / sd.maxReloadTime;
                    panel_health.img_body1.fillAmount = sd.health_body / sd.health_body_max;
                    panel_health.img_team1.fillAmount = sd.health_team / sd.health_team_max;
                    panel_health.img_ctrl1.fillAmount = sd.health_control / sd.health_control_max;
                return;
            }
            case ShipParent.Enemy:
            {
                    panel_health.img_reload2.fillAmount = sd.cannonReloadTimer / sd.maxReloadTime;
                    panel_health.img_body2.fillAmount = sd.health_body / sd.health_body_max;
                    panel_health.img_team2.fillAmount = sd.health_team / sd.health_team_max;
                    panel_health.img_ctrl2.fillAmount = sd.health_control / sd.health_control_max;
                return;
                }
            default: return;
        }
    }
    #endregion

#region выбор оружия
    [Header("Выбор оружия")]
    public Weapon_config_class weapon_config;
    private int old_wpn = -1;
    public void WeaponClick(int value)
    {
        switch (weapon_config.canColor)
        {
            case WeaponSwitchDrawing.coloring:
                {
                    if (old_wpn != -1)
                    {
                        weapon_config.bgs[old_wpn].color = weapon_config.color_inactive;
                    }
                    weapon_config.bgs[value].color = weapon_config.color_active;
                    break;
                }
            case WeaponSwitchDrawing.bordering:
                {
                    if (old_wpn != -1)
                    {
                        weapon_config.bgs_borders[old_wpn].enabled = false;
                    }
                    weapon_config.bgs_borders[value].enabled = true;
                    break;
                }
            case WeaponSwitchDrawing.color_border:
                {
                    if (old_wpn != -1)
                    {
                        weapon_config.bgs_borders[old_wpn].enabled = false;
                    }
                    weapon_config.bgs_borders[value].color = weapon_config.color_active;
                    weapon_config.bgs_borders[value].enabled = true;
                    break;
                }
        }
        old_wpn = value;
        Whirpool.Instance.ship1.cannonID = value;
    }

#endregion

#region методы монобеха
    void Update()
    {
        panel_health.panel_ship1.position = Camera.main.WorldToScreenPoint(panel_health.ship_1_UI.position);
        panel_health.panel_ship2.position = Camera.main.WorldToScreenPoint(panel_health.ship_2_UI.position);
    }

    void Start()
    {
        Instance = this;
        WeaponClick(0);
    }
    void Awake()
    {
        Instance = this;
    }
#endregion
}
