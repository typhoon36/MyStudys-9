using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrl : MonoBehaviour
{
    //--- ���ΰ� ü�� ����
    float m_MaxHp = 200.0f;
    [HideInInspector] public float m_CurHp = 200.0f;
    public Image m_HpBar = null;
    //--- ���ΰ� ü�� ����

    //--- Ű���� �Է°� ���� ����
    float h = 0.0f;
    float v = 0.0f;

    float moveSpeed = 7.0f;
    Vector3 moveDir = Vector3.zero;
    //--- Ű���� �Է°� ���� ����

    //--- ���ΰ� ȭ�� ������ ���� �� ������ ���� ���� ����
    Vector3 HalfSize = Vector3.zero;
    Vector3 m_CacCurPos = Vector3.zero;
    //--- ���ΰ� ȭ�� ������ ���� �� ������ ���� ���� ����

    //--- �Ѿ� �߻� ����
    public GameObject m_BulletPrefab = null;
    public GameObject m_ShootPos = null;
    float m_ShootCool = 0.0f;       //�Ѿ� �߻� �ֱ� ���� ����
    //--- �Ѿ� �߻� ����

    //--- Wolf ��ų
    public GameObject m_WolfPrefab = null;
    //--- Wolf ��ų

    //--- ���� ��ų
    float m_SdOnTime = 0.0f;
    float m_SdDuration = 12.0f; //12�� ���� �ߵ�
    public GameObject ShieldObj = null;
    //--- ���� ��ų

    //--- ����ź ��ų
    public GameObject m_HomingMs = null;
    //--- ����ź ��ų

    // ##  ���� 
    [HideInInspector] public float m_DoubleOnTime = 0.0f;
    float m_DoubleDur = 12.0f; //12�� ���� �ߵ�


    //## ���� ����� 
    int SubHCount = 0;
    float m_SubHTime = 0.0f;
    float m_SubHInterval = 12.0f;
    public GameObject Sub_Parent = null;
    public GameObject SubHeroPrefab = null;



    // Start is called before the first frame update
    void Start()
    {
        //--- ĳ������ ���� �ݻ�����, ���� �ݻ����� ���ϱ�
        //���忡 �׷��� ��������Ʈ ������ ������
        SpriteRenderer sprRend = gameObject.GetComponentInChildren<SpriteRenderer>();
        //sprRend.bounds.size.x   ��������Ʈ�� ���� ������
        //sprRend.bounds.size.y   ��������Ʈ�� ���� ������
        // Debug.Log(sprRend.bounds.size);
        // (1.26, 1.58, 0.20)
        HalfSize.x = sprRend.bounds.size.x / 2.0f - 0.23f; //ĳ������ ���� �� ������(������ Ŀ�� ���� ����)
        HalfSize.y = sprRend.bounds.size.y / 2.0f - 0.05f; //ĳ������ ���� �� ������
        HalfSize.z = 1.0f;
        //���忡 �׷��� ��������Ʈ ������ ������
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0.0f || v != 0.0f)
        {
            moveDir = new Vector3(h, v, 0.0f);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }//if(h != 0.0f || v != 0.0f)

        LimitMove();

        FireUpdate();

        Update_Skill();
    }//void Update()

    void LimitMove()
    {
        m_CacCurPos = transform.position;

        if (m_CacCurPos.x < CameraResolution.m_ScWMin.x + HalfSize.x)
            m_CacCurPos.x = CameraResolution.m_ScWMin.x + HalfSize.x;

        if (CameraResolution.m_ScWMax.x - HalfSize.x < m_CacCurPos.x)
            m_CacCurPos.x = CameraResolution.m_ScWMax.x - HalfSize.x;

        if (m_CacCurPos.y < CameraResolution.m_ScWMin.y + HalfSize.y)
            m_CacCurPos.y = CameraResolution.m_ScWMin.y + HalfSize.y;

        if (CameraResolution.m_ScWMax.y - HalfSize.y < m_CacCurPos.y)
            m_CacCurPos.y = CameraResolution.m_ScWMax.y - HalfSize.y;

        transform.position = m_CacCurPos;

    }

    void FireUpdate()
    {
        if (0.0f < m_ShootCool)
            m_ShootCool -= Time.deltaTime;

        if (m_ShootCool <= 0.0f)
        {
            m_ShootCool = 0.15f;

            //## �����Ͻ�
            if (0.0f < m_DoubleOnTime)
            {
                Vector3 a_Pos;
                GameObject a_CloneObj;
                for(int i= 0; i < 2; i++)
                {
                    a_CloneObj = Instantiate(m_BulletPrefab);
                    a_Pos = m_ShootPos.transform.position;
                    a_Pos.y += 0.2f - (i * 0.4f);
                    a_CloneObj.transform.position = a_Pos;


                }

            }



            //## �Ϲ��Ѿ��Ͻ�
            else
            {
                GameObject a_CloneObj = Instantiate(m_BulletPrefab);
                a_CloneObj.transform.position = m_ShootPos.transform.position;
            }


        }
    }// void FireUpdate()

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Monster")
        {
            Monster_Ctrl a_RefMon = coll.gameObject.GetComponent<Monster_Ctrl>();
            if (a_RefMon != null)
                a_RefMon.TakeDamage(1000);
            
            TakeDamage(50.0f);

            Game_Mgr.Inst.DecreaseGold(10);
        }
        else if (coll.tag == "EnemyBullet")
        {
            TakeDamage(20.0f);
            Game_Mgr.Inst.DecreaseGold(10);
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.name.Contains("CoinPrefab") == true)
        {

            Game_Mgr.Inst.AddGold(99);

            Destroy(coll.gameObject);
        }//else if(coll.gameObject.name.Contains("CoinPrefab") == true)
        else if (coll.gameObject.name.Contains("HeartPrefab") == true)
        {
            m_CurHp += m_MaxHp * 0.5f;
            Game_Mgr.Inst.DamageText(m_MaxHp * 0.5f,
                            transform.position, new Color(0.18f, 0.5f, 0.34f));

            if (m_MaxHp < m_CurHp)
                m_CurHp = m_MaxHp;

            if (m_HpBar != null)
                m_HpBar.fillAmount = m_CurHp / m_MaxHp;

            Destroy(coll.gameObject);
        }//else if(coll.gameObject.name.Contains("HeartPrefab") == true)

    }//void OnTriggerEnter2D(Collider2D coll)

    void TakeDamage(float a_Value)
    {
        if (m_CurHp <= 0.0f)
            return;

        if (0.0f < m_SdOnTime)  //���� ��ų �ߵ� �� �� �� ... ������ ��ŵ
            return;

        Game_Mgr.Inst.DamageText(-a_Value, transform.position, Color.blue);

        m_CurHp -= a_Value;
        if (m_CurHp < 0.0f)
            m_CurHp = 0.0f;

        if (m_HpBar != null)
            m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        if (m_CurHp <= 0.0f)
        {  //���ó��
            Time.timeScale = 0.0f;  //�Ͻ�����
        }
    }

    void Update_Skill()
    {
        //--- ���� ���� ������Ʈ
        if (0.0f < m_SdOnTime)
        {
            m_SdOnTime -= Time.deltaTime;
            if (ShieldObj != null && ShieldObj.activeSelf == false)
                ShieldObj.SetActive(true);
        }
        else
        {
            if (ShieldObj != null && ShieldObj.activeSelf == true)
                ShieldObj.SetActive(false);
        }
        //--- ���� ���� ������Ʈ

        //## ���� ������Ʈ
        if (0.0f < m_DoubleOnTime)
        {
            m_DoubleOnTime -= Time.deltaTime;

            if (m_DoubleOnTime <= 0.0f)
                m_DoubleOnTime = 0.0f;


        }

        //## ���� ����� ������Ʈ    

        if (0.0f < m_SubHTime)
        {
            m_SubHTime -= Time.deltaTime;

            if (m_SubHTime <= 0.0f)
                m_SubHTime = 0.0f;
        }




    }

    public void UseSkill(SkillType a_SkType)
    {
        if (m_CurHp <= 0.0f) //���ΰ� ����� ��ų �ߵ� ����
            return;

        if (a_SkType == SkillType.Skill_0) //Hp 20% ����
        {
            m_CurHp += m_MaxHp * 0.2f;
            Game_Mgr.Inst.DamageText(m_MaxHp * 0.2f, transform.position,
                                            new Color(0.18f, 0.5f, 0.34f));

            if (m_MaxHp < m_CurHp)
                m_CurHp = m_MaxHp;

            if (m_HpBar != null)
                m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        }//if(a_SkType == SkillType.Skill_0) //Hp 20% ����
        else if (a_SkType == SkillType.Skill_1) //������ų
        {
            GameObject a_Clone = Instantiate(m_WolfPrefab);
            a_Clone.transform.position =
                    new Vector3(CameraResolution.m_ScWMin.x - 1.0f, 0.0f, 0.0f);
        }
        else if (a_SkType == SkillType.Skill_2)  //��ȣ��
        {
            if (0.0f < m_SdOnTime)
                return;

            m_SdOnTime = m_SdDuration;

            //UI ��Ÿ�� �ߵ�
            Game_Mgr.Inst.SkillCoolMethod(a_SkType, m_SdOnTime, m_SdDuration);
        }
        else if (a_SkType == SkillType.Skill_3) //����ź
        {
            //--- 4�� �߻�
            Vector3 a_Pos;
            GameObject a_CloneObj;
            for (float yy = 0.8f; yy > -0.9f; yy -= 0.4f)  //5�� �ݺ���
            {
                if (-0.1f < yy && yy < 0.1f)
                    continue;

                a_Pos = Vector3.zero;
                if (-0.7f < yy && yy < 0.7f)
                {
                    a_Pos.x = 0.4f;
                }
                else
                {
                    a_Pos.x = -0.4f;
                }
                a_Pos.y = yy;
                a_Pos = this.transform.position + a_Pos;

                a_CloneObj = Instantiate(m_HomingMs);
                a_CloneObj.transform.position = a_Pos;
            }// for(float yy = 0.8f; yy > -0.9f; yy -= 0.4f)  //5�� �ݺ���
            //--- 4�� �߻�
        }


        //## ���� ��ų
        else if (a_SkType == SkillType.Skill_4)
        {
            if (0.0f < m_DoubleOnTime)
                return;

            m_DoubleOnTime = m_DoubleDur;

            //UI ��Ÿ�� �ߵ�
            Game_Mgr.Inst.SkillCoolMethod(a_SkType, m_DoubleOnTime, m_DoubleDur);
        }



        //## ��ȯ�� ��ų
        else if(a_SkType == SkillType.Skill_5)
        {
            if(0.0f < m_SubHTime)
            {
                return;
            }
            SubHCount = 3;

            m_SubHTime = m_SubHInterval;

            for(int i = 0; i < SubHCount; i++)
            {

                GameObject Obj = Instantiate(SubHeroPrefab);
                Obj.transform.SetParent(Sub_Parent.transform);
                SubHero_Ctrl sub = Obj.GetComponent<SubHero_Ctrl>();
                if(sub != null)
                
                    sub.SubHeroSpawn((360/ SubHCount ) * i, m_SubHTime);
                
            }




            //UI ��Ÿ�� �ߵ�
            Game_Mgr.Inst.SkillCoolMethod(a_SkType, m_SubHTime, m_SubHInterval);

        }

        GlobalValue.g_CurSkillCount[(int)a_SkType]--;

        //## ���� ����
        PlayerPrefs.SetInt($"Skill_Item_{(int)a_SkType}", 
            GlobalValue.g_CurSkillCount[(int)a_SkType]);



    }//public void UseSkill(SkillType a_SkType)
}
