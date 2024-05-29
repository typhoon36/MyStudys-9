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
    Entering,
    Attacking,
    Dead
}

public class Monster_Ctrl : MonoBehaviour
{
    public MonType m_MonType = MonType.MT_Zombi;

    BossState currentState = BossState.Entering;

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


        // ���� ���� ī�޶� ����
        if (m_MonType == MonType.MT_Boss)
        {
            StartCoroutine(ShakeCamera(1.0f, 0.1f));
        }

        if (m_MonType == MonType.MT_Boss)
        {
            m_MaxHp = 500.0f;
        }

        m_CurHp = m_MaxHp;


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


    bool isAttacking = false;
    void Boss_AI_Update()
    {
        switch (currentState)
        {
            case BossState.Entering:
                // ���� ����
                EnterBoss();
                break;
            case BossState.Attacking:
                // ���� ����
                
                if (!isAttacking)
                {
                    StartCoroutine(AttackBoss());
                }
                break;
            case BossState.Dead:
                // ��� �� �罺�� ����
                RespawnBoss();
                break;
        }
    }
    void EnterBoss()
    {
        // ���� ���� ���� ����
        currentState = BossState.Attacking;

        // ���� ���� �� ī�޶� ��鸲 ȿ�� ����
        StartCoroutine(ShakeCamera(1.0f, 0.1f));

        // ���� ���� ����
        StartCoroutine(AttackBoss());
    }


    // �Ѿ� �ӵ� ���� �߰�
    public float bulletSpeed = 10f;

    // AttackBoss �޼��带 �ڷ�ƾ���� ����
    IEnumerator AttackBoss()
    {
        isAttacking = true;
        // ## 360�� �ñ��� 3��
        for (int j = 0; j < 3; j++)
        {
            // 360���� �Ѿ��� �߻��մϴ�.
            for (int i = 0; i < 360; i += 20) // 10�� �������� �Ѿ��� �߻��մϴ�.
            {
                // �Ѿ��� �����մϴ�.
                GameObject bullet = Instantiate(m_BulletPrefab, m_ShootPos.transform.position, Quaternion.identity);

                // �Ѿ��� ������ �����մϴ�.
                float radian = i * Mathf.Deg2Rad; // ������ �������� ��ȯ�մϴ�.
                Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }

            // �� �߻� ���̿� �ణ�� �����̸� �ξ� ��� �Ѿ��� ���ÿ� �߻���� �ʵ��� �մϴ�.
            yield return new WaitForSeconds(0.9f);
        }

        // ## ���ΰ��� ���� �ܹ� ��� 7��
        for (int i = 0; i < 7; i++)
        {
            // ���ΰ��� ��ġ�� ã���ϴ�.
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // ���ΰ��� ���ϴ� ������ ����մϴ�.
                Vector3 direction = (player.transform.position - transform.position).normalized;

                // �Ѿ��� �����ϰ�, ���ΰ��� ���ϴ� �������� �߻��մϴ�.
                GameObject bullet = Instantiate(m_BulletPrefab,m_ShootPos.transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }

            // �Ѿ� ���̿� �ణ�� �����̸� �ξ� ��� �Ѿ��� ���ÿ� �߻���� �ʵ��� �մϴ�.
            yield return new WaitForSeconds(1.0f);
        }
        isAttacking = false;
    }


    void RespawnBoss()
    {
        // ��� �� �罺�� ���� ����
        float respawnTime = Random.Range(25.0f, 30.0f);

        // respawnTime �Ŀ� ���� �罺�� ���� ����
        StartCoroutine(RespawnAfterSeconds(respawnTime));
    }

    IEnumerator RespawnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);



        // ������ ���¸� '����' ���·� ����
        currentState = BossState.Entering;
    }




    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPosition = Camera.main.transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.position = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.position = originalPosition;
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



        if(m_MonType == MonType.MT_Boss)
        {
            a_Value *= 0.8f;
        }


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


            // ������ �׾��� �� �罺��
            if (m_MonType == MonType.MT_Boss)
            {
                MonsterGenerator.Inst.monKillCount = 0;
                MonsterGenerator.Inst.isBossSpawned = false;
            }
            else
            {
                MonsterGenerator.Inst.monKillCount++;
            }


            Destroy(gameObject);
        }
    }//public void TakeDamage(float a_Value)
}
