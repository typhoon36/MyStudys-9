using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DmgTxt_Ctrl : MonoBehaviour
{
    //## ����ð� ��� ����
    float m_EffTime = 0.0f;

    
    public Text DamageText = null;

    //## �̵� ����
    float MvVel = 1.1f/ 1.05f; //�̵� �ӵ�
    float ApVel = 1.0f / (1.0f - 0.4f); //���� ��ȭ �ӵ�
    //���İ��� 0.4�ʺ��� 1.0�ʱ��� 0.6�� ���� ��ȭ�ϵ��� ����

    Vector3 m_CurPos;   //���� ��ġ

    Color m_Color;  //�÷��� ����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_EffTime += Time.deltaTime;

        if(m_EffTime > 1.05f)
        {
            m_CurPos = DamageText.transform.position;
            m_CurPos.y += Time.deltaTime * MvVel;
            DamageText.transform.position = m_CurPos;
        }

        if (0.4f < m_EffTime)
        {
            m_Color = DamageText.color;
            m_Color.a -= Time.deltaTime * ApVel;

            if(m_Color.a < 0.0f)
                m_Color.a = 0.0f;
                DamageText.color = m_Color;
        }

        if(1.05f < m_EffTime)
            Destroy(gameObject);   

     



    }

    public void InitDamage(float a_Damage, Color a_Color)
    {
        if(DamageText == null)
            DamageText = GetComponentInChildren<Text>();

        if (a_Damage <= 0.0f)
        {
            int a_Dmg = (int)Mathf.Abs(a_Damage);
            DamageText.text = "-" + a_Dmg;
        }
        else
        {
            DamageText.text = " +" + (int)a_Damage;
        }

        a_Color.a = 1.0f;
        DamageText.color = a_Color;


    }


}
