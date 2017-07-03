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
    [Tooltip("Минимальный угол между кораблями")]
    public float min_angle = 90f;
    [Tooltip("Прицело")]
    public Transform tr_aim;
    public Transform tr_canvas_aim;
    [Tooltip("Идет ли прицеливание")]
    private bool isAiming = false;
    [Tooltip("Спрайт левой части прицела")]
    public Image _aim_left;
    [Tooltip("Спрайт правой части прицела")]
    public Image _aim_right;
    [Tooltip("Цвет прицела в начале")]
    public Color aim_color_start;
    [Tooltip("Цвет прицела в конце")]
    public Color aim_color_end;
    [Tooltip("Размер сектора прицела в начале")]
    public float aim_size_start = 0.075f;
    [Tooltip("Размер сектора прицела в конце")]
    public float aim_size_end = 0.01f;
    [Tooltip("Время сведения")]
    public float aim_time = 2f;

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
        if (!Instance) Instance = this;
        ship1 = GameObject.FindGameObjectWithTag("ship_player").GetComponent<Ship>().data;
        ship2 = GameObject.FindGameObjectWithTag("ship_enemy").GetComponent<Ship>().data;
        _aim_left.enabled = false;
        _aim_right.enabled = false;
        input = _CustomInput.Instance;
    }

    void Update () {
        //WaterShift();
        //WaterRotate();
        float player_move = 0f;
        float player_angle;
        if (ship_move == Enum_control.accelerate) player_move = ship1.accelerate;
        if (ship_move == Enum_control.brake) player_move = -ship1.brake;
        if (ship_move != Enum_control.none)
        {
            Vector3 v1 = ship1.tr.position - tr_ship1.position;
            Vector3 v2 = ship2.tr.position - tr_ship2.position;
            player_angle = Vector3.Angle(v1, v2);
            if (player_angle < min_angle) player_move = 0f;
        }
        tr_whirpool.Rotate(Vector3.up, speed_whirpool * Time.deltaTime, Space.World);
        tr_ship1.Rotate(Vector3.up, (ship1.speed + player_move) * Time.deltaTime, Space.World);
        tr_ship2.Rotate(Vector3.up, (ship2.speed) * Time.deltaTime, Space.World);

        if (ship_move == Enum_control.none)
        {
            if (input.isClick)
            {
                isAiming = true;
                _aim_left.enabled = true;
                _aim_right.enabled = true;
                RaycastHit rh;
                Ray ray = Camera.main.ScreenPointToRay(input.position);
                LayerMask lm = LayerMask.GetMask( "whirpool" );
                if (Physics.Raycast(ray,out rh, 100f, lm))
                {
                    Vector3 v1 = rh.point;
                    v1.y = tr_canvas_aim.position.y;
                    tr_canvas_aim.LookAt(v1);
                }
                _aim_left.fillAmount -= (aim_size_start - aim_size_end) * (Time.deltaTime / aim_time);
                _aim_left.fillAmount = Mathf.Clamp(_aim_left.fillAmount, aim_size_end, aim_size_start);
                _aim_right.fillAmount -= (aim_size_start - aim_size_end) * (Time.deltaTime / aim_time);
                _aim_right.fillAmount = Mathf.Clamp(_aim_right.fillAmount, aim_size_end, aim_size_start);

                float diap = aim_size_start - aim_size_end;
                float coef = 1 / diap;
                _aim_left.color = Color.Lerp(aim_color_end, aim_color_start, (_aim_left.fillAmount - aim_size_end) * coef );
                _aim_right.color = _aim_left.color;
            }
            else if (isAiming)
            {
                isAiming = false;
                float aimSector = _aim_left.fillAmount;
                // выстрел
                _aim_left.fillAmount = aim_size_start;
                _aim_right.fillAmount = aim_size_start;
                _aim_left.color = aim_color_start;
                _aim_right.color = aim_color_start;
                _aim_left.enabled = false;
                _aim_right.enabled = false;
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

