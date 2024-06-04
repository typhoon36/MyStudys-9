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

public enum BossState
{
    BS_APPEAR_MOVE,     //���� �̵� ���� 
    BS_NORMAL_ATT,      //�⺻ ���� ����
    BS_FEVER_ATT        //�ǹ� Ÿ�� ���� ����
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
    float m_SvCycleSpeed = 0.0f;  //������ ���� �ӵ� Save ����

    //--- �Ѿ� �߻� ���� ���� ����
    public GameObject m_ShootPos = null;
    public GameObject m_BulletPrefab = null;
    float shoot_Time = 0.0f;     //�Ѿ� �߻� �ֱ� ���� ����
    float shoot_Delay = 1.5f;    //�Ѿ� �� Ÿ��
    float BulletMySpeed = 10.0f; //�Ѿ� �̵� �ӵ�
    //--- �Ѿ� �߻� ���� ���� ����

    //--- �̻��� �ൿ ���Ͽ� �ʿ��� ����
    HeroCtrl m_RefHero = null;  //���Ͱ� �����ϰ� �� ���ΰ� ��ü ����
    Vector3 m_DirVec;
    //--- �̻��� �ൿ ���Ͽ� �ʿ��� ����

    //--- ������ �ൿ ���� ���� ����
    BossState m_BossState = BossState.BS_APPEAR_MOVE;   //���� �̵� ����
    int m_ShootCount = 0;
    //--- ������ �ൿ ���� ���� ����

    [HideInInspector] public GameObject m_HomingLockOn = null;
    //�� ���͸� �����ϰ� �ִ� ����ź�� ���� ����
    //�� ������ ���¸� ���� ���µǾ� �ִ��� �ƴ����� Ȯ���Ѵ�.

    // Start is called before the first frame update
    void Start()
    {
        m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();

        m_SpawnPos = transform.position;    //������ ���� ��ġ ����
        m_RandY = Random.Range(0.5f, 2.0f); //Sin �Լ��� ���� ����
        m_CycleSpeed = Random.Range(1.8f, 5.0f);    //������ ������
        m_SvCycleSpeed = m_CycleSpeed;  //������ ���� �ӵ� Save ����

        if (m_MonType == MonType.MT_Boss)
        {
            m_MaxHp = 3000.0f;  //�ִ� ü��ġ
            m_CurHp = m_MaxHp;  //���� ü��
        }
    }

    //void FixedUpdate()
    //{
    //    m_Speed = 4.0f;   //�̵��ӵ�
    //    m_CycleSpeed = m_SvCycleSpeed;  //������ ���� �ӵ� Save ����
    //}

    //void OnTriggerStay2D(Collider2D coll)
    //{
    //    if (coll.gameObject.name.Contains("ShieldField") == true)
    //    //if (coll.gameObject.tag == "Shield")
    //    {
    //        m_Speed = 4.0f * 0.2f;   //�̵��ӵ�
    //        m_CycleSpeed = m_SvCycleSpeed * 0.2f;  //������ ���� �ӵ� Save ����
    //    }
    //}

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

        //--- �Ѿ� �߻�
        if (m_BulletPrefab == null)
            return;

