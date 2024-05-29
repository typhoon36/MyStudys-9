using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    //## 궁극기 총알 발사 변수
    public GameObject m_UltiBulletPrefab = null;
    

    //## 방어막 변수

    public GameObject m_Shield = null;
    public Image m_ShieldCooldownUI = null;

    bool m_ShieldActive = false;


    float m_ShieldCool = 0.0f;



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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GameScene");
            Time.timeScale = 1.0f;
            
        }


      
            // 1번 키를 누르면 체력을 회복합니다.
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 체력을 회복합니다.
                float healAmount = 50.0f;
                m_CurHp += healAmount;

                // 체력이 최대 체력을 초과하면 최대 체력으로 설정합니다.
                if (m_CurHp > m_MaxHp)
                {
                    m_CurHp = m_MaxHp;
                }

                // 체력 바 UI를 업데이트합니다.
                if (m_HpBar != null)
                {
                    m_HpBar.fillAmount = m_CurHp / m_MaxHp;
                }

                // 회복량을 표시합니다.
                Game_Mgr.Inst.DamageTxt(healAmount, transform.position, Color.green);
            }


        


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject ultimateSkill = Instantiate(m_UltiBulletPrefab, m_ShootPos.transform.position,
                Quaternion.identity);
            Bullet_Ctrl bulletCtrl = ultimateSkill.GetComponent<Bullet_Ctrl>();
            bulletCtrl.BulletSpawn(ultimateSkill.transform.position, Vector3.right);
        }



        // 방어막 버튼을 누르면 방어막 스프라이트를 활성화합니다.
        if (Input.GetKeyDown(KeyCode.Alpha3) && !m_ShieldActive && m_ShieldCooldownUI.fillAmount == 1.0f)
        {
            m_Shield.SetActive(true); // 방어막 게임 오브젝트를 활성화합니다.
            m_ShieldActive = true; // 방어막을 활성화합니다.
            m_ShieldCool = 0.0f; // 쿨타임을 시작합니다.
            m_ShieldCooldownUI.fillAmount = 0.0f; // 쿨타임 이미지를 비웁니다.
        }

        // 방어막이 활성화된 상태에서 쿨타임을 증가시킵니다.
        if (m_ShieldActive)
        {
            m_ShieldCool += Time.deltaTime;

            // 10초가 지나면 방어막 게임 오브젝트를 비활성화합니다.
            if (m_ShieldCool >= 10.0f)
            {
                m_Shield.SetActive(false); // 방어막 게임 오브젝트를 비활성화합니다.
                m_ShieldActive = false; // 방어막을 비활성화합니다.
                m_ShieldCool = 0.0f; // 쿨타임을 재설정합니다.
            }
        }
        // 방어막이 비활성화된 상태에서 쿨타임을 증가시키고 쿨타임 이미지를 천천히 채웁니다.
        else
        {
            m_ShieldCool += Time.deltaTime;

            // 쿨타임 이미지를 1초에 0.1f씩 채웁니다.
            m_ShieldCooldownUI.fillAmount += 0.1f * Time.deltaTime;

            // 쿨타임이 다 되면 방어막을 활성화합니다.
            if (m_ShieldCool >= 10.0f)
            {
                m_ShieldCool = 0.0f; // 쿨타임을 재설정합니다.
            }
        }







        LimitMove();

        FireUpdate();
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

            GameObject a_CloneObj = Instantiate(m_BulletPrefab);
            a_CloneObj.transform.position = m_ShootPos.transform.position;
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
            //##유저 골드 증가

            Destroy(coll.gameObject);
        }

        else if (coll.tag == "Boss")
        {
            TakeDamage(5);


        }


    }//void OnTriggerEnter2D(Collider2D coll)

    void TakeDamage(float a_Value)
    {
      
        if(m_ShieldActive)
        {
            return;
        }



        if (m_CurHp <= 0.0f)
            return;



        Game_Mgr.Inst.DamageTxt(-a_Value, transform.position, Color.red); // 데미지 출력


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
}
