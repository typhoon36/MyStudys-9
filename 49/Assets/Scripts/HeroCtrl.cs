using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrl : MonoBehaviour
{
    //--- 주인공 체력 변수
    float m_MaxHp = 200.0f;
    [HideInInspector] public float m_CurHp = 200.0f;
    public Image m_HpBar = null;
    //--- 주인공 체력 변수

    //--- 키보드 입력값 변수 선언
    float h = 0.0f;
    float v = 0.0f;

    float moveSpeed = 7.0f;
    Vector3 moveDir = Vector3.zero;
    //--- 키보드 입력값 변수 선언

    //--- 주인공 화면 밖으로 나갈 수 없도록 막기 위한 변수
    Vector3 HalfSize = Vector3.zero;
    Vector3 m_CacCurPos = Vector3.zero;
    //--- 주인공 화면 밖으로 나갈 수 없도록 막기 위한 변수

    //--- 총알 발사 변수
    public GameObject m_BulletPrefab = null;
    public GameObject m_ShootPos = null;
    float m_ShootCool = 0.0f;       //총알 발사 주기 계산용 변수
    //--- 총알 발사 변수

    //--- Wolf 스킬
    public GameObject m_WolfPrefab = null;
    //--- Wolf 스킬

    //--- 쉴드 스킬
    float m_SdOnTime = 0.0f;
    float m_SdDuration = 12.0f; //12초 동안 발동
    public GameObject ShieldObj = null;
    //--- 쉴드 스킬

    //--- 유도탄 스킬
    public GameObject m_HomingMs = null;
    //--- 유도탄 스킬

    // ##  더블샷 
    [HideInInspector] public float m_DoubleOnTime = 0.0f;
    float m_DoubleDur = 12.0f; //12초 동안 발동


    //## 서브 히어로 
    int SubHCount = 0;
    float m_SubHTime = 0.0f;
    float m_SubHInterval = 12.0f;
    public GameObject Sub_Parent = null;
    public GameObject SubHeroPrefab = null;



    // Start is called before the first frame update
    void Start()
    {
        //--- 캐릭터의 가로 반사이즈, 세로 반사이즈 구하기
        //월드에 그려진 스프라이트 사이즈 얻어오기
        SpriteRenderer sprRend = gameObject.GetComponentInChildren<SpriteRenderer>();
        //sprRend.bounds.size.x   스프라이트의 가로 사이즈
        //sprRend.bounds.size.y   스프라이트의 세로 사이즈
        // Debug.Log(sprRend.bounds.size);
        // (1.26, 1.58, 0.20)
        HalfSize.x = sprRend.bounds.size.x / 2.0f - 0.23f; //캐릭터의 가로 반 사이즈(여백이 커서 조금 줄임)
        HalfSize.y = sprRend.bounds.size.y / 2.0f - 0.05f; //캐릭터의 세로 반 사이즈
        HalfSize.z = 1.0f;
        //월드에 그려진 스프라이트 사이즈 얻어오기
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

            //## 더블샷일시
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



            //## 일반총알일시
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

        if (0.0f < m_SdOnTime)  //쉴드 스킬 발동 중 일 때 ... 데미지 스킵
            return;

        Game_Mgr.Inst.DamageText(-a_Value, transform.position, Color.blue);

        m_CurHp -= a_Value;
        if (m_CurHp < 0.0f)
            m_CurHp = 0.0f;

        if (m_HpBar != null)
            m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        if (m_CurHp <= 0.0f)
        {  //사망처리
            Time.timeScale = 0.0f;  //일시정지
        }
    }

    void Update_Skill()
    {
        //--- 쉴드 상태 업데이트
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
        //--- 쉴드 상태 업데이트

        //## 더블샷 업데이트
        if (0.0f < m_DoubleOnTime)
        {
            m_DoubleOnTime -= Time.deltaTime;

            if (m_DoubleOnTime <= 0.0f)
                m_DoubleOnTime = 0.0f;


        }

        //## 서브 히어로 업데이트    

        if (0.0f < m_SubHTime)
        {
            m_SubHTime -= Time.deltaTime;

            if (m_SubHTime <= 0.0f)
                m_SubHTime = 0.0f;
        }




    }

    public void UseSkill(SkillType a_SkType)
    {
        if (m_CurHp <= 0.0f) //주인공 사망시 스킬 발동 제외
            return;

        if (a_SkType == SkillType.Skill_0) //Hp 20% 힐링
        {
            m_CurHp += m_MaxHp * 0.2f;
            Game_Mgr.Inst.DamageText(m_MaxHp * 0.2f, transform.position,
                                            new Color(0.18f, 0.5f, 0.34f));

            if (m_MaxHp < m_CurHp)
                m_CurHp = m_MaxHp;

            if (m_HpBar != null)
                m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        }//if(a_SkType == SkillType.Skill_0) //Hp 20% 힐링
        else if (a_SkType == SkillType.Skill_1) //울프스킬
        {
            GameObject a_Clone = Instantiate(m_WolfPrefab);
            a_Clone.transform.position =
                    new Vector3(CameraResolution.m_ScWMin.x - 1.0f, 0.0f, 0.0f);
        }
        else if (a_SkType == SkillType.Skill_2)  //보호막
        {
            if (0.0f < m_SdOnTime)
                return;

            m_SdOnTime = m_SdDuration;

            //UI 쿨타임 발동
            Game_Mgr.Inst.SkillCoolMethod(a_SkType, m_SdOnTime, m_SdDuration);
        }
        else if (a_SkType == SkillType.Skill_3) //유도탄
        {
            //--- 4발 발사
            Vector3 a_Pos;
            GameObject a_CloneObj;
            for (float yy = 0.8f; yy > -0.9f; yy -= 0.4f)  //5번 반복됨
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
            }// for(float yy = 0.8f; yy > -0.9f; yy -= 0.4f)  //5번 반복됨
            //--- 4발 발사
        }


        //## 더블샷 스킬
        else if (a_SkType == SkillType.Skill_4)
        {
            if (0.0f < m_DoubleOnTime)
                return;

            m_DoubleOnTime = m_DoubleDur;

            //UI 쿨타임 발동
            Game_Mgr.Inst.SkillCoolMethod(a_SkType, m_DoubleOnTime, m_DoubleDur);
        }



        //## 소환수 스킬
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




            //UI 쿨타임 발동
            Game_Mgr.Inst.SkillCoolMethod(a_SkType, m_SubHTime, m_SubHInterval);

        }

        GlobalValue.g_CurSkillCount[(int)a_SkType]--;

        //## 로컬 저장
        PlayerPrefs.SetInt($"Skill_Item_{(int)a_SkType}", 
            GlobalValue.g_CurSkillCount[(int)a_SkType]);



    }//public void UseSkill(SkillType a_SkType)
}
