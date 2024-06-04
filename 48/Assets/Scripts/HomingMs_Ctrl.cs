using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMs_Ctrl : MonoBehaviour
{
    float m_MoveSpeed = 8.0f;   //�̵��ӵ�
    float m_RotSpeed = 250.0f;  //200.0f;  //ȸ���ӵ�

    //--- ����ź ����
    [HideInInspector] public GameObject Target_Obj = null;  //Ÿ�� ���� ����
    Vector3 m_DesireDir;    //Ÿ���� ���ϴ� ���� ����
    //--- ����ź ����

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target_Obj == null) //�����ؾ� �� Ÿ���� ������...
            FindEnemy();     //Ÿ���� ã���ִ� �Լ�

        if (Target_Obj != null)
            BulletHoming();     //Ÿ���� ���� ���� �̵��ϴ� �ൿ ���� �Լ�
        else
            transform.Translate(transform.right * m_MoveSpeed * Time.deltaTime, Space.World);
        //�̻��� �Ӹ������� �̵�
    }//void Update()

    void FindEnemy()  //Ÿ���� ã���ִ� �Լ�
    {
        GameObject[] a_EnemyList = GameObject.FindGameObjectsWithTag("Monster");

        if (a_EnemyList.Length <= 0) //������ �ִ� ���Ͱ� �ϳ��� ������...
            return;     //������ ����� ã�� ���Ѵ�.

        GameObject a_Find_Mon = null;
        Vector3 a_CacVec = Vector3.zero;
        Monster_Ctrl a_RefMon = null;
        for(int i = 0; i < a_EnemyList.Length; i++)
        {
            a_RefMon = a_EnemyList[i].GetComponent<Monster_Ctrl>();

            if (a_RefMon == null)
                continue;

            if (a_RefMon.m_HomingLockOn != null)
                continue;

            a_Find_Mon = a_EnemyList[i].gameObject;
            a_RefMon.m_HomingLockOn = this.gameObject;
            break;

        }//for(int i = 0; i < a_EnemyList.Length; i++)

        Target_Obj = a_Find_Mon;

    }//void FindEnemy()  //Ÿ���� ã���ִ� �Լ�

    void BulletHoming()  //Ÿ���� ���� ���� �̵��ϴ� �ൿ ���� �Լ�
    {
        m_DesireDir = Target_Obj.transform.position - transform.position;
        m_DesireDir.z = 0.0f;
        m_DesireDir.Normalize();

        //Ÿ���� ���� ����ź ȸ����Ű��
        float angle = Mathf.Atan2(m_DesireDir.y, m_DesireDir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward); //������ ���ʹϿ�����
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                        targetRot, m_RotSpeed * Time.deltaTime);

        //����ź�� �ٶ󺸴� ���������� �����̰� �ϱ�...
        transform.Translate(transform.right * m_MoveSpeed * Time.deltaTime, Space.World);

    }//void BulletHoming()  //Ÿ���� ���� ���� �̵��ϴ� �ൿ ���� �Լ�
}
