using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class EnnemyManager : MonoBehaviour
{



    [SerializeField] private float appearance_delay;

    public GameObject ennemy_prefab;

    [SerializeField] private Camera m_MainCamera;
    // Start is called before the first frame update

    void Awake()
    {
        
        m_MainCamera = Camera.main;
    }
    void Start()
    {
        if (Application.isPlaying)
        {
            IEnumerator coroutine = WaitAndSpawnEnnemy(appearance_delay);
            StartCoroutine(coroutine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // every 2 seconds perform the print()
    private IEnumerator WaitAndSpawnEnnemy(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Vector3 screenTopSide = m_MainCamera.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, m_MainCamera.pixelWidth), m_MainCamera.pixelHeight, -m_MainCamera.transform.position.z));
            Instantiate(ennemy_prefab,screenTopSide, Quaternion.identity);
        }
    }
}
