using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrl : MonoBehaviour
{
    //--- ���ΰ� ü�� ����
    float m_MaxHp = 100.0f;
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

    //## �ñر� �Ѿ� �߻� ����
    public GameObject m_WolfPrefab = null;


    //## �� ����

    float m_SdOnTime = 0.0f;
    float m_SdDuration = 12.0f;
    public GameObject m_ShieldObj = null;

    //## ���� �̻��� ����
    public GameObject m_HormingMs = null;


    //## ���� �� ����
    public GameObject m_DoubleShot = null;

    float m_CoolTime = 0.0f;
    float m_Dur = 12.0f;


    bool IsDoubleShot = false;

    //## ��ȯ�� ����
    public GameObject summonPrefab = null;
    float m_SummonCool = 0.0f;
    float m_SummonCoolTime = 12.0f;

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
            if (!IsDoubleShot)
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
            //if (a_RefMon != null)
            //    a_RefMon.TakeDamage(1000);
            TakeDamage(50.0f);
        }
        else if (coll.tag == "EnemyBullet")
        {
            TakeDamage(20.0f);
            Destroy(coll.gameObject);
        }

        else if (coll.gameObject.name.Contains("Coin_Prefab") == true)
        {
            //##���� ��� ����
            Game_Mgr.Inst.IncreaseGold(10);
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.name.Contains("Heart_Prefab") == true)
        {
            m_CurHp +=  m_MaxHp * 0.5f;
            Game_Mgr.Inst.DamageTxt(m_MaxHp * 0.5f, transform.position, Color.green);

            if (m_MaxHp <= m_CurHp)
                m_CurHp = m_MaxHp;

            if (m_HpBar != null)
                m_HpBar.fillAmount = m_CurHp / m_MaxHp;

            Destroy(coll.gameObject);
        }


    }//void OnTriggerEnter2D(Collider2D coll)

    void TakeDamage(float a_Value)
    {
        if (m_CurHp <= 0.0f)
            return;

        if (0.0f< m_SdOnTime)
        {
            return;
        }



        Game_Mgr.Inst.DamageTxt(-a_Value, transform.position, Color.red);//������ ���


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
        //## �� ���� ������Ʈ
        if (0.0f < m_SdOnTime)
        {
            m_SdOnTime -= Time.deltaTime;
            if (m_ShieldObj != null && m_ShieldObj.activeSelf == false)
            {
                m_ShieldObj.SetActive(true);
            }
        }
        else
        {
            if (m_ShieldObj != null && m_ShieldObj.activeSelf == true)
            {
                m_ShieldObj.SetActive(false);
            }
        }



        //## ���� ��Ÿ�� ������Ʈ
        if (0.0f < m_CoolTime)
        {
            m_CoolTime -= Time.deltaTime;
        }



    }


    public void UseSkill(SkillType a_SKType)
    {
        if (m_CurHp <= 0.0f)
            return;

        //## ù��° ��ų ���
        if (a_SKType == SkillType.Skill_0)
        {
            m_CurHp += m_MaxHp * 0.2f;

            Game_Mgr.Inst.DamageTxt(m_MaxHp * 0.2f,
                transform.position, new Color(0.18f, 0.5f, 0.34f));

            if (m_MaxHp <= m_CurHp)
                m_CurHp = m_MaxHp;

            if (m_HpBar != null)
                m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        }

        //## �ι�° ��ų
        else if (a_SKType == SkillType.Skill_1)
        {
            GameObject a_Wolf = Instantiate(m_WolfPrefab);
            a_Wolf.transform.position = new Vector3(CameraResolution.m_ScWMin.x - 1.0f,
                              0.0f, 0.0f);
        }


        //## ����° ��ų ���
        else if (a_SKType == SkillType.Skill_2)
        {
            if (0.0f < m_SdOnTime)
            {
                return;
            }
            m_SdOnTime = m_SdDuration;

            // ��Ÿ�� 
            Game_Mgr.Inst.SkillCoolMethod(a_SKType, m_SdOnTime, m_SdDuration);


        }

        //## �׹�° ��ų ���
        else if (a_SKType == SkillType.Skill_3)
        {
            Vector3 a_Pos;
            GameObject a_CloneObj;

            for (float yy = 0.8f; yy >= -0.9f; yy -= 0.4f) //5�� �ݺ�
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

                a_CloneObj = Instantiate(m_HormingMs);
                a_CloneObj.transform.position = a_Pos;



            }


        }


        //## �ټ���° ��ų ���
        else if (a_SKType == SkillType.Skill_4)
        {
            if (0.0f < m_CoolTime)
            {
                return;
            }
            m_CoolTime = m_Dur;

            IsDoubleShot = true;
            StartCoroutine(DoubleShotSkill());
            Game_Mgr.Inst.SkillCoolMethod(a_SKType, m_CoolTime, m_Dur);
        }

        //## ������° ��ų ���
        else if (a_SKType == SkillType.Skill_5)
        {
            Summon_Ctrl a_Summon = Instantiate(summonPrefab).GetComponent<Summon_Ctrl>();

            // ������ ��ȯ���� �θ� ���ΰ� ���� ������Ʈ�� �����մϴ�.
            a_Summon.transform.parent = this.transform;
                
            Game_Mgr.Inst.SkillCoolMethod(a_SKType, m_SummonCoolTime, m_SummonCoolTime);

            Destroy(a_Summon.gameObject, 12.0f);

        }





        IEnumerator DoubleShotSkill()
        {
            float skillDuration = 12.0f; // ��ų ���� �ð�
            float elapsedTime = 0.0f;
            float spawnInterval = 0.1f; // ��ų �ߵ� ����
            float nextSpawnTime = 0.0f;

            while (elapsedTime < skillDuration)
            {
                if (elapsedTime >= nextSpawnTime)
                {
                    Vector3 a_Pos = m_ShootPos.transform.position;
                    GameObject a_CloneObj = Instantiate(m_DoubleShot);
                    a_CloneObj.transform.position = a_Pos;

                    nextSpawnTime = elapsedTime + spawnInterval;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            IsDoubleShot = false;
        }

    }
}
