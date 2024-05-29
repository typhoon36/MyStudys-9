using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Mgr : MonoBehaviour
{
    //## 캐릭터 머리 위에 데미지 띄우기 
    GameObject m_DmgClone = null;
    DmgTxt_Ctrl m_DmgCtrl;
    Vector3 m_StCacPos;
    [Header("##Damage_Text")]
    public Transform DmgCanvs = null;
    public GameObject DmgTxt_Root = null;


    //## 코인 아이템 
    GameObject m_CoinItem = null;

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
        m_CoinItem = Resources.Load("Coin_Prefab") as GameObject;

        m_RefHero = GameObject.FindObjectOfType<HeroCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageTxt (float a_Val,Vector3 a_Pos, Color a_Color)
    {
        if (DmgCanvs == null || DmgTxt_Root == null)
            return;

        m_DmgClone = Instantiate(DmgTxt_Root);
        m_DmgClone.transform.SetParent(DmgCanvs);

        m_DmgCtrl = m_DmgClone.GetComponent<DmgTxt_Ctrl>();

        if(m_DmgCtrl != null)
        m_DmgCtrl.InitDamage(a_Val, a_Color);

        m_StCacPos = new Vector3(a_Pos.x, a_Pos.y + 1.14f, 0.0f);
        m_DmgClone.transform.position = m_StCacPos;
    }

    public void SpawnCoin(Vector3 a_Pos, int a_Val = 10)
    {
        if(m_CoinItem == null)
        
            return;
        GameObject a_CoinObj = Instantiate(m_CoinItem);
        a_CoinObj.transform.position = a_Pos;
        Coin_Ctrl a_CoinCtrl = a_CoinObj.GetComponent<Coin_Ctrl>();
        if(a_CoinCtrl != null)
            a_CoinCtrl.m_RefHero = m_RefHero;



    }
}
