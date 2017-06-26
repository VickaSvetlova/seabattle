using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    #region events

    #endregion

    #region propertes           
    #endregion

    #region Field
    private bool _isBulletReady = true;
    [SerializeField]
    private GameObject _pointExplosion;
    [SerializeField]
    private SpriteRenderer _flareCannon;
    [SerializeField]
    [Range(1f, 24f)]
    private float _shotDistance = 1f;
    private RaycastHit _target;
    private Color _color = Color.green;

    [SerializeField]
    [Range(1, 255)]
    private byte _damage = 1;

    [SerializeField]
    [Range(1f, 5f)]
    private float _reloadTime = 1f;
    [SerializeField]

    [Range(0.2f,5f)]
    private float _timeToFlyBoolet;
    private float _timeLifeFlareCanno=0.05f;
    #endregion

    #region method
    private void Start()
    {
        _flareCannon = this.GetComponentInChildren<SpriteRenderer>();
    }
    private void Shot()
    {
        if (!_isBulletReady)
        {
            return;
        }
        if (Aim())
        {

            //_target.TakeDamage(_damage);
            _color = Color.red;
            _isBulletReady = false;           
            StartCoroutine(TimeToFlayRoutine(_target.collider.GetComponent<Transform>().position));
            _flareCannon.color = new Color(255, 255, 255,255);
            StartCoroutine(LifeFlareCannon());
            StartCoroutine(ReloadBulletRoutine());            
        }
    }

    private bool Aim()
    {
        Ray cannonRay = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(cannonRay, out hitInfo, _shotDistance);
        Debug.DrawRay(transform.position, transform.forward * _shotDistance, _color);

        if (hasHit)
        {
            if (hitInfo.collider.GetComponentInParent<IShip>() != null)
            {
                _target = hitInfo;
            }
        }
        return hasHit;
        
    }
    private IEnumerator ReloadBulletRoutine()
    {
        yield return new WaitForSeconds(_reloadTime);
        _color = Color.green;
        _isBulletReady = true;
    }
    private IEnumerator TimeToFlayRoutine(Vector3 _spotExplosion)
    {
      yield return new WaitForSeconds(_timeToFlyBoolet+Random.Range(0.1f,1));
      Destroy(Instantiate(_pointExplosion, _spotExplosion, Quaternion.identity), 1.15f);
    }
    private IEnumerator LifeFlareCannon()
    {
        yield return new WaitForSeconds(_timeLifeFlareCanno);
        _flareCannon.color = new Color(0, 0, 0, 0);
    }

    private void LateUpdate()
    {
        if (_isBulletReady)
        {
            Aim();
            Shot();
        }
    }
    #endregion
}
