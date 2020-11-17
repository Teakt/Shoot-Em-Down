using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBoss : Entity
{

    public enum BossState
    {
        WAITING,
        ATTACKING,
        APPEARING,
    }

    public enum BossPhases
    {
        PHASE1,
        PHASE2,
        PHASE3,
    }

    private Player target;



    GameObject cube;
    float angleZ = 0;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_width = Screen.width;
    [SerializeField] private float m_height = Screen.height;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;

    [SerializeField] private float rate_of_fire;

    [SerializeField] private float shooting_power;

    [SerializeField] private GameObject ennemybullet_prefab;

    public float timeBetweenEachShot = 1f;
    [SerializeField] private float timeBetweenShotCountdown;

    float cd = 5f;
    float speed;

    public float timeBetweenEachState = 3f;
    [SerializeField] private float timeBetweenStateCountdown ;

    // Start is called before the first frame update
    [SerializeField] private BossState boss_state = BossState.APPEARING;
    [SerializeField] private BossPhases boss_phase = BossPhases.PHASE1;


    public Transform[] corners;


    void Start()
    {
        
        m_MainCamera = Camera.main;
        m_width = Screen.width;
        m_height = Screen.height;

       speed = 15f;

        timeBetweenStateCountdown = timeBetweenEachState;
        timeBetweenShotCountdown = timeBetweenEachShot;
    }

    // Update is called once per frame
    void Update()
    {

        if (FindObjectOfType<Player>() != null)
            target = FindObjectOfType<Player>();


        if (boss_state == BossState.APPEARING)
        {
            MoveToInitPos();
            
        }
        /*
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        angleZ += 10f;
        this.transform.Rotate(angleZ , 0.0f, angleZ, Space.World);
        if (screenPos.x > m_width)
            return;
        this.transform.position = new Vector3(transform.position.x + (m_HorizontalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        */

        if(boss_phase == BossPhases.PHASE1)
        {
            if (timeBetweenStateCountdown <= 0)
            {
                if (boss_state == BossState.WAITING)
                {
                    this.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(speed * Time.deltaTime, -speed * Time.deltaTime), transform.position.y, transform.position.z);

                    // Start spawning wave
                    StartCoroutine(WaitingState());
                }


                if (boss_state == BossState.ATTACKING)
                {

                    if( timeBetweenShotCountdown <= 0)
                    {
                        Corner_Shoot();
                        timeBetweenShotCountdown = timeBetweenEachShot;
                    }
                    else
                    {
                        timeBetweenShotCountdown -= Time.deltaTime;
                    }
                        
                }
            }
            else
            {
                timeBetweenStateCountdown -= Time.deltaTime;
            }
        }
    }


    void MoveToInitPos()
    {
        // Pos of the player to the camera
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.y > m_height)
        {
            //Debug.Log("IF");
            if (screenPos.x < m_width / 2)
            {
                this.transform.position = new Vector3(transform.position.x + (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(transform.position.x - (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            this.transform.position = new Vector3(transform.position.x, transform.position.y - (m_VerticalSpeed * Time.deltaTime), transform.position.z);

        }
        else
        {
            //Debug.Log("ELSE");
            if (screenPos.x < m_width/2)
            {
                //Debug.Log("ELSE1" + screenPos.x + " width / 2 : " + m_width /2);
                this.transform.position = new Vector3(transform.position.x + (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            else
            {
                //Debug.Log("ELS2E2");
                this.transform.position = new Vector3(transform.position.x - (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }


            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }

        if(screenPos.x > (m_width / 2) - 1 && screenPos.x <= (m_width / 2) + 1 && screenPos.y > m_height  - 1 && screenPos.y <= m_height + 1)
        {
            boss_state = BossState.WAITING;
        }
        
       
        
    }

    IEnumerator WaitingState()
    {
       
       


        yield return new WaitForSeconds(timeBetweenEachState);
        boss_state = BossState.ATTACKING;

        timeBetweenStateCountdown = timeBetweenEachState;

       
        
    }


    private void Corner_Shoot()
    {
        if (corners.Length == 0)
        {
            Debug.LogError("No spawn points referenced .");
        }
        for (int i = 0; i < corners.Length; i++)
        {
            //Debug.Log("Shoot : " + corners[i].name);
            GameObject instanciated_prefab = Instantiate(ennemybullet_prefab, new Vector3(corners[i].position.x, corners[i].position.y, corners[i].position.z), Quaternion.identity);
            Vector3 direction = target.transform.position - corners[i].position;
            instanciated_prefab.GetComponent<EnnemyBullet>().ShootWithDirection(direction.normalized);
        }
    }
}
