using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Mgr : MonoBehaviour
{
    //## UI����
    public Text m_BestScoreTxt = null;
    //�ְ�����
    public Text m_CurScoreTxt = null;
    //��������
    public Text m_GoldText = null;
    //���� ���
    public Text m_InfoText = null;
    //���� ����
    public Button Lobby_Btn = null;
    //�κ�� ���� ��ư

    //���� ����
    int m_CurScore = 0;
    //���� ��尪
    int m_CurGold = 0;



    //## ĳ���� �Ӹ� ���� ������ ���� 
    GameObject m_DmgClone = null;
    DmgTxt_Ctrl m_DmgCtrl;
    Vector3 m_StCacPos;
    [Header("##Damage_Text")]
    public Transform DmgCanvs = null;
    public GameObject DmgTxt_Root = null;


    //## ���� ������ 
    GameObject m_CoinItem = null;
    //## ��Ʈ ������
    GameObject m_HeartItem = null;

    [Header("##Skill_Coll_Timer")]
    public GameObject m_SkillNode = null;
    public Transform m_SkillCool_Root = null;

    HeroCtrl m_RefHero = null;





    //## �̱��� ����
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
        //## ����Ű ��ų ���
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

        //## ����
        else if (Input.GetKeyDown(KeyCode.Alpha5)|| Input.GetKeyDown(KeyCode.Keypad5))
        {
            UseSill_Key(SkillType.Skill_4);
        }

        //## ��ȯ�� ��ȯ
        else if (Input.GetKeyDown(KeyCode.Alpha6)|| Input.GetKeyDown(KeyCode.Keypad6))
        {
            UseSill_Key(SkillType.Skill_5);
        }
    }

    //## ������ �ؽ�Ʈ    
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

    //## ���� ������ ����
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

    //## ��Ʈ ������ ����
    public void SpawnHeart(Vector3 a_Pos)
    {
        if (m_HeartItem == null)
            return;



        GameObject a_HeartObj = Instantiate(m_HeartItem);
        a_Pos.z = 0.0f;
        a_HeartObj.transform.position = a_Pos;

    }

    //## ��ų ��� �Լ�

    void UseSill_Key(SkillType a_SkType)
    {
        if (m_RefHero == null)
        {
            return;
        }

        m_RefHero.UseSkill(a_SkType);

    }

    //## ��ų ��Ÿ�� �Լ�
    public void SkillCoolMethod(SkillType a_SkType, float a_Time, float a_During)
    {
        GameObject a_Obj = Instantiate(m_SkillNode);
        a_Obj.transform.SetParent(m_SkillCool_Root, false); // WorldPositionStays = false

        SkillCool_Ctrl a_SCtrl = a_Obj.GetComponent<SkillCool_Ctrl>();

        if (a_SCtrl != null)

            a_SCtrl.InitState(a_SkType, a_Time, a_During);




    }

    // ������ ������Ű�� �Լ�.
    public void IncreaseScore(int amount)
    {
        m_CurScore += amount;

        // ���� ������ �ְ� �������� ���ٸ�, �ְ� ������ �����մϴ�.
        if (m_CurScore > GlobalVal.g_BestScore)
        {
            GlobalVal.g_BestScore = m_CurScore;
            PlayerPrefs.SetInt("HighScore", m_CurScore);
            PlayerPrefs.Save(); // ��������� ��� ��ũ�� �����մϴ�.
        }

        // UI�� ������Ʈ�մϴ�.
        m_CurScoreTxt.text = "���� ����:( " + m_CurScore + " )";
    }

    //��带 ������Ű�� �Լ�
    public void IncreaseGold(int amount)
    {
        m_CurGold += amount;

        // ��带 �����մϴ�.
        PlayerPrefs.SetInt("Gold", m_CurGold);
        PlayerPrefs.Save(); // ��������� ��� ��ũ�� �����մϴ�.

        // UI�� ������Ʈ�մϴ�.
        m_GoldText.text = "���� ���: " + m_CurGold;
    }


    public void LoadData()
    {
        // ������ ����� ���� �ְ� ������ �ε��մϴ�.
        m_CurGold = PlayerPrefs.GetInt("Gold", 0);
        GlobalVal.g_BestScore = PlayerPrefs.GetInt("HighScore", 0);

        // UI�� ������Ʈ�մϴ�.
        m_GoldText.text = "���� ��� (" + m_CurGold + ")";
        m_BestScoreTxt.text = "�ְ� ���� (" + GlobalVal.g_BestScore + ")";
    }

}
