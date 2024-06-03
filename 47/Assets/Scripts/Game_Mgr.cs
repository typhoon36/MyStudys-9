using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Mgr : MonoBehaviour
{
    //## UI관련
    public Text m_BestScoreTxt = null;
    //최고점수
    public Text m_CurScoreTxt = null;
    //현재점수
    public Text m_GoldText = null;
    //보유 골드
    public Text m_InfoText = null;
    //유저 정보
    public Button Lobby_Btn = null;
    //로비로 가기 버튼

    //얻은 점수
    int m_CurScore = 0;
    //얻은 골드값
    int m_CurGold = 0;



    //## 캐릭터 머리 위에 데미지 띄우기 
    GameObject m_DmgClone = null;
    DmgTxt_Ctrl m_DmgCtrl;
    Vector3 m_StCacPos;
    [Header("##Damage_Text")]
    public Transform DmgCanvs = null;
    public GameObject DmgTxt_Root = null;


    //## 코인 아이템 
    GameObject m_CoinItem = null;
    //## 하트 아이템
    GameObject m_HeartItem = null;

    [Header("##Skill_Coll_Timer")]
    public GameObject m_SkillNode = null;
    public Transform m_SkillCool_Root = null;

    HeroCtrl m_RefHero = null;





    //## 싱글톤 패턴
    public static Game_Mgr Inst = null;


    void Awake()
    {
        Inst = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        if (Lobby_Btn != null)
        {
            Lobby_Btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbyScene");
            });
        }



        LoadData();



        m_CoinItem = Resources.Load("Coin_Prefab") as GameObject;

        m_HeartItem = Resources.Load("Heart_Prefab") as GameObject;

        m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        //## 단축키 스킬 사용
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            UseSill_Key(SkillType.Skill_0);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.Keypad2))
        {
            UseSill_Key(SkillType.Skill_1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)|| Input.GetKeyDown(KeyCode.Keypad3))
        {
            UseSill_Key(SkillType.Skill_2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)|| Input.GetKeyDown(KeyCode.Keypad4))
        {
            UseSill_Key(SkillType.Skill_3);
        }

        //## 더블샷
        else if (Input.GetKeyDown(KeyCode.Alpha5)|| Input.GetKeyDown(KeyCode.Keypad5))
        {
            UseSill_Key(SkillType.Skill_4);
        }

        //## 소환수 소환
        else if (Input.GetKeyDown(KeyCode.Alpha6)|| Input.GetKeyDown(KeyCode.Keypad6))
        {
            UseSill_Key(SkillType.Skill_5);
        }
    }

    //## 데미지 텍스트    
    public void DamageTxt(float a_Val, Vector3 a_Pos, Color a_Color)
    {
        if (DmgCanvs == null || DmgTxt_Root == null)
            return;

        m_DmgClone = Instantiate(DmgTxt_Root);
        m_DmgClone.transform.SetParent(DmgCanvs);

        m_DmgCtrl = m_DmgClone.GetComponent<DmgTxt_Ctrl>();

        if (m_DmgCtrl != null)
            m_DmgCtrl.InitDamage(a_Val, a_Color);

        m_StCacPos = new Vector3(a_Pos.x, a_Pos.y + 1.14f, 0.0f);
        m_DmgClone.transform.position = m_StCacPos;
    }

    //## 코인 아이템 생성
    public void SpawnCoin(Vector3 a_Pos, int a_Val = 10)
    {
        if (m_CoinItem == null)

            return;
        GameObject a_CoinObj = Instantiate(m_CoinItem);
        a_CoinObj.transform.position = a_Pos;
        Coin_Ctrl a_CoinCtrl = a_CoinObj.GetComponent<Coin_Ctrl>();
        if (a_CoinCtrl != null)
            a_CoinCtrl.m_RefHero = m_RefHero;



    }

    //## 하트 아이템 생성
    public void SpawnHeart(Vector3 a_Pos)
    {
        if (m_HeartItem == null)
            return;



        GameObject a_HeartObj = Instantiate(m_HeartItem);
        a_Pos.z = 0.0f;
        a_HeartObj.transform.position = a_Pos;

    }

    //## 스킬 사용 함수

    void UseSill_Key(SkillType a_SkType)
    {
        if (m_RefHero == null)
        {
            return;
        }

        m_RefHero.UseSkill(a_SkType);

    }

    //## 스킬 쿨타임 함수
    public void SkillCoolMethod(SkillType a_SkType, float a_Time, float a_During)
    {
        GameObject a_Obj = Instantiate(m_SkillNode);
        a_Obj.transform.SetParent(m_SkillCool_Root, false); // WorldPositionStays = false

        SkillCool_Ctrl a_SCtrl = a_Obj.GetComponent<SkillCool_Ctrl>();

        if (a_SCtrl != null)

            a_SCtrl.InitState(a_SkType, a_Time, a_During);




    }

    // 점수를 증가시키는 함수.
    public void IncreaseScore(int amount)
    {
        m_CurScore += amount;

        // 현재 점수가 최고 점수보다 높다면, 최고 점수를 갱신합니다.
        if (m_CurScore > GlobalVal.g_BestScore)
        {
            GlobalVal.g_BestScore = m_CurScore;
            PlayerPrefs.SetInt("HighScore", m_CurScore);
            PlayerPrefs.Save(); // 변경사항을 즉시 디스크에 저장합니다.
        }

        // UI를 업데이트합니다.
        m_CurScoreTxt.text = "현재 점수:( " + m_CurScore + " )";
    }

    //골드를 증가시키는 함수
    public void IncreaseGold(int amount)
    {
        m_CurGold += amount;

        // 골드를 저장합니다.
        PlayerPrefs.SetInt("Gold", m_CurGold);
        PlayerPrefs.Save(); // 변경사항을 즉시 디스크에 저장합니다.

        // UI를 업데이트합니다.
        m_GoldText.text = "보유 골드: " + m_CurGold;
    }


    public void LoadData()
    {
        // 이전에 저장된 골드와 최고 점수를 로드합니다.
        m_CurGold = PlayerPrefs.GetInt("Gold", 0);
        GlobalVal.g_BestScore = PlayerPrefs.GetInt("HighScore", 0);

        // UI를 업데이트합니다.
        m_GoldText.text = "보유 골드 (" + m_CurGold + ")";
        m_BestScoreTxt.text = "최고 점수 (" + GlobalVal.g_BestScore + ")";
    }

}
