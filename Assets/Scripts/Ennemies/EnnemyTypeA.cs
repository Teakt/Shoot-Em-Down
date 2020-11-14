using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class EnnemyTypeA : Ennemy
{
   
    private Player target;


    private Camera m_MainCamera;
    
    [SerializeField] private float m_height = Screen.height;

    [SerializeField] private float delay_before_teleportation;
    [SerializeField] private float travel_distance;
    [SerializeField] private float oscillation;

    Stopwatch stopWatch = new Stopwatch(); // for the teleportation of the EnnemyA

    [SerializeField] private GameObject ennemybullet_prefab;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        stopWatch.Start(); // For the teleportation
        m_MainCamera = Camera.main;

        
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(FindObjectOfType<Player>()!=null)
            target = FindObjectOfType<Player>();
        if (target != null)
        {
            Move();
        }
    }

    public override void Move()
    {


        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);

        //System.Console.WriteLine("balle.y = ", screenPos.y, ", position max_ecran = ", m_height);
        if (screenPos.y < 0)
            Destroy(gameObject, 0f);

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        double elapsed_time = ts.TotalSeconds;

        if (elapsed_time > delay_before_teleportation)
        {
            stopWatch.Reset();
            stopWatch.Start();
            
            if (screenPos.y > 0)
                this.transform.position = new Vector3(transform.position.x, transform.position.y - (travel_distance), transform.position.z); // Goes toward down direction
                this.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range( -oscillation, oscillation), transform.position.y , transform.position.z); // Goes toward down direction
            Shoot();
            
        }
    
        else
        {
            stopWatch.Start();
        }

       

       
       
    }


    public override void Shoot()
    {
        //Shoot
        
            GameObject instanciated_prefab = Instantiate(ennemybullet_prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Vector3 direction = target.transform.position - transform.position;
            instanciated_prefab.GetComponent<EnnemyBullet>().ShootWithDirection(direction.normalized);
       
       
      
       
        
    }
}
