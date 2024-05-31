using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Ctrl : MonoBehaviour
{ 
    float m_MoveSpeed = 9.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=  Vector3.right * m_MoveSpeed * Time.deltaTime;

        if(CameraResolution.m_ScWMax.x + 0.5f < transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            Monster_Ctrl a_Enermy  = other.gameObject.GetComponent<Monster_Ctrl>();
            if(a_Enermy != null)
            {
                a_Enermy.TakeDamage(700);
            }
        }

        else if(other.tag == "EnermyBullt")
        {
            Destroy(other.gameObject);
        }


    }


}
