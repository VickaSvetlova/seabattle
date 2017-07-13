using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class CannonItem
{
    [Tooltip("Имя пушки")]
    public string name = "default_cannon";
    [Tooltip("Время перезарядки")]
    public float reloadTime = 1f;
    [Tooltip("Время переключения на эту пушку")]
    public float switchTime = 2f;
    [Tooltip("Время сведения")]
    public float aimTime = 1f;
    [Tooltip("Есть ли сведение")]
    public bool canAim = true;
    [Tooltip("Максимальный прицел")]
    public float maxAim = 0.1f;
    [Tooltip("Минимальный прицел")]
    public float minAim = 0.025f;
    [Tooltip("Выстрел в секторе или точно в цель")]
    public bool randomAim = false;
    [Tooltip("Тип снаряда из списка AmmoData")]
    public int ammoType = -1;
    [Tooltip("Наличие максимального угла отклонения")]
    public bool hasMaxAngle = true;
    [Tooltip("Максимальный угол отклонения от борта")]
    public float maxAngle = 45f;
    [Tooltip("Трекает ли прицел")]
    public bool isTrackable = true;
    [Tooltip("Тип прицела")]
    public AimType aimType = AimType.Cannon;
    [Tooltip("Анимация полета")]
    public ShootAnimation anima = ShootAnimation.Linear;
//    [Tooltip("")]
//    [Tooltip("")]
}

[System.Serializable]
public class AmmoItem
{
    [Tooltip("Название снаряда")]
    public string name = "defaul_ammo";
    [Tooltip("Дамаг снаряда")]
    public float damage = 1f;
    [Tooltip("Процентовка дамага")]
    public DamagePercentage target;
    [Tooltip("Шанс крита")]
    public float critChance = 10f;
    [Tooltip("Множитель крита")]
    public float critMultiplier = 3f;
    [Tooltip("Шанс промаха")]
    public float missChance = 15f;
    [Tooltip("Скорость полёта")]
    public float velocity = 5f;
}

[System.Serializable]
public class DamagePercentage
{
    public float body = 80f;
    public float team = 10f;
    public float control = 10f;
}

public enum AimType
{
    Cannon = 0,
    Mortar = 1
}

public enum ShootAnimation
{
    Linear = 0,
    Ballistic = 1,
    Momental = 2
}

public class WeaponData : MonoBehaviour {

