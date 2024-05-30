using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum BossState
{
    BS_appear, // ���� �� �̵�����
    BS_Normal_Att, //�⺻ ����
    BS_Fever_Att, //�ǹ� ����
}


public enum MonType
{
    MT_Zombi,
    MT_Missile,
    MT_Boss
}

public class Monster_Ctrl : MonoBehaviour
{
    public MonType m_MonType = MonType.MT_Zombi;

    //--- ���� ü�� ����
    float m_MaxHp = 200.0f;
    float m_CurHp = 200.0f;
    public Image m_HpBar = null;
    //--- ���� ü�� ����

    float m_Speed = 4.0f;   //�̵��ӵ�
    Vector3 m_CurPos;       //��ġ ���� ����
    Vector3 m_SpawnPos;     //���� ��ġ

    float m_CacPosY = 0.0f; //���� �Լ��� �� ���� ���� ���� ����
    float m_RandY   = 0.0f; //������ ������ ����� ����
    float m_CycleSpeed = 0.0f;  //������ ���� �ӵ� ����

    //## �Ѿ� �߻� ����
    public GameObject m_BulletPrefab = null;
    public GameObject m_ShootPos = null;

    float Shoot_Time = 0.0f;       //�Ѿ� �߻� �ֱ� ���� ����
    float Shoot_Delay = 1.5f;      //�Ѿ� �߻� ��Ÿ��
    float BulletSpeed = 10.0f;     //�Ѿ� �߻� �ӵ�

    //## �̻��� ai ����
    HeroCtrl m_RefHero = null;
    Vector3 m_DirVec;

    //## ���� ai ����
    BossState m_BossState = BossState.BS_appear;
    float m_ShotCount = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();    

        m_SpawnPos = transform.position;    //������ ���� ��ġ ����
        m_RandY = Random.Range(0.5f, 2.0f); //Sin �Լ��� ���� ����
        m_CycleSpeed = Random.Range(1.8f, 5.0f);    //������ ������


