using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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

    [Header("[Camera Settings]")]
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_width = Screen.width;
    [SerializeField] private float m_height = Screen.height;

    [Header("[Movements Settings]")]
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float MovSpeed = 10f;
    [SerializeField] private float rotateSpeed = 80f;
    [SerializeField] private float chargePower = 80f;

    [Header("[Attack Settings]")]
    [SerializeField] private float rate_of_fire;
    [SerializeField] private float shooting_power;
    public float timeBetweenEachShot = 1f;
    [SerializeField] private float timeBetweenShotCountdown;
    public float timeBetweenEachCharge = 3f;
    [SerializeField] private float timeBetweenChargeCountdown;

    [Header("[Ennemy_bullet Settings]")]
    [SerializeField] private GameObject ennemybullet_prefab;
    [SerializeField] private GameObject ennemytype2_prefab;
    [SerializeField] private bool boss_status;

    [Header("[Time Settings]")]
    public float timeBetweenEachState = 3f;
    [SerializeField] private float timeBetweenStateCountdown ;

    [Header("[Spawn Settings]")]
    public Transform[] corners;

    [Header("[Boss status Tracking]")]
    // Start is called before the first frame update
    [SerializeField] private BossState boss_state = BossState.APPEARING;
    [SerializeField] private BossPhases boss_phase = BossPhases.PHASE1;

    /*-----------------------------------------------------------------------------*/
    public delegate void OnHPChangeEvent(int hp);
    public event OnHPChangeEvent OnHPChange;
    public AudioSource Bang;
    public AudioSource State_two_Sound;
    public AudioSource Laugh;
    public AudioSource FinalState;

    bool onetime = false;

    Vector3 fixed_direction;
    private Rigidbody rb;
    private MeshRenderer mesh_renderer;
    public Material mat_boss;

    void Start()
    {
        Laugh.Play();
        rb = GetComponent<Rigidbody>();
        mesh_renderer = GetComponent<MeshRenderer>();
        m_MainCamera = Camera.main;
        m_width = Screen.width;
        m_height = Screen.height;

        timeBetweenStateCountdown = timeBetweenEachState;
        timeBetweenShotCountdown = timeBetweenEachShot;

        boss_status = true;   
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
        if(boss_phase == BossPhases.PHASE1)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;

            if (this.current_HP <= this.GetMaxHP() / 2)
            {
                State_two_Sound.Play();
                boss_phase = BossPhases.PHASE2;
                
            }

            if (timeBetweenStateCountdown <= 0)
            {
                if (boss_state == BossState.WAITING)
                {
                    MoveToInitPos();
                    // Start spawning wave
                    StartCoroutine(WaitingState());
                }
                if (boss_state == BossState.ATTACKING)
                {
                    if( timeBetweenShotCountdown <= 0)
                    {
                        Bang.Play();
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

        if(boss_phase == BossPhases.PHASE2)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            if (boss_state == BossState.WAITING)
            {
                //this.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(speed * Time.deltaTime, -speed * Time.deltaTime), transform.position.y, transform.position.z);
                MoveToInitPos();
                // Start spawning wave
                StartCoroutine(WaitingState());
            }
            if (boss_state == BossState.ATTACKING)
            {
                if (timeBetweenShotCountdown <= 0)
                {
                    Bang.Play();
                    Corner_Shoot();
                    Corner_Spawn();
                    timeBetweenShotCountdown = timeBetweenEachShot;
                }
                else
                {
                    timeBetweenShotCountdown -= Time.deltaTime;
                }
            }
            if (current_HP <= 0)  // If the player has nno HP , he dies 
            {
                FinalState.Play();
                this.setHP(GetMaxHP());
                boss_state = BossState.APPEARING;
                boss_phase = BossPhases.PHASE3;
            }
        }
        if (boss_phase == BossPhases.PHASE3)
        {
            mesh_renderer.material = mat_boss;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime, Space.Self);
            if (!onetime)
            {
                transform.localScale += new Vector3(7, 7, 7);
                onetime = true;
                boss_state = BossState.WAITING;
            }
            if (boss_state == BossState.WAITING)
            {
                MoveToInitPosBis();
                ChargeAttack();
                Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
                // Start spawning wave
                if (screenPos.x > (m_width / 2) - 1 && screenPos.x <= (m_width / 2) + 1 && screenPos.y > m_height - 0.25 * m_height - 1 && screenPos.y <= m_height - 0.25 * m_height + 1)
                {
                    boss_state = BossState.ATTACKING;
                }
            }
            if (boss_state == BossState.ATTACKING)
            {
                if (timeBetweenChargeCountdown <= 0)
                {
                    ChargeAttack();
                    Corner_Shoot();
                    timeBetweenChargeCountdown = timeBetweenEachCharge;
                }
                else
                {
                    timeBetweenChargeCountdown -= Time.deltaTime;
                }
            }
            if (current_HP  <= 0)  // If the player has nno HP , he dies 
            {
                boss_status= false;
                Destroy(this.gameObject);
                
            }
        }
    }

    void MoveToInitPos()
    {
        // Pos of the player to the camera
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        if (screenPos.y > m_height - 0.25*m_height)
        {
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
            if (screenPos.x < m_width/2)
            {
                this.transform.position = new Vector3(transform.position.x + (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(transform.position.x - (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }
        if(screenPos.x > (m_width / 2) - 1 && screenPos.x <= (m_width / 2) + 1 && screenPos.y > m_height - 0.25 * m_height - 1 && screenPos.y <= m_height - 0.25 * m_height + 1)
        {
            boss_state = BossState.WAITING;
        }
        Vector3 direction = target.transform.position - rb.position;
        fixed_direction = direction;
    }

    void MoveToInitPosBis()
    {
        // Pos of the player to the camera
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        if (screenPos.y > m_height - 0.25 * m_height)
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
            if (screenPos.x < m_width / 2)
            {
                this.transform.position = new Vector3(transform.position.x + (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(transform.position.x - (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }
        if (screenPos.x > (m_width / 2) - 1 && screenPos.x <= (m_width / 2) + 1 && screenPos.y > m_height - 0.25 * m_height - 1 && screenPos.y <= m_height - 0.25 * m_height + 1)
        {
            boss_state = BossState.ATTACKING;
        }
        Vector3 direction = target.transform.position - rb.position;
        fixed_direction = direction;
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
        }
        boss_state = BossState.WAITING;
    }

    private void Corner_Spawn()
    {
        if (corners.Length == 0)
        {
            Debug.LogError("No spawn points referenced .");
        }
        for (int i = 0; i < corners.Length; i++)
        {
            //Debug.Log("Shoot : " + corners[i].name);
            GameObject instanciated_prefab = Instantiate(ennemytype2_prefab, new Vector3(corners[i].position.x, corners[i].position.y, 0), Quaternion.identity);
        }
        boss_state = BossState.WAITING;
    }

    IEnumerator  MoveRandom()
    {
        int rand = (int)Random.Range(0, 2);
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        yield return new WaitForSeconds(1f);
        if (screenPos.x < m_width  && screenPos.x > 0 )
        {
            if (rand == 0)
                GoLeft();
            else
                GoRight();
        }
    }

    private void ChargeAttack()
    {
        rotateSpeed = 270f;
        //Taunt 
        Vector3 direction = target.transform.position - this.transform.position;
        this.transform.LookAt(target.transform);
        rb.AddForce(fixed_direction * chargePower * 2 * Time.deltaTime);
        if (timeBetweenChargeCountdown <= 0)
        {
            boss_state = BossState.WAITING;
            timeBetweenChargeCountdown = timeBetweenEachCharge;
        }
        else
        {
            timeBetweenChargeCountdown -= Time.deltaTime;
        }
    }

    private void GoRight()
    {
        this.transform.position = new Vector3(transform.position.x + (MovSpeed * Time.deltaTime), transform.position.y, transform.position.z);
    }
    private void GoLeft()
    {
        this.transform.position = new Vector3(transform.position.x - (MovSpeed * Time.deltaTime), transform.position.y, transform.position.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.rigidbody.tag == "Player") // the ennemy crashes if he collides with player
        {    
        }

        if (collision.rigidbody.tag == "Bullet") // the ennemy crashes if he collides with player
        {
            if (OnHPChange != null)
            {
                OnHPChange(1);
            }
            this.loseHP(1);
            
            //Debug.Log("Ennemy HP : " + current_HP);
        }

        if (collision.rigidbody.tag == "Bullet_type2") // the ennemy crashes if he collides with player
        {
            if (OnHPChange != null)
            {
                OnHPChange(5);
            }
            this.loseHP(5);

        }
    }

    public int GetHP() // Egalement, il sera nécessaire de créer une propriété public qui permet de récupérer la valeur max des PVs du vaisseau. 
    {
        return current_HP;
    }

    public bool GetStatus(){
        return boss_status;
    }
}
