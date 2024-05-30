using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet_Ctrl : MonoBehaviour
{
    Vector3 m_DirVec = Vector3.right;       //���ư��� �� ���� ����

    float m_MoveSpeed = 15.0f;              //�̵��ӵ�

    //float m_LifeTime = 3.0f;

    //## ����ź ����
    public bool isHoming = false; // ����ź �ɷ��� �������� ����


    // Start is called before the first frame update
    void Start()
    {
        //m_LifeTime = 3.0f;
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //if(0.0f < m_LifeTime)
        //{
        //    m_LifeTime -= Time.deltaTime;
        //    if(m_LifeTime <= 0.0f)
        //        Destroy(gameObject);
        //}
        // �̵� ai
        Vector3 m_CurPos = transform.position;

        if (isHoming)
        {
            // ���� ����� ���͸� ã���ϴ�.
            GameObject closestMonster = FindClosestMonster();
            if (closestMonster != null)
            {
                // ���͸� ���ϴ� ������ ����մϴ�.
                Vector3 directionToMonster = (closestMonster.transform.position - transform.position).normalized;

                // �̵� ������ ���͸� ���ϴ� �������� �����մϴ�.
                m_DirVec = directionToMonster;

                // ����ź�� ���͸� ���ϵ��� ȸ����ŵ�ϴ�.
                float angle = Mathf.Atan2(directionToMonster.y, directionToMonster.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        // �̵�
        transform.position += m_DirVec * m_MoveSpeed * Time.deltaTime;



        if (CameraResolution.m_ScWMax.x + 1.0f < transform.position.x ||
           transform.position.x < CameraResolution.m_ScWMin.x - 1.0f ||
           CameraResolution.m_ScWMax.y + 1.0f < transform.position.y ||
           transform.position.y < CameraResolution.m_ScWMin.y - 1.0f)
        { //�Ѿ��� ȭ���� �����... ��� ����
            Destroy(gameObject);
        }

    }//void Update()

    public void BulletSpawn(Vector3 a_StPos, Vector3 a_DirVec, float a_MvSpeed = 15.0f, float Att = 20.0f)
    {
        m_DirVec = a_DirVec;
        transform.position = new Vector3(a_StPos.x, a_StPos.y, 0.0f);
        m_MoveSpeed = a_MvSpeed;
    }

    public void HomingBulletSpawn(Vector3 a_StPos, Vector3 a_DirVec, float a_MvSpeed = 15.0f, float Att = 20.0f)
    {
        m_DirVec = a_DirVec;
        transform.position = new Vector3(a_StPos.x, a_StPos.y, 0.0f);
        m_MoveSpeed = a_MvSpeed;
        isHoming = true; // ����ź �ɷ� Ȱ��ȭ
    }


    // ���� ����� ���͸� ã�� �޼���
    GameObject FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject monster in monsters)
        {
            Vector3 diff = monster.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = monster;
                distance = curDistance;
            }
        }
        return closest;
    }


}