        if(m_MonType == MonType.MT_Boss)
        {
            m_MaxHp = 3000.0f;
            m_CurHp = m_MaxHp;
        }




    }

    // Update is called once per frame
    void Update()
    {
        if (m_MonType == MonType.MT_Zombi)
            Zombi_AI_Update();

        else if (m_MonType == MonType.MT_Missile)
            Missile_AI_Update();

        else if (m_MonType == MonType.MT_Boss)
            Boss_AI_Update();

        if (this.transform.position.x < CameraResolution.m_ScWMin.x - 2.0f)
            Destroy(gameObject);    //���� ȭ���� ����� ��� ����




    }

    void Zombi_AI_Update()
    {
        m_CurPos = transform.position;
        m_CurPos.x += (-1.0f * m_Speed * Time.deltaTime);
        m_CacPosY += (Time.deltaTime * m_CycleSpeed);
        m_CurPos.y = m_SpawnPos.y + Mathf.Sin(m_CacPosY) * m_RandY;
        transform.position = m_CurPos;

        //## �Ѿ� �߻�

        if (m_BulletPrefab == null)
            return;

        Shoot_Time += Time.deltaTime;

        if (Shoot_Time >= Shoot_Delay)
        {
            Shoot_Time = 0.0f;

            
            GameObject a_Newobj = Instantiate(m_BulletPrefab); //�Ѿ� ����
            Bullet_Ctrl a_BulletSc = a_Newobj.GetComponent<Bullet_Ctrl>();
           
            a_BulletSc.BulletSpawn(m_ShootPos.transform.position, 
                Vector3.left, BulletSpeed, 20.0f);
            

        }//if (Shoot_Time > Shoot_Delay)

    }


    void Missile_AI_Update()
    {
        //## �̵� ai
        m_CurPos = transform.position;
        m_DirVec = Vector3.left;


        //## �̻��� ���� ai
        if (m_RefHero != null)
        {
            Vector3 a_CacVec = m_RefHero.transform.position - transform.position;
            m_DirVec= a_CacVec;

            //## �̻����� ���ΰ����� �Ÿ� ����
            if(a_CacVec.x < -3.5f)
                m_DirVec.y = 0.0f;

        }

       

        m_DirVec.Normalize();

        m_DirVec.x = -1.0f;

        m_DirVec.z = 0.0f;

        m_CurPos += m_DirVec * m_Speed * Time.deltaTime;

        transform.position = m_CurPos;

        
    }

    void Boss_AI_Update()
    {
        //## ���� ����
        if(m_BossState == BossState.BS_appear)
        {
            m_CurPos = transform.position;
            //����
            float a_ArrivePos = CameraResolution.m_ScWMax.x - 1.9f;

            if(a_ArrivePos < m_CurPos.x)
            {
                m_CurPos.x += (-1.0f * m_Speed * Time.deltaTime);

                if(m_CurPos.x <= a_ArrivePos)
                {
                    Shoot_Time = 1.28f;
                    m_BossState = BossState.BS_Fever_Att;
                }

            }

            transform.position = m_CurPos;

        }
        //## ���� �Ϲ� ����
        else if(m_BossState == BossState.BS_Normal_Att)
        {
            Shoot_Time -= Time.deltaTime;
            if (Shoot_Time <= 0.0f)
            {  //## �Ϲ� ����
                Vector3 a_Target = m_RefHero.transform.position - transform.position;

                a_Target.z = 0.0f;
                a_Target.Normalize();

                GameObject a_Newobj = Instantiate(m_BulletPrefab); //�Ѿ� ����
                Bullet_Ctrl a_BulletSc = a_Newobj.GetComponent<Bullet_Ctrl>();

                a_BulletSc.BulletSpawn(m_ShootPos.transform.position,
                                   a_Target, BulletSpeed);

                //## ���ư��� �������� ȸ��
                a_Newobj.transform.right = new Vector3(-a_Target.x, -a_Target.y, 0.0f);


                m_ShotCount++;
                // �Ϲ� ���� �ֱ�
                if (m_ShotCount < 7)
                {
                    Shoot_Time = 0.7f;


                }

                else
                {
                    m_ShotCount = 0.0f;
                    Shoot_Time = 2.0f;
                    m_BossState = BossState.BS_Fever_Att;
                }




            }
        }

        //## ���� �ñر�
        else if(m_BossState == BossState.BS_Fever_Att)
        {
            Shoot_Time -= Time.deltaTime;
            //##�ñر�
            if(Shoot_Time <= 0.0f)
            {
                float Rad = 1.0f;
                Vector3 a_Target = Vector3.zero;
                GameObject a_NewObj = null;
                Bullet_Ctrl a_Bullet_Sc = null;
                //float a_CavAngle = 0.0f;
                for(float Angle = 0.0f; Angle < 360.0f; Angle += 15.0f)
                {
                    a_Target.x = Mathf.Sin(Angle * Mathf.Deg2Rad) * Rad;
                    a_Target.y = Mathf.Cos(Angle * Mathf.Deg2Rad) * Rad;
                    a_Target.Normalize();

                    a_NewObj = Instantiate(m_BulletPrefab);

                    a_Bullet_Sc = a_NewObj.GetComponent<Bullet_Ctrl>();

                    a_Bullet_Sc.BulletSpawn(transform.position, 
                        a_Target,BulletSpeed);


                    //## ���ư��� �������� ȸ��
                    a_NewObj.transform.right = new Vector3(-a_Target.x, -a_Target.y, 0.0f);
                    //a_CavAngle = Mathf.Atan2(-a_Target.y, -a_Target.x) * Mathf.Rad2Deg;
                    //a_CavAngle += 180.0f; 
                    //a_NewObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, a_CavAngle);


                }


                m_ShotCount++;
                if(m_ShotCount < 3.0f)
                {
                  Shoot_Time = 1.0f;
                }

                //##�ñر� ����
                else
                {
                    m_ShotCount = 0.0f;
                    Shoot_Time = 1.5f;
                    m_BossState = BossState.BS_Normal_Att;
                }

            }



        }



    }








    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "AllyBullet")
        {
            TakeDamage(80.0f);
            Destroy(coll.gameObject);
        }
    }//void OnTriggerEnter2D(Collider2D coll)

    public void TakeDamage(float a_Value)
    {
        if (m_CurHp <= 0.0f)  //�� ���Ͱ� �̹� �׾� ������...
            return;           //�������� ������ �ʿ� ������ ���� ��Ű�ڴٴ� ��


        float a_CacDmg = a_Value;
        if (m_CurHp < a_Value)
            a_CacDmg = m_CurHp;
       

        Game_Mgr.Inst.DamageTxt(-a_Value, transform.position, Color.magenta);//���� ������ ���



        m_CurHp -= a_Value;
        if (m_CurHp < 0.0f)
            m_CurHp = 0.0f;

        if (m_HpBar != null)
            m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        if(m_CurHp <= 0.0f)
        { //���� ��� ó��

            //�����ֱ�
            //## ��� ����
            Game_Mgr.Inst.SpawnCoin(transform.position);


            //## ���� ���
            if(m_MonType == MonType.MT_Boss)
            {
               MonsterGenerator.Inst.m_SpBsDelta = Random.Range(25.0f, 30.0f);

                Game_Mgr.Inst.SpawnHeart(transform.position, 100);
                Destroy(gameObject, 10f);

            }



            Destroy(gameObject);
        }
    }//public void TakeDamage(float a_Value)
}
