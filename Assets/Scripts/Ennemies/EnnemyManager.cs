using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnnemyManager : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING,
        BOSS,
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform ennemy;
        public int count;
        public float rate;  
    }

    [Header("[Waves Settings]")]
    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    [SerializeField] private float waveCountdown;

    [Header("[Spawn Settings]")]
    public Transform[] spawnPoints;
    [SerializeField] private SpawnState state = SpawnState.COUNTING;

    private float SearchCountdown = 1f;
    /*--------------------*/
    [Header("[Ennemy Prefabs Settings]")]
    public GameObject ennemy_prefab;
    public GameObject ennemy_typeB;
    [SerializeField] private CubeBoss cube_boss;

    private Camera m_MainCamera;
    // Start is called before the first frame update
    void Awake()
    {
        
        m_MainCamera = Camera.main;
    }
    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced .");
        }
        waveCountdown = timeBetweenWaves; 
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnnemyIsAlive())
            {
                // Begin a new round 
                WaveCompleted();
            }
            else
            {
                return;
            }
        }


        if (state != SpawnState.BOSS)
        {
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    // Start spawning wave
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
    }
       

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1 )
        {
            nextWave = 0;
            Debug.Log("All waves compelted! BOSS INCOMING");
            //Spawn boss ? 
            state = SpawnState.BOSS;
            cube_boss.gameObject.SetActive(true);
        }
        else
        {
            nextWave++;
        } 
    }

    bool EnnemyIsAlive()
    {

        SearchCountdown -= Time.deltaTime; 
        if(SearchCountdown <= 0)
        {
            SearchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Ennemy") == null) // if there is any ennemy alive in the scene
            {   
                return false;
            }
        }
        return true; 
    }

    private IEnumerator SpawnWave(Wave _wave)
    {

        Debug.Log("Spawn wave : " + _wave.name);
        state = SpawnState.SPAWNING;

        //Spawn 
        state = SpawnState.WAITING; 

        for(int i = 0; i< _wave.count ; i ++)
        {
            SpawnEnnemy(_wave.ennemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        yield break;  
    }

    void SpawnEnnemy(Transform _ennemy)
    {
       
        UnityEngine.Debug.Log("spawn ennemy" + _ennemy.name);

        //Spawn ennemy 
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced .");
        }
        Transform _sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; 
        Instantiate(_ennemy, _sp.position, _sp.rotation);
    }

    public SpawnState GetSpawnState()
    {
        return state;
    }
}
