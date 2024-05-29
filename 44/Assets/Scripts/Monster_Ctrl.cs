using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    // Start is called before the first frame update
    void Start()
    {
        m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();    

        m_SpawnPos = transform.position;    //������ ���� ��ġ ����
        m_RandY = Random.Range(0.5f, 2.0f); //Sin �Լ��� ���� ����
        m_CycleSpeed = Random.Range(1.8f, 5.0f);    //������ ������
    }

    // Update is called once per frame
    void Update()
    {
        if (m_MonType == MonType.MT_Zombi)
            Zombi_AI_Update();

        else if (m_MonType == MonType.MT_Missile)
            Missile_AI_Update();



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

            Destroy(gameObject);
        }
    }//public void TakeDamage(float a_Value)
}
