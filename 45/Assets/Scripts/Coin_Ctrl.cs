using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Ctrl : MonoBehaviour
{
    [HideInInspector] public HeroCtrl m_RefHero = null;

    float m_MoveSpeed = 4.0f;   //�̵��ӵ�
    float m_MagnetSpeed = 9.0f; //�ڼ� ȿ�� �ӵ�
    Vector3 m_MoveDir;       //��ġ ���� ����




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

        //## ����ó��
        if (transform.position.x < CameraResolution.m_ScWMin.x - 0.5f)
        {
            Destroy(gameObject);
        }

    }
}
