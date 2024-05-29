using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    //## �ñر� �Ѿ� �߻� ����
    public GameObject m_UltiBulletPrefab = null;
    

    //## �� ����

    public GameObject m_Shield = null;
    public Image m_ShieldCooldownUI = null;

    bool m_ShieldActive = false;


    float m_ShieldCool = 0.0f;



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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GameScene");
            Time.timeScale = 1.0f;
            
        }


      
            // 1�� Ű�� ������ ü���� ȸ���մϴ�.
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // ü���� ȸ���մϴ�.
                float healAmount = 50.0f;
                m_CurHp += healAmount;

                // ü���� �ִ� ü���� �ʰ��ϸ� �ִ� ü������ �����մϴ�.
                if (m_CurHp > m_MaxHp)
                {
                    m_CurHp = m_MaxHp;
                }

                // ü�� �� UI�� ������Ʈ�մϴ�.
                if (m_HpBar != null)
                {
                    m_HpBar.fillAmount = m_CurHp / m_MaxHp;
                }

                // ȸ������ ǥ���մϴ�.
                Game_Mgr.Inst.DamageTxt(healAmount, transform.position, Color.green);
            }


        


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject ultimateSkill = Instantiate(m_UltiBulletPrefab, m_ShootPos.transform.position,
                Quaternion.identity);
            Bullet_Ctrl bulletCtrl = ultimateSkill.GetComponent<Bullet_Ctrl>();
            bulletCtrl.BulletSpawn(ultimateSkill.transform.position, Vector3.right);
        }



        // �� ��ư�� ������ �� ��������Ʈ�� Ȱ��ȭ�մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha3) && !m_ShieldActive && m_ShieldCooldownUI.fillAmount == 1.0f)
        {
            m_Shield.SetActive(true); // �� ���� ������Ʈ�� Ȱ��ȭ�մϴ�.
            m_ShieldActive = true; // ���� Ȱ��ȭ�մϴ�.
            m_ShieldCool = 0.0f; // ��Ÿ���� �����մϴ�.
            m_ShieldCooldownUI.fillAmount = 0.0f; // ��Ÿ�� �̹����� ���ϴ�.
        }

        // ���� Ȱ��ȭ�� ���¿��� ��Ÿ���� ������ŵ�ϴ�.
        if (m_ShieldActive)
        {
            m_ShieldCool += Time.deltaTime;

            // 10�ʰ� ������ �� ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            if (m_ShieldCool >= 10.0f)
            {
                m_Shield.SetActive(false); // �� ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
                m_ShieldActive = false; // ���� ��Ȱ��ȭ�մϴ�.
                m_ShieldCool = 0.0f; // ��Ÿ���� �缳���մϴ�.
            }
        }
        // ���� ��Ȱ��ȭ�� ���¿��� ��Ÿ���� ������Ű�� ��Ÿ�� �̹����� õõ�� ä��ϴ�.
        else
        {
            m_ShieldCool += Time.deltaTime;

            // ��Ÿ�� �̹����� 1�ʿ� 0.1f�� ä��ϴ�.
            m_ShieldCooldownUI.fillAmount += 0.1f * Time.deltaTime;

            // ��Ÿ���� �� �Ǹ� ���� Ȱ��ȭ�մϴ�.
            if (m_ShieldCool >= 10.0f)
            {
                m_ShieldCool = 0.0f; // ��Ÿ���� �缳���մϴ�.
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
            //##���� ��� ����

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



        Game_Mgr.Inst.DamageTxt(-a_Value, transform.position, Color.red); // ������ ���


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
}