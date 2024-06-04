using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby_Mgr : MonoBehaviour
{
    public Button Store_Btn;
    public Button MyRoom_Btn;
    public Button Exit_Btn;
    public Button GameStart_Btn;

    // Start is called before the first frame update
    void Start()
    {
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
