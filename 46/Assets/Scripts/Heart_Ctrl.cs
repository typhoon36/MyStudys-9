using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Ctrl : MonoBehaviour
{
    //## 날아갈 방향 변수
    float m_DirVecX = 1.0f;
    float m_DirVecY = 1.0f;

    //## 날아갈 속도 변수
    Vector3 m_DirVec;
    float m_Speed = 9.9f;

    // Start is called before the first frame update
    void Start()
    {
        m_DirVec = new Vector3(m_DirVecX, m_DirVecY, 0.0f);
        Destroy(gameObject, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < CameraResolution.m_ScWMin.x + 0.5f || 
            CameraResolution.m_ScWMax.x -0.5f < transform.position.x)
        {
            m_DirVecX = -m_DirVecX;

        }
        if(transform.position.y < CameraResolution.m_ScWMin.y + 0.5f || 
                       CameraResolution.m_ScWMax.y -0.5f < transform.position.y)
        {
            m_DirVecY = -m_DirVecY;
        }

       // m_DirVec = new Vector2(m_DirVecX, m_DirVecY);


        m_DirVec.x = m_DirVecX;
        m_DirVec.y = m_DirVecY;
        m_DirVec.z = 0.0f;
        m_DirVec.Normalize();


        transform.position += (m_DirVec * m_Speed * Time.deltaTime);
    }
}
