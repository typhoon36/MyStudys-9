using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHero_Ctrl : MonoBehaviour
{
    HeroCtrl m_RefHero = null;//(�÷��̾�)���� ����
    float Angle = 0.0f;
    //ȸ�� ���� ���
    float radius = 1.0f;
    //���� ������
    float m_Speed = 100.0f;
    //ȸ�� �ӵ�


    Vector3 ParentPos = Vector3.zero;
    //�θ��� ��ġ

    float m_LifeTime = 0.0f;
    //���� �ð�


    //## ���� ����
    GameObject m_BulletObj = null;
    //�Ѿ� ������Ʈ
  //  Bullet_Ctrl m_BulletSC = null;
    //�Ѿ� ��ũ��Ʈ
    float m_AttSpeed = 0.5f;
    float m_ShotCool = 0.0f;

    GameObject m_ClonObj = null;
    bool IsDouble = false;


    // Start is called before the first frame update
    void Start()
    {
        //m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();
        m_RefHero = transform.root.GetComponent<HeroCtrl>();

        m_BulletObj = Resources.Load("BulletPrefab") as GameObject;

    }

    // Update is called once per frame
    void Update()
    {
        m_LifeTime -= Time.deltaTime;

        if(m_LifeTime <= 0.0f)
        {
            Destroy(gameObject);
            return;
        }


        if(m_RefHero == null || transform.parent == null)
        {
            Destroy(gameObject);
            return;
        }

        Angle += m_Speed * Time.deltaTime;
        
        if(Angle > 360.0f)
        
            Angle -= 360.0f;
        

        ParentPos = transform.parent.position;
        transform.position = ParentPos +
            new Vector3(Mathf.Cos(Angle * Mathf.Deg2Rad) * radius, 
          Mathf.Sin(Angle * Mathf.Deg2Rad) * radius,
          0.0f);

        FireUpdate();

    }

    public void SubHeroSpawn(float  a_Angle , float a_LifeTime)
    {
        Angle = a_Angle;
        m_LifeTime = a_LifeTime;

    }

    void FireUpdate()
    {
        if(m_RefHero != null)
        {
            if(0.0f < m_RefHero.m_DoubleOnTime)
                    IsDouble = true;
            else 
                IsDouble = false;

        }

        if(0.0f < m_ShotCool)
            m_ShotCool -= Time.deltaTime;

        if(m_ShotCool <= 0.0f)
        {
            m_ShotCool = m_AttSpeed;

            //## �����϶�
            if(IsDouble == true)
            {
                Vector3 a_Pos;
                for(int i= 0; i< 2; i++)
                {
                    m_ClonObj = Instantiate(m_BulletObj);
                    a_Pos = transform.position;
                    a_Pos.y += 0.2f - (0.4f * i);
                    m_ClonObj.transform.position = a_Pos;
                }
            }
            //## ������ �ƴҶ�
            else
            {
                m_ClonObj = Instantiate(m_BulletObj);
                m_ClonObj.transform.position = transform.position;
            }




        }




    }



}
