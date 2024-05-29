using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject[] MonPrefab;
    public int monKillCount = 0;
    public bool isBossSpawned = false;

    float m_SpDelta = 0.0f;     //���� �ֱ� ���� ����
    float m_DiffSpawn = 3.0f;   //���̵��� ���� ���� ���� �ֱ� ����


    //--- �̱��� ����
    public static MonsterGenerator Inst = null;

    void Awake()
    {
        Inst = this;
    }
    //--- �̱��� ����

    void Update()
    {
        m_SpDelta -= Time.deltaTime;

        if (m_SpDelta <= 0.0f)
        {
            m_SpDelta = Random.Range(1.0f, m_DiffSpawn); // ���� ������ �����ϰ� ����

            if (monKillCount >= 7 && !isBossSpawned)
            {
                // ���� ����
                SpawnBoss();
                isBossSpawned = true;
            }
            else if (!isBossSpawned)
            {
                // �Ϲ� ���� ����
                float py = Random.Range(-3.0f, 3.0f);
                GameObject Go = Instantiate(MonPrefab[0]); //���� ����
                Go.transform.position = new Vector3(CameraResolution.m_ScWMax.x + 1.0f, py, 0.0f);
                StartCoroutine(SpawnMon(Random.Range(2.0f, 4.0f), py)); // 2�ʿ��� 4�� �Ŀ� �̻��� ����
            }
        }
    }

    public void SpawnBoss()
    {
        float px = Random.Range(3.0f, 3.2f);
        GameObject Go = Instantiate(MonPrefab[2]); //���� ����
        Go.transform.position = new Vector3(px, 0.0f, 0.0f);
    }



    public IEnumerator SpawnMon(float delay, float py)
    {
        yield return new WaitForSeconds(delay);

        if (MonPrefab[1] != null)
        {
            GameObject Go = Instantiate(MonPrefab[1]); //�̻��� ����
            Go.transform.position = new Vector3(CameraResolution.m_ScWMax.x + 1.0f, py, 0.0f);
        }


    }

    public IEnumerator RespawnBoss(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ������ �������� ���� ���� ������ ����
        if (MonPrefab[2] != null && GameObject.FindGameObjectWithTag("Boss") == null)
        {
            float py = Random.Range(-3.0f, 3.0f);
            GameObject Go = Instantiate(MonPrefab[2]); //���� �罺��
            Go.transform.position = new Vector3(CameraResolution.m_ScWMax.x + 1.0f, py, 0.0f);
        }
    }

}
