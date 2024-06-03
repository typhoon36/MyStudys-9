using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon_Ctrl : MonoBehaviour
{
    public GameObject Summons;
    public GameObject m_DoubleShot; // 더블샷 스킬 프리팹
    public Transform m_ShootPos1; // 스킬 발사 위치1
    public Transform m_ShootPos2; // 스킬 발사 위치2
    public Transform m_ShootPos3; // 스킬 발사 위치3
    float m_CoolTime = 0.0f; // 스킬 쿨타임
    float m_Dur = 12.0f; // 스킬 지속 시간
    public bool isDoubleShotActive = false; // 더블샷 스킬 활성화 상태

    void Start()
    {
        isDoubleShotActive = true;
        StartCoroutine(DoubleShotSkill());
    }

    void Update()
    {
        // 주인공 게임 오브젝트를 찾습니다.
        GameObject hero = GameObject.FindGameObjectWithTag("Player");

        // 주인공이 존재하면 주인공을 기준으로 소환수를 회전시킵니다.
        if (hero != null)
        {
            // Vector3.up 대신 -Vector3.up을 사용하여 시계 방향으로 회전합니다.
            transform.RotateAround(hero.transform.position, -Vector3.forward, 50.0f * Time.deltaTime);
        }

        // 더블샷 스킬을 사용합니다.
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
        float skillDuration = 12.0f; // 스킬 지속 시간
        float elapsedTime = 0.0f;
        float spawnInterval = 0.2f; // 스킬 발동 간격
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
        // 주인공 게임 오브젝트를 찾습니다.
        GameObject hero = GameObject.FindGameObjectWithTag("Player");

        if (hero != null)
        {
            // 프로젝타일을 발사 위치에서 생성합니다.
            GameObject projectile = Instantiate(m_DoubleShot, shotPosition.position, Quaternion.identity);

            // 생성된 프로젝타일의 부모를 주인공 게임 오브젝트로 설정합니다.
            projectile.transform.parent = hero.transform;

            // 12초 후에 프로젝타일을 파괴합니다.
            Destroy(projectile, 12.0f);
        }
    }
}