        shoot_Time += Time.deltaTime;
        if(shoot_Delay <= shoot_Time)
        {
            shoot_Time = 0.0f;

            GameObject a_NewObj = Instantiate(m_BulletPrefab);
            Bullet_Ctrl a_BulletSc = a_NewObj.GetComponent<Bullet_Ctrl>();
            a_BulletSc.BulletSpawn(m_ShootPos.transform.position, Vector3.left, BulletMySpeed);
 
        }//if(shoot_Delay <= shoot_Time)
        //--- �Ѿ� �߻�
    }

    void Missile_AI_Update()
    {
        m_CurPos = transform.position;
        m_DirVec = Vector3.left;

        if(m_RefHero != null)
        {
            Vector3 a_CacVec = m_RefHero.transform.position - transform.position;
            m_DirVec = a_CacVec;

            //�̻����� ���ΰ����� �Ÿ��� ������������ 3.5m �̻��̸� ������ ��ȭ����
            //�������θ� �̵���Ű���� �ǵ�
            if (a_CacVec.x < -3.5f)
                m_DirVec.y = 0.0f;
        }

        m_DirVec.Normalize();
        m_DirVec.x = -1.0f; //���ΰ��� ����ġ�� �� �Ŀ��� ������ ���� �������� �̵��ϰ�...
        m_DirVec.z = 0.0f;

        m_CurPos += (m_DirVec * Time.deltaTime * m_Speed);
        transform.position = m_CurPos;
    }

    void Boss_AI_Update()
    {
        if(m_BossState == BossState.BS_APPEAR_MOVE)  //���� �̵� ����
        {
            m_CurPos = transform.position;
            float a_ArrivePos = CameraResolution.m_ScWMax.x - 1.9f; //���� ��ġ
            if(a_ArrivePos < m_CurPos.x)
            {
                m_CurPos.x += (-1.0f * Time.deltaTime * m_Speed);   //�������� �̵�
                if(m_CurPos.x <= a_ArrivePos) //������ġ���� �������� �� 
                {
                    shoot_Time = 1.28f;
                    m_BossState = BossState.BS_FEVER_ATT;
                }
            }//if(a_ArrivePos < m_CurPos.x)

            transform.position = m_CurPos;

        }//if(m_BossState == BossState.BS_APPEAR_MOVE)  //���� �̵� ����
        else if(m_BossState == BossState.BS_NORMAL_ATT) //�Ϲ� ����
        {
            shoot_Time -= Time.deltaTime;
            if (shoot_Time <= 0.0f)
            {
                //�Ϲݰ���
                Vector3 a_TargetV =
                        m_RefHero.transform.position - m_ShootPos.transform.position;
                a_TargetV.z = 0.0f;
                a_TargetV.Normalize();

                GameObject a_NewObj = Instantiate(m_BulletPrefab);
                Bullet_Ctrl a_BulletSc = a_NewObj.GetComponent<Bullet_Ctrl>();
                a_BulletSc.BulletSpawn(m_ShootPos.transform.position,
                                                        a_TargetV, BulletMySpeed);

                //--- �Ѿ��� ���ư��� �������� ȸ����Ű��...
                a_NewObj.transform.right = new Vector3(-a_TargetV.x, -a_TargetV.y, 0.0f);
                //a_CacAngle = Mathf.Atan2(a_TargetV.y, a_TargetV.x) * Mathf.Rad2Deg;
                //a_CacAngle += 180.0f;   //Flip x �� üũ�Ǿ� �ֱ� ������...
                //a_NewObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, a_CacAngle);
                //--- �Ѿ��� ���ư��� �������� ȸ����Ű��...

                m_ShootCount++;
                if (m_ShootCount < 7) //�Ϲݰ��� 7�������� ���� �ֱ�
                {
                    shoot_Time = 0.7f;
                }
                else
                {
                    m_ShootCount = 0;
                    shoot_Time = 2.0f;
                    m_BossState = BossState.BS_FEVER_ATT;
                }
            }//if (shoot_Time <= 0.0f)

        }//else if(m_BossState == BossState.BS_NORMAL_ATT) //�Ϲ� ����
        else if(m_BossState == BossState.BS_FEVER_ATT)  //�ǹ� ����
        {
            shoot_Time -= Time.deltaTime;
            if(shoot_Time <= 0.0f)
            {
                //�ñر� ����
                float Radius = 1.0f;
                Vector3 a_TargetV = Vector3.zero;
                GameObject a_NewObj = null;
                Bullet_Ctrl a_BulletSc = null;
                //float a_CacAngle = 0.0f;
                for(float Angle = 0.0f; Angle < 360.0f; Angle += 15.0f)
                {
                    a_TargetV.x = Mathf.Sin(Angle * Mathf.Deg2Rad) * Radius;
                    a_TargetV.y = Mathf.Cos(Angle * Mathf.Deg2Rad) * Radius;
                    a_TargetV.Normalize();
                    a_NewObj = Instantiate(m_BulletPrefab);
                    a_BulletSc = a_NewObj.GetComponent<Bullet_Ctrl>();
                    a_BulletSc.BulletSpawn(m_ShootPos.transform.position,
                                                        a_TargetV, BulletMySpeed);

                    //--- �Ѿ��� ���ư��� �������� ȸ����Ű��...
                    a_NewObj.transform.right = new Vector3(-a_TargetV.x, -a_TargetV.y, 0.0f);
                    //a_CacAngle = Mathf.Atan2(a_TargetV.y, a_TargetV.x) * Mathf.Rad2Deg;
                    //a_CacAngle += 180.0f;   //Flip x �� üũ�Ǿ� �ֱ� ������...
                    //a_NewObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, a_CacAngle);
                    //--- �Ѿ��� ���ư��� �������� ȸ����Ű��...
                }//for(float Angle = 0.0f; Angle < 360.0f; Angle += 15.0f)

                m_ShootCount++;
                if(m_ShootCount < 3)
                {
                    shoot_Time = 1.0f;
                }
                else  
                {
                    m_ShootCount = 0;
                    shoot_Time = 1.5f; //�ñر⿡�� �⺻ �������� �Ѿ �� 1.5�� ������ �� ���� ���� 
                    m_BossState = BossState.BS_NORMAL_ATT;
                }

            }//if(shoot_Time <= 0.0f)
        }//else if(m_BossState == BossState.BS_FEVER_ATT)  //�ǹ� ����

    }//void Boss_AI_Update()

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "AllyBullet")
        {
            if (coll.gameObject.name.Contains("HomingMissile") == true)
            {
                TakeDamage(150.0f);
                Destroy(coll.gameObject);
            }
            else
            {
                TakeDamage(80.0f);
                Destroy(coll.gameObject);
            }
        }
    }//void OnTriggerEnter2D(Collider2D coll)

    public void TakeDamage(float a_Value)
    {
        if (m_CurHp <= 0.0f)  //�� ���Ͱ� �̹� �׾� ������...
            return;           //�������� ������ �ʿ� ������ ���� ��Ű�ڴٴ� ��

        float a_CacDmg = a_Value;
        if (m_CurHp < a_Value)
            a_CacDmg = m_CurHp;

        Game_Mgr.Inst.DamageText(-a_CacDmg, transform.position, Color.red);

        m_CurHp -= a_Value;
        if (m_CurHp < 0.0f)
            m_CurHp = 0.0f;

        if (m_HpBar != null)
            m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        if(m_CurHp <= 0.0f)
        { //���� ��� ó��

            //�����ֱ�

            //--- ��� ����
            Game_Mgr.Inst.SpawnCoin(transform.position);
            //--- ��� ����

            //--- ��Ʈ ����
            if(m_MonType == MonType.MT_Boss)
                Game_Mgr.Inst.SpawnHeart(transform.position);
            //--- ��Ʈ ����

            //--- ����� ���Ͱ� ������ ������ ���� �ֱ� ����
            if (m_MonType == MonType.MT_Boss)
            {
                MonsterGenerator.Inst.m_SpBossTimer = Random.Range(25.0f, 30.0f);
            }
            //--- ����� ���Ͱ� ������ ������ ���� �ֱ� ����

            Destroy(gameObject);
        }
    }//public void TakeDamage(float a_Value)
}
