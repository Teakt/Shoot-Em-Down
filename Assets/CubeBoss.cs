using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBoss : MonoBehaviour
{

    GameObject cube;
    float angleZ = 0;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_width = Screen.width;
    [SerializeField] private float m_height = Screen.height;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;
    // Start is called before the first frame update


    void Start()
    {
        
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        angleZ += 10f;
        this.transform.Rotate(angleZ , 0.0f, angleZ, Space.World);
        if (screenPos.x > m_width)
            return;
        this.transform.position = new Vector3(transform.position.x + (m_HorizontalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
    }
}
