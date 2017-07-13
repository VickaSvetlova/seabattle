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
    }
    #endregion

    public CannonItem[] cannons;
    public AmmoItem[] ammos;

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
        if (!cannons[cid].isTrackable)
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

    public void EndAiming()
    {
        switch (cannons[cid].aimType)
        {
            case AimType.Cannon:
            {
                aim_left.enabled = false;
                aim_right.enabled = false;
                break;
            }
            case AimType.Mortar:
            {
                aim_mortar.enabled = false;
                break;
            }
        }
        DamnShootEm();
    }

    public void DamnShootEm()
    {

    }
}
