using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet_Ctrl : MonoBehaviour
{
    Vector3 m_DirVec = Vector3.right;       //날아가야 할 방향 벡터

    float m_MoveSpeed = 15.0f;              //이동속도

    //float m_LifeTime = 3.0f;

    //## 유도탄 변수
    public bool isHoming = false; // 유도탄 능력을 가지는지 여부


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
        // 이동 ai
        Vector3 m_CurPos = transform.position;

        if (isHoming)
        {
            // 가장 가까운 몬스터를 찾습니다.
            GameObject closestMonster = FindClosestMonster();
            if (closestMonster != null)
            {
                // 몬스터를 향하는 방향을 계산합니다.
                Vector3 directionToMonster = (closestMonster.transform.position - transform.position).normalized;

                // 이동 방향을 몬스터를 향하는 방향으로 설정합니다.
                m_DirVec = directionToMonster;

                // 유도탄을 몬스터를 향하도록 회전시킵니다.
                float angle = Mathf.Atan2(directionToMonster.y, directionToMonster.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        // 이동
        transform.position += m_DirVec * m_MoveSpeed * Time.deltaTime;



        if (CameraResolution.m_ScWMax.x + 1.0f < transform.position.x ||
           transform.position.x < CameraResolution.m_ScWMin.x - 1.0f ||
           CameraResolution.m_ScWMax.y + 1.0f < transform.position.y ||
           transform.position.y < CameraResolution.m_ScWMin.y - 1.0f)
        { //총알이 화면을 벗어나면... 즉시 제거
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
        isHoming = true; // 유도탄 능력 활성화
    }


    // 가장 가까운 몬스터를 찾는 메서드
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
