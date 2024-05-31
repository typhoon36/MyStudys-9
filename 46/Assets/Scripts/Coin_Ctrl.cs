using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Ctrl : MonoBehaviour
{
    [HideInInspector] public HeroCtrl m_RefHero = null;

    float m_MoveSpeed = 4.0f;   //이동속도
    float m_MagnetSpeed = 9.0f; //자석 효과 속도
    Vector3 m_MoveDir;       //위치 계산용 변수




    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {

        bool isMagnet = false;

        if(m_RefHero != null)
        {
            m_MoveDir = m_RefHero.transform.position - transform.position;
            m_MoveDir.z = 0.0f;
            if(m_MoveDir.magnitude < 2.0f)
            {
                m_MoveDir.Normalize();
                transform.position += m_MoveDir * m_MagnetSpeed * Time.deltaTime;
                isMagnet = true;
            }
        }

        if(isMagnet == false)
        transform.position += Vector3.left * m_MoveSpeed * Time.deltaTime;

        //## 예외처리
        if (transform.position.x < CameraResolution.m_ScWMin.x - 0.5f)
        {
            Destroy(gameObject);
        }

    }
}
