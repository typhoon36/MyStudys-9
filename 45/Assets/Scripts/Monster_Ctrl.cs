using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum BossState
{
    BS_appear, // 등장 및 이동상태
    BS_Normal_Att, //기본 공격
    BS_Fever_Att, //피버 공격
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

    //--- 몬스터 체력 변수
    float m_MaxHp = 200.0f;
    float m_CurHp = 200.0f;
    public Image m_HpBar = null;
    //--- 몬스터 체력 변수

    float m_Speed = 4.0f;   //이동속도
    Vector3 m_CurPos;       //위치 계산용 변수
    Vector3 m_SpawnPos;     //스폰 위치

    float m_CacPosY = 0.0f; //싸인 함수에 들어갈 누적 각도 계산용 변수
    float m_RandY   = 0.0f; //랜덤한 진폭값 저장용 변수
    float m_CycleSpeed = 0.0f;  //랜덤한 진동 속도 변수

    //## 총알 발사 변수
    public GameObject m_BulletPrefab = null;
    public GameObject m_ShootPos = null;

    float Shoot_Time = 0.0f;       //총알 발사 주기 계산용 변수
    float Shoot_Delay = 1.5f;      //총알 발사 쿨타임
    float BulletSpeed = 10.0f;     //총알 발사 속도

    //## 미사일 ai 변수
    HeroCtrl m_RefHero = null;
    Vector3 m_DirVec;

    //## 보스 ai 변수
    BossState m_BossState = BossState.BS_appear;
    float m_ShotCount = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();    

        m_SpawnPos = transform.position;    //몬스터의 스폰 위치 저장
        m_RandY = Random.Range(0.5f, 2.0f); //Sin 함수의 랜덤 진폭
        m_CycleSpeed = Random.Range(1.8f, 5.0f);    //진동수 랜덤값


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
            Destroy(gameObject);    //왼쪽 화면을 벗어나면 즉시 제거




    }

    void Zombi_AI_Update()
    {
        m_CurPos = transform.position;
        m_CurPos.x += (-1.0f * m_Speed * Time.deltaTime);
        m_CacPosY += (Time.deltaTime * m_CycleSpeed);
        m_CurPos.y = m_SpawnPos.y + Mathf.Sin(m_CacPosY) * m_RandY;
        transform.position = m_CurPos;

        //## 총알 발사

        if (m_BulletPrefab == null)
            return;

        Shoot_Time += Time.deltaTime;

        if (Shoot_Time >= Shoot_Delay)
        {
            Shoot_Time = 0.0f;

            
            GameObject a_Newobj = Instantiate(m_BulletPrefab); //총알 스폰
            Bullet_Ctrl a_BulletSc = a_Newobj.GetComponent<Bullet_Ctrl>();
           
            a_BulletSc.BulletSpawn(m_ShootPos.transform.position, 
                Vector3.left, BulletSpeed, 20.0f);
            

        }//if (Shoot_Time > Shoot_Delay)

    }


    void Missile_AI_Update()
    {
        //## 이동 ai
        m_CurPos = transform.position;
        m_DirVec = Vector3.left;


        //## 미사일 유도 ai
        if (m_RefHero != null)
        {
            Vector3 a_CacVec = m_RefHero.transform.position - transform.position;
            m_DirVec= a_CacVec;

            //## 미사일의 주인공과의 거리 범위
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
        //## 보스 등장
        if(m_BossState == BossState.BS_appear)
        {
            m_CurPos = transform.position;
            //도착
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
        //## 보스 일반 공격
        else if(m_BossState == BossState.BS_Normal_Att)
        {
            Shoot_Time -= Time.deltaTime;
            if (Shoot_Time <= 0.0f)
            {  //## 일반 공격
                Vector3 a_Target = m_RefHero.transform.position - transform.position;

                a_Target.z = 0.0f;
                a_Target.Normalize();

                GameObject a_Newobj = Instantiate(m_BulletPrefab); //총알 스폰
                Bullet_Ctrl a_BulletSc = a_Newobj.GetComponent<Bullet_Ctrl>();

                a_BulletSc.BulletSpawn(m_ShootPos.transform.position,
                                   a_Target, BulletSpeed);

                //## 날아가는 방향으로 회전
                a_Newobj.transform.right = new Vector3(-a_Target.x, -a_Target.y, 0.0f);


                m_ShotCount++;
                // 일반 공격 주기
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

        //## 보스 궁극기
        else if(m_BossState == BossState.BS_Fever_Att)
        {
            Shoot_Time -= Time.deltaTime;
            //##궁극기
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


                    //## 날아가는 방향으로 회전
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

                //##궁극기 종료
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
        if (m_CurHp <= 0.0f)  //이 몬스터가 이미 죽어 있으면...
            return;           //데미지를 차감할 필요 없으니 리턴 시키겠다는 뜻


        float a_CacDmg = a_Value;
        if (m_CurHp < a_Value)
            a_CacDmg = m_CurHp;
       

        Game_Mgr.Inst.DamageTxt(-a_Value, transform.position, Color.magenta);//고정 데미지 출력



        m_CurHp -= a_Value;
        if (m_CurHp < 0.0f)
            m_CurHp = 0.0f;

        if (m_HpBar != null)
            m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        if(m_CurHp <= 0.0f)
        { //몬스터 사망 처리

            //보상주기
            //## 골드 보상
            Game_Mgr.Inst.SpawnCoin(transform.position);


            //## 보스 사망
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
