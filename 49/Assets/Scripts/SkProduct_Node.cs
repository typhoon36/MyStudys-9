using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkProduct_Node : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType = SkillType.SkCount;

    public Text m_CountText;
    public Image m_SkIconImg;
    public Text m_HelpTxt;
    public Text m_BuyTxt;



    // Start is called before the first frame update
    void Start()
    {
        //## 스킬 구매 버튼을 눌렀을경우
        Button a_BuyCom = GetComponentInChildren<Button>();

        if(a_BuyCom != null)
        {
            a_BuyCom.onClick.AddListener(() =>
            {
                
                Store_Mgr a_StoreMgr = GameObject.FindObjectOfType<Store_Mgr>();

                if(a_StoreMgr != null)
                {
                    
                    a_StoreMgr.Panel.SetActive(true);
                    a_StoreMgr.BuySkill(m_SkType);
                }

            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitData(SkillType a_SkType)
    {
        m_SkType = a_SkType;

        m_SkIconImg.sprite = GlobalValue.g_SkDataList[(int)a_SkType].m_IconImg;

        //## 사이즈 조정
        m_SkIconImg.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(GlobalValue.g_SkDataList[(int)a_SkType].m_IconSize.x * 135.0f,135.0f);

        //## 텍스트 설정
        m_HelpTxt.text = GlobalValue.g_SkDataList[(int)a_SkType].m_SkillExp;


    }

    public void RefreshState()
    {
        if(m_SkType < SkillType.Skill_0 || m_SkType >= SkillType.SkCount)
            return;
        
        Skill_Info a_RefSKInfo = GlobalValue.g_SkDataList[(int)m_SkType];

        if(a_RefSKInfo == null)
            return;

        m_CountText.text = GlobalValue.g_CurSkillCount[(int)m_SkType] + "/5";

        m_BuyTxt.text = a_RefSKInfo.m_Price + "G";


    }



}
