using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HormingMs_Ctrl : MonoBehaviour
{
    // 이동속도
    float m_Speed = 8.0f;
    // 회전속도
    float m_RotSpeed = 200.0f;

    //## 유도탄 변수
    [HideInInspector] public GameObject Target_Obj = null;
    //타겟 오브젝트
    Vector3 m_DesireDir;
    //타겟 방향



    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);       
    }

    // Update is called once per frame
    void Update()
    {
        if(Target_Obj == null)
        {
            //유도탄이 타겟을 찾아가는 로직
            FindEnermy();
        }

        if(Target_Obj != null)
        {
           //유도탄이 타겟을 향해 회전하는 로직
            BulletHorming();

        }
        else
        {
            transform.Translate(Vector3.right * m_Speed * Time.deltaTime,Space.World);
        }




    }

    //## 유도탄이 타겟을 찾아가는 로직
    void FindEnermy()
    {
        GameObject[] a_EnermyList = GameObject.FindGameObjectsWithTag("Monster");

        if (a_EnermyList.Length <= 0)
            return;

        GameObject a_Find_Mon = null;
        Vector3 a_CacVec = Vector3.zero;
        Monster_Ctrl a_RefMon = null;

        for(int i = 0; i < a_EnermyList.Length; i++)
        {
            a_RefMon = a_EnermyList[i].GetComponent<Monster_Ctrl>();
            
            if(a_RefMon == null)
            
                continue;
            
            if(a_RefMon.m_HormingLockOn != null)
            
                continue;


            a_Find_Mon = a_EnermyList[i].gameObject;
            a_RefMon.m_HormingLockOn = this.gameObject;
            break;


        }
        Target_Obj = a_Find_Mon;


       
     
    }

    //## 유도탄이 타겟을 향해 추적 이동하는 로직
    void BulletHorming()
    {
        
        m_DesireDir = Target_Obj.transform.position - transform.position;
        m_DesireDir.z = 0.0f;
        m_DesireDir.Normalize();

        //## 타겟을 향해 회전

        float a_Angle = Mathf.Atan2(m_DesireDir.y, m_DesireDir.x) * Mathf.Rad2Deg;
        Quaternion a_TargetRot = Quaternion.AngleAxis(a_Angle, Vector3.forward);
        //각도를 회전값으로 변환
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            a_TargetRot, m_RotSpeed * Time.deltaTime);

        //## 유도탄이 바라보는 방향으로 회전
        transform.Translate(Vector3.right * m_Speed * Time.deltaTime, Space.Self);
      



    }



}
