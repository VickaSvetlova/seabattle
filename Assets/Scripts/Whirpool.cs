using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Whirpool : MonoBehaviour {

    // ссылка на себя, типа как синглтон, но это чтобы не искать его в других скриптах
    public static Whirpool Instance { get; private set; }
    [Header("Ссылки на водоворот")]
    public Transform tr_whirpool;
    [Range(0f, 720f)]
    [Tooltip("Скорость вращения водоворота")]
    public float speed_whirpool = 0.3f;
    [Header("Ссылки на корабли")]
    public Material mat_ship_red;
    public Material mat_ship_blue;
    [Tooltip("Трансформ вращения корабля #1")]
    public Transform tr_ship1;
    [Tooltip("Данные корабля #1")]
    public ShipData ship1;
    [Tooltip("Трансформ вращения корабля #2")]
    public Transform tr_ship2;
    [Tooltip("Данные корабля #2")]
    public ShipData ship2;
    [Header("Состояния корабля")]
    [SerializeField]
    [Tooltip("Енум управления газ/тормоз/ничего")]
    private Enum_control ship_move;
    [Tooltip("Идет ли прицеливание")]
    private bool isAiming = false;

    private enum Enum_control
    {
        none = 0,
        accelerate = 1,
        brake = 2
    }
    // ссылка на контроллер ввода
    private _CustomInput input;

#region постоянный сдвиг UV текстуры воды
    [Header("Ссылки и настройки сдвига UV фона")]
    public Renderer rend_water;
    [Tooltip("Направление сдвига")]
    public Vector2 water_dir = new Vector2(1f, 0f);
    [Tooltip("Скорость сдвига")]
    public float water_shift_speed = 0.5f;
    private void WaterShift()
    {
        Vector2 v = rend_water.material.GetTextureOffset("_MainTex");
        v += water_dir * water_shift_speed * Time.deltaTime;
        if (v.x > 1f) v.x -= 1f;
        if (v.y > 1f) v.y -= 1f;
        rend_water.material.SetTextureOffset("_MainTex", v);
    }
    #endregion

#region вращение фона (океана)
    [Header("Анимация фона")]
    public Transform tr_water;
    [Tooltip("Скорость вращения")]
    public float water_speed = 45f;
    private void WaterRotate()
    {
        tr_water.Rotate(Vector3.up, water_speed * Time.deltaTime, Space.World);
    }
#endregion

   void Awake()
    {
        Instance = this;
        ship1 = GameObject.FindGameObjectWithTag("ship_player").GetComponent<Ship>().data;
        ship2 = GameObject.FindGameObjectWithTag("ship_enemy").GetComponent<Ship>().data;
        input = _CustomInput.Instance;
    }
    void Start()
    {
        input = _CustomInput.Instance;
        Instance = this;
    }

    void Update () {
        //WaterShift();
        //WaterRotate();
        float player_move = 0f;
        float player_angle = Vector3.Dot(ship1.tr_ship.position - tr_whirpool.position, ship2.tr_ship.position - tr_whirpool.position);
        if (ship_move == Enum_control.accelerate) player_move = ship1.accelerate;
        if (ship_move == Enum_control.brake) player_move = -ship1.brake;
        if (ship_move != Enum_control.none)
        {
            Vector3 v1 = ship1.tr_ship.position - tr_whirpool.position;
            Vector3 v2 = ship2.tr_ship.position - tr_whirpool.position;
            player_angle = Vector3.Angle(v1, v2);
            if ( player_angle < 0 ) player_move = 0f;
        }
        tr_whirpool.Rotate(Vector3.up, speed_whirpool * Time.deltaTime, Space.World);
        tr_ship1.Rotate(Vector3.up, (ship1.speed + player_move) * Time.deltaTime, Space.World);
        tr_ship2.Rotate(Vector3.up, (ship2.speed) * Time.deltaTime, Space.World);

        if (ship_move == Enum_control.none)
        {
            if (input.isClick)
            {
                if (!isAiming)
                {
                    isAiming = true;
                    RaycastHit rh;
                    Ray ray = Camera.main.ScreenPointToRay(input.position);
                    LayerMask lm = LayerMask.GetMask("whirpool");
                    if (Physics.Raycast(ray, out rh, 100f, lm))
                    {
                        Vector3 v1 = rh.point;
                        WeaponData.Instance.StartAiming(ship1.cannonID, v1);
                    }
                } else
                {
                    RaycastHit rh;
                    Ray ray = Camera.main.ScreenPointToRay(input.position);
                    LayerMask lm = LayerMask.GetMask("whirpool");
                    if (Physics.Raycast(ray, out rh, 100f, lm))
                    {
                        Vector3 v1 = rh.point;
                        WeaponData.Instance.ProcessAiming(v1);
                    }
                }
            }
            else if (isAiming)
            {
                isAiming = false;
                WeaponData.Instance.EndAiming();
            }
        }
    }
    void LateUpdate () {
        UI_script.Instance.SetHealthShip(UI_script.ShipParent.Player, ship1);
        UI_script.Instance.SetHealthShip(UI_script.ShipParent.Enemy, ship2);
    }
    //void FixedUpdate () {}

    public void ImageDown(int value)
    {
        if (value == 0) ship_move = Enum_control.accelerate;
        if (value == 1) ship_move = Enum_control.brake;
    }
    public void ImageUp(int value)
    {
        ship_move = Enum_control.none;
    }
}

