using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tory.seabatlle
{

    public class BaseShip : MonoBehaviour, IDamagable, IShip
    {
        #region variables   
        private float speedShip;
        [Range(0,10)]
        public float acceleration;
        #endregion
        #region propertes        
        public float SpeedShip
        {
            get
            {
                return speedShip;
            }
            set
            {
                speedShip = Mathf.Clamp(value, 0f, 10+ acceleration);
              
            }
        }
        #endregion
        #region field
        private GameObject _centerOfRotation;
        private Transform _transformShip;

        [SerializeField]
        [Range(0, 255)]
        private byte _health = 255;
        #endregion
        #region method
        private void Start()
        {
            _centerOfRotation = GameObject.FindGameObjectWithTag("center");           
            _transformShip = GetComponent<Transform>();
            CenterControll.Instanse._changeSpeed += ChangeSpeed;
            SpeedShip = CenterControll.Instanse._maxSpeedVaterpoll;
        }
        private void Update()
        {
            if (_centerOfRotation != null)
            {
                centerrotation();
            }
            if (acceleration != speedShip)
            {
                speedShip = acceleration;
            }
        }
        private void centerrotation()
        {
            _transformShip.RotateAround(_centerOfRotation.transform.position, Vector3.up, speedShip * Time.deltaTime+acceleration);
        }
        private void ChangeSpeed(float speed)
        {
            SpeedShip = speed;
        }

        public void TakeDamage(byte damage)
        {
            int health = _health - damage;

            if (health <= 0)
            {
                _health = 0;

                Die();
            }
            else
            {
                _health = (byte)health;
            }
        }

        private void Die()
        {
            Debug.Log("Умер");
        }
        #endregion

    }
}


