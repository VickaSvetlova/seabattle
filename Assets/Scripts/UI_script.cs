using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_script : MonoBehaviour {

    // ссылка на себя, типа как синглтон, но это чтобы не искать его в других скриптах
    public static UI_script Instance { get; private set; }

    public Transform ship_1_UI;
    public Transform ship_2_UI;
    public RectTransform panel_ship1;
    public Image img_body1;
    public Image img_team1;
    public Image img_ctrl1;
    public RectTransform panel_ship2;
    public Image img_body2;
    public Image img_team2;
    public Image img_ctrl2;
    //void OnEnable(){}
    //void OnDisable(){}
    //void OnDestroy(){}
    //void OnGUI(){}
    void Start () {
        if (!Instance) Instance = this;
    }
    void Awake()
    {
        if (!Instance) Instance = this;
    }
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
                img_body1.fillAmount = sd.health_body/sd.health_body_max; 
                img_team1.fillAmount = sd.health_team/sd.health_team_max;
                img_ctrl1.fillAmount = sd.health_control/sd.health_control_max;
                return;
            }
            case ShipParent.Enemy:
            {
                img_body2.fillAmount = sd.health_body/sd.health_body_max;
                img_team2.fillAmount = sd.health_team/sd.health_team_max;
                img_ctrl2.fillAmount = sd.health_control/sd.health_control_max;
                return;
                }
            default: return;
        }
    }
    void Update () {
        panel_ship1.position = Camera.main.WorldToScreenPoint(ship_1_UI.position);
        panel_ship2.position = Camera.main.WorldToScreenPoint(ship_2_UI.position);
    }
    //void LateUpdate () {}
    //void FixedUpdate () {}

}
