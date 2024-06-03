using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HormingMs_Ctrl : MonoBehaviour
{
    // �̵��ӵ�
    float m_Speed = 8.0f;
    // ȸ���ӵ�
    float m_RotSpeed = 200.0f;

    //## ����ź ����
    [HideInInspector] public GameObject Target_Obj = null;
    //Ÿ�� ������Ʈ
    Vector3 m_DesireDir;
    //Ÿ�� ����



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
            //����ź�� Ÿ���� ã�ư��� ����
            FindEnermy();
        }

        if(Target_Obj != null)
        {
           //����ź�� Ÿ���� ���� ȸ���ϴ� ����
            BulletHorming();

        }
        else
        {
            transform.Translate(Vector3.right * m_Speed * Time.deltaTime,Space.World);
        }




    }

    //## ����ź�� Ÿ���� ã�ư��� ����
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

    //## ����ź�� Ÿ���� ���� ���� �̵��ϴ� ����
    void BulletHorming()
    {
        
        m_DesireDir = Target_Obj.transform.position - transform.position;
        m_DesireDir.z = 0.0f;
        m_DesireDir.Normalize();

        //## Ÿ���� ���� ȸ��

        float a_Angle = Mathf.Atan2(m_DesireDir.y, m_DesireDir.x) * Mathf.Rad2Deg;
        Quaternion a_TargetRot = Quaternion.AngleAxis(a_Angle, Vector3.forward);
        //������ ȸ�������� ��ȯ
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            a_TargetRot, m_RotSpeed * Time.deltaTime);

        //## ����ź�� �ٶ󺸴� �������� ȸ��
        transform.Translate(Vector3.right * m_Speed * Time.deltaTime, Space.Self);
      



    }



}
