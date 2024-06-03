using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject[] MonPrefab;

    float m_SpDelta = 0.0f;     //스폰 주기 계산용 변수
    float m_DiffSpawn = 1.0f;   //난이도에 따른 몬스터 스폰 주기 변수



    [HideInInspector] public float m_SpBsDelta = 20.0f; //보스 스폰 주기 계산용 변수


    //--- 싱글톤 패턴
    public static MonsterGenerator Inst = null;

    void Awake()
    {
        Inst = this;
    }
    //--- 싱글톤 패턴

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_SpDelta -= Time.deltaTime;
        if(m_SpDelta < 0.0f)
        {
            m_SpDelta  = m_DiffSpawn;

            GameObject Go = null;

            int dice = Random.Range(1,11);
            //1~10 랜덤 발생

            if(dice > 2)
                Go = Instantiate(MonPrefab[0]); 
            else
            {
                 Go = Instantiate(MonPrefab[1]);
            }

            float py = Random.Range(-3.0f, 3.0f);
            Go.transform.position = new Vector3(CameraResolution.m_ScWMax.x + 1.0f, py, 0.0f);

        }//if(m_SpDelta < 0.0f)

        //## 보스 스폰
        if(0.0f < m_SpBsDelta)
        {
            m_SpBsDelta -= Time.deltaTime;
            if(m_SpBsDelta < 0.0f)
            {
                m_SpBsDelta = 20.0f;

                GameObject Go = Instantiate(MonPrefab[2]);
                float py3 = Random.Range(-3.0f, 3.0f);
                Go.transform.position = 
                    new Vector3(CameraResolution.m_ScWMax.x + 1.0f, 0.0f, 0.0f);
            }
        }//if(0.0f < m_SpBsDelta



    }//void Update()
}