    #region Singleton-style
    public static WeaponData Instance { private set; get; }
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Instance = this;
        aim_left.enabled = false;
        aim_right.enabled = false;
        aim_mortar.enabled = false;
    }
    #endregion
    public bool debug_aim = false;

    public CannonItem[] cannons;
    public AmmoItem[] ammos;

    [Header("Разное")]
    public GameObject bullet;

    [Header("Прицел - общее")]
    [Tooltip("Цвет прицела в начале")]
    public Color aim_color_start;
    [Tooltip("Цвет прицела в конце")]
    public Color aim_color_end;

    [Header("Прицел - тип CANNON")]
    [Tooltip("Трансформ прицела пушки")]
    public Transform tr_aim;
    public Transform tr_canvas_aim;
    [Tooltip("Спрайт левой части прицела пушки")]
    public Image aim_left;
    [Tooltip("Спрайт правой части прицела пушки")]
    public Image aim_right;

    [Header("Прицел - тип MORTAR")]
    [Tooltip("Трансформ прицела")]
    public Transform tr_mortar_aim;
    [Tooltip("Картинка прицела")]
    public Image aim_mortar;
    [Tooltip("Поднятие над точкой прицела")]
    public float mortar_aim_overhead = 0.01f;
    [Tooltip("Вращение прицела при сведении")]
    public bool canRotate = true;
    [Tooltip("Скорость вращения прицела")]
    public float rotateSpeed = 0.5f;

    private int cid;
    private Vector3 aim_point;
    private float timer;

    public void StartAiming(int _cID, Vector3 _pos)
    {
        cid = _cID;
        timer = 0f;
        switch (cannons[cid].aimType)
        {
            case AimType.Cannon:
            {
                aim_left.enabled = true;
                aim_right.enabled = true;
                aim_left.fillAmount = cannons[cid].maxAim;
                aim_right.fillAmount = aim_left.fillAmount;
                _pos.y = tr_canvas_aim.position.y;
                tr_canvas_aim.LookAt(_pos);
                aim_point = _pos;
                break;
            }
            case AimType.Mortar:
            {
                tr_mortar_aim.position = _pos + new Vector3(0f, mortar_aim_overhead, 0f);
                aim_mortar.enabled = true;
                tr_mortar_aim.localScale = Vector3.one * cannons[cid].maxAim;
                break;
            }
        }
        if (!cannons[cid].hasMaxAngle)
        {
            aim_left.color = aim_color_end;
            aim_right.color = aim_left.color;
        }
    }

    public void ProcessAiming(Vector3 _pos)
    {
        timer = Mathf.Clamp(timer + Time.deltaTime, 0f, cannons[cid].aimTime);
        
        switch (cannons[cid].aimType)
        {
            case AimType.Cannon:
            {
                if (cannons[cid].hasMaxAngle)
                {
                    aim_left.fillAmount = Mathf.Lerp(cannons[cid].maxAim, cannons[cid].minAim, (timer / cannons[cid].aimTime));
                    aim_right.fillAmount = aim_left.fillAmount;
                }
                _pos.y = tr_canvas_aim.position.y;
                if (cannons[cid].isTrackable)
                {
                    tr_canvas_aim.LookAt(_pos);
                }
                else
                {
                    tr_canvas_aim.LookAt(aim_point);
                }
                if (cannons[cid].hasMaxAngle)
                {
                    aim_left.color = Color.Lerp(aim_color_end, aim_color_start,
                        (aim_left.fillAmount - cannons[cid].minAim) / (cannons[cid].maxAim - cannons[cid].minAim));
                    aim_right.color = aim_left.color;
                }
                break;
            }
            case AimType.Mortar:
            {
                if (cannons[cid].hasMaxAngle)
                {
                    if (cannons[cid].hasMaxAngle)
                    {
                        tr_mortar_aim.localScale = Mathf.Lerp(cannons[cid].maxAim, cannons[cid].minAim, (timer / cannons[cid].aimTime)) * Vector3.one;
                        aim_mortar.color = Color.Lerp(aim_color_start, aim_color_end, timer / cannons[cid].aimTime);
                    }
                }
                if (cannons[cid].isTrackable)
                    {
                        tr_mortar_aim.position = _pos + new Vector3(0f, mortar_aim_overhead, 0f);
                    }
                if (canRotate)
                    {
                        tr_mortar_aim.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
                    }
                break;
            }
        }
    }

    public void EndAiming(Vector3 _pos)
    {
        float aim_value = 0f;
        switch (cannons[cid].aimType)
        {
            case AimType.Cannon:
            {
                aim_left.enabled = debug_aim;
                aim_right.enabled = debug_aim;
                aim_value = aim_right.fillAmount * 360f;
                break;
            }
            case AimType.Mortar:
            {
                aim_mortar.enabled = debug_aim;
                aim_value = tr_mortar_aim.localScale.x;
                break;
            }
        }
        DamnShootEm(_pos, aim_value);
    }

    public void DamnShootEm(Vector3 _pos, float _aim_value)
    {
        Vector3 cannon = Whirpool.Instance.ship1.tr_cannon.position;
        Vector3 cannon_dir = Whirpool.Instance.ship1.tr_cannon.forward;

        switch (cannons[cid].anima)
        {
            case ShootAnimation.Linear:
                {
                    GameObject go = (GameObject)Instantiate(bullet, cannon, Quaternion.identity);
                    go.transform.position = Whirpool.Instance.ship1.tr_cannon.position;
                    Vector3 dir = _pos - Whirpool.Instance.ship1.tr_cannon.position;
                    dir.y = 0f;
                    dir.Normalize();
                    go.transform.forward = dir;
                    go.transform.Rotate(Vector3.up, Random.Range(-_aim_value/2f, _aim_value), Space.World);
                    go.GetComponent<Rigidbody>().velocity = go.transform.forward * ammos[cannons[cid].ammoType].velocity;
                    go.GetComponent<Bullet>().ammoID = cannons[cid].ammoType;
                    go.GetComponent<Bullet>().parent = Whirpool.Instance.ship1.tr_ship;
                    Whirpool.Instance.ship1.ReloadCannon();
                    break;
                }
            case ShootAnimation.Ballistic:
                {
                    Whirpool.Instance.ship1.ReloadCannon();
                    break;
                }
            case ShootAnimation.Momental:
                {
                    Whirpool.Instance.ship2.Damage(cannons[cid].ammoType);
                    Whirpool.Instance.ship1.ReloadCannon();
                    break;
                }
        }
    }
}
