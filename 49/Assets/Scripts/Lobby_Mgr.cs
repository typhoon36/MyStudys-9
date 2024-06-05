using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby_Mgr : MonoBehaviour
{

    public Button m_ClearSvData_Btn;


    public Button Store_Btn;
    public Button MyRoom_Btn;
    public Button Exit_Btn;
    public Button GameStart_Btn;


    //## �ؽ�Ʈ
    public Text m_GoldText;
    public Text m_UserInfoText;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        GlobalValue.LoadGameData();


        if (m_ClearSvData_Btn != null)
            m_ClearSvData_Btn.onClick.AddListener(ClearSvData);



        if (Store_Btn != null)
            Store_Btn.onClick.AddListener(StoreBtnClick);

        if (MyRoom_Btn != null)
            MyRoom_Btn.onClick.AddListener(MyRoomBtnClick);

        if (Exit_Btn != null)
            Exit_Btn.onClick.AddListener(ExitBtnClick);

        if (GameStart_Btn != null)
            GameStart_Btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });


        if (m_GoldText != null)
            m_GoldText.text = GlobalValue.g_UserGold.ToString("N0");



        if (m_UserInfoText != null)
            m_UserInfoText.text = "�� ���� : �г��� ( " + GlobalValue.g_NickName + ") : ���� ("
                + GlobalValue.g_BestScore + ")";



    }

    void ClearSvData()
    {
        //���� ������ ����
        PlayerPrefs.DeleteAll();

        //��ų ī��Ʈ�� ����
        GlobalValue.g_CurSkillCount.Clear();

        //�ٽ� �ε�
        GlobalValue.LoadGameData();

        if(m_GoldText != null)
            m_GoldText.text = GlobalValue.g_UserGold.ToString("N0");



        if (m_UserInfoText != null)
            m_UserInfoText.text = "�� ���� : �г��� ( " + GlobalValue.g_NickName + ") : ���� ("
                + GlobalValue.g_BestScore + ")";




    }





    private void StoreBtnClick()
    {
        //Debug.Log("�������� ���� ��ư Ŭ��");
        SceneManager.LoadScene("StoreScene");
    }

    private void MyRoomBtnClick()
    {
        //Debug.Log("�ٹ̱� �� ���� ��ư Ŭ��");
        SceneManager.LoadScene("MyRoomScene");
    }

    private void ExitBtnClick()
    {
        Debug.Log("Ÿ��Ʋ ������ ������ ��ư Ŭ��");
        SceneManager.LoadScene("TitleScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
