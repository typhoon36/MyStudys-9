using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon_Ctrl : MonoBehaviour
{
    public GameObject Summons;
    public GameObject m_DoubleShot; // ���� ��ų ������
    public Transform m_ShootPos1; // ��ų �߻� ��ġ1
    public Transform m_ShootPos2; // ��ų �߻� ��ġ2
    public Transform m_ShootPos3; // ��ų �߻� ��ġ3
    float m_CoolTime = 0.0f; // ��ų ��Ÿ��
    float m_Dur = 12.0f; // ��ų ���� �ð�
    public bool isDoubleShotActive = false; // ���� ��ų Ȱ��ȭ ����

    void Start()
    {
        isDoubleShotActive = true;
        StartCoroutine(DoubleShotSkill());
    }

    void Update()
    {
        // ���ΰ� ���� ������Ʈ�� ã���ϴ�.
        GameObject hero = GameObject.FindGameObjectWithTag("Player");

        // ���ΰ��� �����ϸ� ���ΰ��� �������� ��ȯ���� ȸ����ŵ�ϴ�.
        if (hero != null)
        {
            // Vector3.up ��� -Vector3.up�� ����Ͽ� �ð� �������� ȸ���մϴ�.
            transform.RotateAround(hero.transform.position, -Vector3.forward, 50.0f * Time.deltaTime);
        }

        // ���� ��ų�� ����մϴ�.
        if (isDoubleShotActive)
        {
            m_CoolTime += Time.deltaTime;
            if (m_CoolTime >= 0.5f)
            {
                FireFromPosition(m_ShootPos1);
                FireFromPosition(m_ShootPos2);
                FireFromPosition(m_ShootPos3);

                m_CoolTime = 0.0f;
            }

            if (m_CoolTime >= m_Dur)
            {
                isDoubleShotActive = false;
                m_CoolTime = 0.0f;
            }
        }
    }

    public void StartDoubleShotSkill()
    {
        if (!isDoubleShotActive)
        {
            isDoubleShotActive = true;
            StartCoroutine(DoubleShotSkill());
        }
    }

    IEnumerator DoubleShotSkill()
    {
        float skillDuration = 12.0f; // ��ų ���� �ð�
        float elapsedTime = 0.0f;
        float spawnInterval = 0.2f; // ��ų �ߵ� ����
        float nextSpawnTime = 0.0f;

        while (elapsedTime < skillDuration)
        {
            if (elapsedTime >= nextSpawnTime)
            {
                FireFromPosition(m_ShootPos1);
                FireFromPosition(m_ShootPos2);
                FireFromPosition(m_ShootPos3);

                nextSpawnTime = elapsedTime + spawnInterval;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDoubleShotActive = false;
    }

    void FireFromPosition(Transform shotPosition)
    {
        // ���ΰ� ���� ������Ʈ�� ã���ϴ�.
        GameObject hero = GameObject.FindGameObjectWithTag("Player");

        if (hero != null)
        {
            // ������Ÿ���� �߻� ��ġ���� �����մϴ�.
            GameObject projectile = Instantiate(m_DoubleShot, shotPosition.position, Quaternion.identity);

            // ������ ������Ÿ���� �θ� ���ΰ� ���� ������Ʈ�� �����մϴ�.
            projectile.transform.parent = hero.transform;

            // 12�� �Ŀ� ������Ÿ���� �ı��մϴ�.
            Destroy(projectile, 12.0f);
        }
    }
}
