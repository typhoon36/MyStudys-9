using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store_Mgr : MonoBehaviour
{
    public Button BackBtn;

    //## ����ǥ��
    public Text m_UserInfoText = null;


    public GameObject m_Item_ScContent;
    //��ũ�Ѻ� ������ ���ϵ�� ������ �θ� ������Ʈ

    public GameObject m_SkProduct_Node;

    SkProduct_Node[] m_SkNodList;
    //������ ��ϵ�

    //## dialog
    public GameObject Panel;

    public Text Help_Txt;
    public Text Title_Txt;


    public Button Confirm_Btn;
    public Button Cancel_Btn;
    public Button Close_Btn;

    // Add a new field to store the selected skill type
    //public SkillType SelectedSkillType { get; set; }

    // Start is called before the first frame update
    void Start()
    {

       

        GlobalValue.LoadGameData();



        if (BackBtn != null)
            BackBtn.onClick.AddListener(BackBtnClick);

        if(Close_Btn != null)
        Close_Btn.onClick.AddListener(() =>
        {
            Panel.SetActive(false);
        });
      



        if (Cancel_Btn != null)
            Cancel_Btn.onClick.AddListener(() =>
            {
                Panel.SetActive(false);
            });



        if (Confirm_Btn != null)
        {
            Confirm_Btn.onClick.AddListener(() =>
            {
                Panel.SetActive(false);
            });
        }


        if (m_UserInfoText != null)
            m_UserInfoText.text = "�г��� ( " + GlobalValue.g_NickName + ") : ���� ��� ( " +
                GlobalValue.g_UserGold+ " ) ";

        //## ������ ��� ����
        GameObject a_ItemObj = null;
        SkProduct_Node a_SkNode = null;

        for (int i = 0; i < GlobalValue.g_SkDataList.Count; i++)
        {
            a_ItemObj = Instantiate(m_SkProduct_Node, m_Item_ScContent.transform);
            a_SkNode = a_ItemObj.GetComponent<SkProduct_Node>();
            a_SkNode.InitData(GlobalValue.g_SkDataList[i].m_SkType);
            a_ItemObj.transform.SetParent(m_Item_ScContent.transform, false);


            
        }

        //## ������ ��� ����
        RefreshSKItemList();


    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void BackBtnClick()
    {
        SceneManager.LoadScene("LobbyScene");
    }


 
    //## ������ ��� ����
    void RefreshSKItemList()
    {
        if (m_Item_ScContent != null)
        {
            if (m_SkNodList == null || m_SkNodList.Length <= 0)
            {
                m_SkNodList = m_Item_ScContent.GetComponentsInChildren<SkProduct_Node>();
            }
        }

        for (int i = 0; i < m_SkNodList.Length; i++)
        {
            m_SkNodList[i].RefreshState();
        }



    }




    public void BuySkill(SkillType SkillType)
    {
        //## ���� ���� ���� Ȯ��
        if (GlobalValue.g_UserGold < GlobalValue.g_SkDataList[(int)SkillType].m_Price)
        {
            Title_Txt.text = "���";
            Help_Txt.text = "��尡 �����մϴ�.";
            return;
        }

        if (GlobalValue.g_CurSkillCount[(int)SkillType] >= 5)
        {
            Title_Txt.text = "���";
            Help_Txt.text = "��ų�� 5������ �� ���� ������ �� �����ϴ�.";
            return;
        }

        //## ���� ó��
        GlobalValue.g_UserGold -= GlobalValue.g_SkDataList[(int)SkillType].m_Price;
        GlobalValue.g_CurSkillCount[(int)SkillType]++;
        m_UserInfoText.text = "�г��� ( " + GlobalValue.g_NickName + ") : ���� ��� ( " +
            GlobalValue.g_UserGold + " ) ";
        RefreshSKItemList();
        Title_Txt.text = "���� ����";
        Help_Txt.text = "��ų�� ���������� �����߽��ϴ�.";
    }




}
