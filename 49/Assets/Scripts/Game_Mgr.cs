using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Mgr : MonoBehaviour
{
    public Text m_BestScoreText = null; //�ִ����� ǥ�� UI
    public Text m_CurScoreText = null;  //�������� ǥ�� UI
    public Text m_GoldText = null;      //������� ǥ�� UI
    public Text m_UserInfoText = null;  //���� ���� ǥ�� UI
    public Button GoLobbyBtn = null;    //�κ�� �̵� ��ư

    int m_CurScore = 0;     //�̹� ������������ ���� ���� ����
    int m_CurGold = 0;      //�̹� ������������ ���� ��尪

    //--- ĳ���� �Ӹ����� ������ ����� ���� ����
    GameObject m_DmgClone;  //Damage Text ���纻�� ���� ����
    DmgTxt_Ctrl m_DmgTxt;   //Damage Text ���纻�� �پ� �ִ� DmgTxt_Ctrl ������Ʈ�� ���� ����
    Vector3 m_StCacPos;     //���� ��ġ�� ����� �ֱ� ���� ����
    [Header("--- Damage Text ---")]
    public Transform Damage_Canvas = null;
    public GameObject DmgTxtRoot = null;
    //--- ĳ���� �Ӹ����� ������ ����� ���� ����

    //--- ���� ������ ���� ����
    GameObject m_CoinItem = null;
    //--- ���� ������ ���� ����

    //--- ��Ʈ ������ ���� ����
    GameObject m_HeartItem = null;
    //--- ��Ʈ ������ ���� ����

    [Header("--- Skill Coll Timer ---")]
    public Transform m_SkillCoolRoot = null;
    public GameObject m_SkCollNode = null;


    //## Inventory
    [Header("Inventory IsShow")]
    public Button m_Inven_Btn = null;
    public Transform m_InvenRoot = null;
    Transform m_ArrowIcon = null;
    bool m_IsInvenShow = true;
    float m_ScSpeed = 1600.0f;
    Vector3 m_ScOnPos = new Vector3(-170.0f, 0.0f, 0.0f);
    Vector3 m_ScOffPos = new Vector3(-572.0f, 0.0f, 0.0f);
  


    HeroCtrl m_RefHero = null;

    //--- �̱��� ����
    public static Game_Mgr Inst = null;

    void Awake()
    {
        Inst = this;
    }
    //--- �̱��� ����

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f; //���� �ӵ���...


        GlobalValue.LoadGameData();


        InitRefreshUI();


        RefreshSkillList();

        if (GoLobbyBtn != null)
            GoLobbyBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbyScene");
            });

        if (m_Inven_Btn != null)
        {
            m_ArrowIcon  = m_Inven_Btn.transform.Find("Arrow_Icon");
            m_Inven_Btn.onClick.AddListener(() =>
            {
                m_IsInvenShow = !m_IsInvenShow;
            });
        }

        m_CoinItem = Resources.Load("CoinPrefab") as GameObject;
        m_HeartItem = Resources.Load("HeartPrefab") as GameObject;

        m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();    
    }

    // Update is called once per frame
    void Update()
    {
        //--- ����Ű �̿����� ��ų ����ϱ�
        if(Input.GetKeyDown(KeyCode.Alpha1) ||
           Input.GetKeyDown(KeyCode.Keypad1))
        {
            UseSkill_Key(SkillType.Skill_0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Keypad2))
        {
            UseSkill_Key(SkillType.Skill_1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) ||
            Input.GetKeyDown(KeyCode.Keypad3))
        {
            UseSkill_Key(SkillType.Skill_2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) ||
                Input.GetKeyDown(KeyCode.Keypad4))
        {
            UseSkill_Key(SkillType.Skill_3);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5) ||
             Input.GetKeyDown(KeyCode.Keypad5))
        {
            UseSkill_Key(SkillType.Skill_4);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha6) ||
             Input.GetKeyDown(KeyCode.Keypad6))
        {
            UseSkill_Key(SkillType.Skill_5);
        }

        ScOnOffUpdate();
    }

    public void DamageText(float a_Value, Vector3 a_Pos, Color a_Color)
    {
        if(Damage_Canvas == null || DmgTxtRoot == null) 
            return;

        m_DmgClone = Instantiate(DmgTxtRoot);
        m_DmgClone.transform.SetParent(Damage_Canvas);
        m_DmgTxt = m_DmgClone.GetComponent<DmgTxt_Ctrl>();
        if (m_DmgTxt != null)
            m_DmgTxt.InitDamage(a_Value, a_Color);
        m_StCacPos = new Vector3(a_Pos.x, a_Pos.y + 1.14f, 0.0f);
        m_DmgClone.transform.position = m_StCacPos;
    }

    public void SpawnCoin(Vector3 a_Pos, int a_Value = 10)
    {
        if(m_CoinItem == null)
            return;

        GameObject a_CoinObj = Instantiate(m_CoinItem);
        a_CoinObj.transform.position = a_Pos;
        Coin_Ctrl a_CoinCtrl = a_CoinObj.GetComponent<Coin_Ctrl>();
        if (a_CoinCtrl != null)
            a_CoinCtrl.m_RefHero = m_RefHero;
    }

    public void SpawnHeart(Vector3 a_Pos)
    {
        if (m_HeartItem == null)
            return;

        GameObject a_HeartObj = Instantiate(m_HeartItem);
        a_Pos.z = 0.0f;
        a_HeartObj.transform.position = a_Pos;
    }

    void UseSkill_Key(SkillType a_SkType)
    {
        //## ��ų �����Ǿ��� ��� ��� �Ұ�
        if (GlobalValue.g_CurSkillCount[(int)a_SkType] <= 0)
            return;


        if(m_RefHero == null)
            return;

        m_RefHero.UseSkill(a_SkType);

        //## ��ų ������ ���� UI����
        
        if(m_InvenRoot == null)
            return;
        
        SkInvenNode[] a_SKIvnNodes = m_InvenRoot.GetComponentsInChildren<SkInvenNode>();
        for (int i = 0; i < a_SKIvnNodes.Length; i++)
        {

            if (a_SKIvnNodes[i].m_SkType == a_SkType)
            {
                a_SKIvnNodes[i].Refresh_UI(a_SkType);
                break;
            }


        }


    }

    public void SkillCoolMethod(SkillType a_SkType, float a_Time, float a_During)
    {
        GameObject a_Obj = Instantiate(m_SkCollNode);
        a_Obj.transform.SetParent(m_SkillCoolRoot, false);
        //�ι�° �Ű����� worldPositionStays (�⺻���� true)

        SkillCool_Ctrl a_SCtrl = a_Obj.GetComponent<SkillCool_Ctrl>();  
        if(a_SCtrl != null)
            a_SCtrl.InitState(a_SkType, a_Time, a_During);
    }

    void ScOnOffUpdate()
    {
        if(m_InvenRoot == null)
            return;

        if(Input.GetKeyDown(KeyCode.R) == true)
            m_IsInvenShow = !m_IsInvenShow;

        if(m_IsInvenShow == false)
        {
            if(m_InvenRoot.localPosition.x > m_ScOffPos.x)
                m_InvenRoot.localPosition = 
                    Vector3.MoveTowards(m_InvenRoot.localPosition, 
                                        m_ScOffPos,m_ScSpeed * Time.deltaTime);


            if(m_ScOffPos.x <= m_InvenRoot.localPosition.x)
            {
                m_ArrowIcon.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);  
            }  
          



        }

        else
        {
            if(m_ScOnPos.x > m_InvenRoot.localPosition.x)
              m_InvenRoot.localPosition = 
                    Vector3.MoveTowards(m_InvenRoot.localPosition , m_ScOnPos, 
                    m_ScSpeed * Time.deltaTime);

            if (m_InvenRoot.localPosition.x <= m_ScOnPos.x)
            {
                m_ArrowIcon.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
            }
          
        
        
        }


    }
    public void AddScore(int a_Value = 10)
    {
        if(m_CurScore <= int.MaxValue - a_Value)
            m_CurScore += a_Value;
        else
            m_CurScore = int.MaxValue;

        if(m_CurScore < 0)
            m_CurScore = 0;
       
        
        m_CurScoreText.text = "���� ����( "+ m_CurScore + ")";

        if(GlobalValue.g_BestScore < m_CurScore)
        {
            GlobalValue.g_BestScore = m_CurScore;
            m_BestScoreText.text = "�ְ� ����( " + GlobalValue.g_BestScore + ")";
            PlayerPrefs.SetInt("BestScore", GlobalValue.g_BestScore);
        }
        


    }
    public void AddGold(int a_Value = 10 ) 
    {
        //��� ȹ��� ���� ������������ ���� ��尪�� �����ش�.
        if(m_CurGold <= int.MaxValue - a_Value)
                       m_CurGold += a_Value;
        else
            m_CurGold = int.MaxValue;
    

        //��� ȹ��� (����) ���� ��尪���� �����ش�.

        if(GlobalValue.g_UserGold <= int.MaxValue - a_Value)
            GlobalValue.g_UserGold += a_Value;
        else
            GlobalValue.g_UserGold = int.MaxValue;
    
        //��� ȹ��� UI�� ǥ��
        m_GoldText.text = "���� ���( " + GlobalValue.g_UserGold + ")";

        //��� ȹ��� ���� ��尪�� ����
        PlayerPrefs.SetInt("UserGold", GlobalValue.g_UserGold);


    }
    public void DecreaseGold(int a_Value = 10)
    {
        //��� ���ҽ� ���� ������������ ���� ��尪���� ���ش�.
        if (m_CurGold >= a_Value)
            m_CurGold -= a_Value;
        else
            m_CurGold = 0;

        //��� ���ҽ� (����) ���� ��尪������ ���ش�.
        if (GlobalValue.g_UserGold >= a_Value)
            GlobalValue.g_UserGold -= a_Value;
        else
            GlobalValue.g_UserGold = 0;

        //��� ���ҽ� UI�� ǥ��
        m_GoldText.text = "���� ���( " + GlobalValue.g_UserGold + ")";

        //��� ���ҽ� ���� ��尪�� ����
        PlayerPrefs.SetInt("UserGold", GlobalValue.g_UserGold);
    }




    //## UI �ʱ�ȭ
    void InitRefreshUI()
    {
        if(m_BestScoreText != null)
            m_BestScoreText.text = "�ְ� ����( " + GlobalValue.g_BestScore + ")";

        if(m_CurScoreText != null)
            m_CurScoreText.text = "���� ����( " + m_CurScore + ")";

        if(m_GoldText != null)
            m_GoldText.text = "���� ���( " + GlobalValue.g_UserGold + ")";

        if(m_UserInfoText != null)
            m_UserInfoText.text = "������ : �г���( " + GlobalValue.g_NickName + ")";



    }

    //## ���� ��ų ������ ����� UI�� ����
    void RefreshSkillList()
    {
        SkInvenNode[] a_SKIvnNodes = m_InvenRoot.GetComponentsInChildren<SkInvenNode>();
        for(int i = 0; i < a_SKIvnNodes.Length; i++)
        {
           if(i > GlobalValue.g_SkDataList.Count)
               continue;

            a_SKIvnNodes[i].InitState((SkillType)i);



           
        }
    }

}
