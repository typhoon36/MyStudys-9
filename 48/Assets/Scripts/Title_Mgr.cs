using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_Mgr : MonoBehaviour
{
    public Button StartBtn;

    // Start is called before the first frame update
    void Start()
    {
        StartBtn.onClick.AddListener(StartClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartClick()
    {
        Debug.Log("��ư�� Ŭ�� �߾��.");
        SceneManager.LoadScene("LobbyScene");
    }
}
