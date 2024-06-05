using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkInvenNode : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;

    [HideInInspector] public int m_CurSkCount = 0;


    public Text m_SkCountText;

    public Image m_SkIcon_Img;
    //0이 되었을때 알파를 변경할수 있는 변수





    // Start is called before the first frame update
    void Start()
    {
        Button a_BuyCom = GetComponentInChildren<Button>();
        if(a_BuyCom !=null)
        {  //## 버튼을 눌렀을시
            a_BuyCom.onClick.AddListener(() =>
            {
                if (GlobalValue.g_CurSkillCount[(int)m_SkType] <= 0)
                
                    return;
                

                HeroCtrl a_Hero = GameObject.FindObjectOfType<HeroCtrl>();

                if (a_Hero != null) 
                    a_Hero.UseSkill(m_SkType);

                Refresh_UI(m_SkType);

                
            });
        }



    }

    // Update is called once per frame
    void Update()
    {

      

    }

    public void InitState(SkillType a_SkType)
    {

        m_SkType = a_SkType;

        m_CurSkCount = GlobalValue.g_CurSkillCount[(int)a_SkType];

        m_SkCountText.text = m_CurSkCount.ToString();


        if (m_SkIcon_Img != null)
        {
            if (m_CurSkCount <=0)
            {
                m_SkIcon_Img.color = new Color32(255, 255, 255, 80);
            }
            else
            {
                m_SkIcon_Img.color = new Color32(255, 255, 255, 220);
            }

        }
    }


    public void Refresh_UI(SkillType a_SkType)
    {
        if(m_SkType != a_SkType)
            return;

        m_CurSkCount = GlobalValue.g_CurSkillCount[(int)a_SkType];
       if ( m_SkCountText!=null)
            m_SkCountText.text = m_CurSkCount.ToString();


        if (m_SkIcon_Img != null)
        {
            if (m_CurSkCount <=0)
            {
                m_SkIcon_Img.color = new Color32(255, 255, 255, 80);
            }
            else
            {
                m_SkIcon_Img.color = new Color32(255, 255, 255, 220);
            }

        }



    }


}
