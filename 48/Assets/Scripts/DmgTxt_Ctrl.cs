using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DmgTxt_Ctrl : MonoBehaviour
{
    float m_EffTime = 0.0f;     //���� �ð� ���� ����
    public Text DamageText = null;  //Text UI ���ٿ� ����

    //�ӵ� = �Ÿ� / �ð�
    float MvVelocity = 1.1f / 1.05f;        //1.05�� ���ȿ� 1.1m ���ٴ� ... �ӵ�
    float ApVelocity = 1.0f / (1.0f - 0.4f);
    //alpha 0.4�ʺ��� 1.0�ʱ��� (0.6�� ����) : 0.0 -> 1.0 ��ȭ�ϴ� �ӵ�

    Vector3 m_CurPos;       //��ġ ���� ����
    Color m_Color;          //���� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_EffTime += Time.deltaTime;

        if(m_EffTime < 1.05f)
        {
            m_CurPos = DamageText.transform.position;
            m_CurPos.y += Time.deltaTime * MvVelocity;
            DamageText.transform.position = m_CurPos;
        }

        if(0.4f < m_EffTime)
        {
            m_Color = DamageText.color;
            m_Color.a -= (Time.deltaTime * ApVelocity);
            if(m_Color.a < 0.0f)
                m_Color.a = 0.0f;
            DamageText.color = m_Color;
        }

        if(1.05f < m_EffTime)
        {
            Destroy(gameObject);    
        }

    }//void Update()

    public void InitDamage(float a_Damage, Color a_Color)
    {
        if (DamageText == null)
            DamageText = this.GetComponentInChildren<Text>();

        if(a_Damage <= 0.0f)
        {
            int a_Dmg = (int)Mathf.Abs(a_Damage);   //���밪 �Լ�
            DamageText.text = "- " + a_Dmg;
        }
        else
        {
            DamageText.text = "+ " + (int)a_Damage;
        }

        a_Color.a = 1.0f;
        DamageText.color = a_Color;
    }
}
