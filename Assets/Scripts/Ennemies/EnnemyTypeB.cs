using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class EnnemyTypeB : Ennemy
{
    [Header("[Shooting Settings]")]
    [SerializeField] private float shotgun_spread_angle;
    [SerializeField] private float delay_before_shooting;
    [SerializeField] private float shoot_zone_distance;

    [Header("[Movements Settings]")]
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float movementSpeed;

    [Header("[Prefabs Settings]")]
    [SerializeField] private GameObject ennemybullet_prefab;

    [Header("[Screen Settings]")]
    private Camera m_MainCamera;
    [SerializeField] private float m_verticalSpeed;
    [SerializeField] private float m_height = Screen.height;

    private Player target;

    Stopwatch stopWatch = new Stopwatch(); // for the teleportation of the EnnemyA

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        m_MainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Player>() != null)
            target = FindObjectOfType<Player>();
        if (target != null)
        {
            distanceToPlayer = Vector3.Distance(target.transform.position, transform.position); // distance between target and this ennemy
            if(distanceToPlayer > shoot_zone_distance)
            {
                Move();
            }
            else
            {
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                double elapsed_time = ts.TotalSeconds;

                if (elapsed_time > delay_before_shooting)
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    Shoot();
                }
                else
                {
                    stopWatch.Start();
                }
            }   
        }
    }

    public override void Move()
    {
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);
        Vector3 directionToTarget = target.transform.position - transform.position; 
        if (screenPos.y < 0)
            Destroy(gameObject, 0f);
        if (screenPos.y > 0)
            this.transform.position = new Vector3(transform.position.x + (directionToTarget.x * Time.deltaTime ), transform.position.y + (directionToTarget.y * Time.deltaTime), transform.position.z);
    }

    public override void Shoot()
    {
        GameObject instanciated_prefab = Instantiate(ennemybullet_prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Vector3 direction = target.transform.position - transform.position;
        instanciated_prefab.GetComponent<EnnemyBullet>().ShootWithDirection(direction.normalized);

        //Shotgun pattern
        GameObject instanciated_prefab1 = Instantiate(ennemybullet_prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Vector3 direction1 = target.transform.position - transform.position;
        direction1 = Quaternion.AngleAxis(-shotgun_spread_angle, new Vector3(0, 0, 1)) * direction1;
        direction1.z = 0f; 
        instanciated_prefab1.GetComponent<EnnemyBullet>().ShootWithDirection(direction1.normalized);

        
        GameObject instanciated_prefab2 = Instantiate(ennemybullet_prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Vector3 direction2 = target.transform.position - transform.position;
        direction2 = Quaternion.AngleAxis(shotgun_spread_angle, new Vector3(0, 0, 1)) * direction2;
        direction2.z = 0f;
        instanciated_prefab2.GetComponent<EnnemyBullet>().ShootWithDirection(direction2.normalized);

        GameObject instanciated_prefab3 = Instantiate(ennemybullet_prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Vector3 direction3 = target.transform.position - transform.position;
        direction3 = Quaternion.AngleAxis(-shotgun_spread_angle -shotgun_spread_angle/2f, new Vector3(0, 0, 1)) * direction3;
        direction3.z = 0f;
        instanciated_prefab3.GetComponent<EnnemyBullet>().ShootWithDirection(direction3.normalized);

        GameObject instanciated_prefab4 = Instantiate(ennemybullet_prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Vector3 direction4 = target.transform.position - transform.position;
        direction4 = Quaternion.AngleAxis(shotgun_spread_angle - shotgun_spread_angle/2f, new Vector3(0, 0, 1)) * direction4;
        direction4.z = 0f;
        instanciated_prefab4.GetComponent<EnnemyBullet>().ShootWithDirection(direction4.normalized);
    }
}
