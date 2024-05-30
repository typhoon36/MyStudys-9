using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Ctrl : MonoBehaviour
{

    [HideInInspector] public HeroCtrl m_RefHero = null;

    public float speed = 5f;
    public float bounciness = 0.5f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<CircleCollider2D>();
        }

        PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D();
        bouncyMaterial.bounciness = bounciness;
        col.sharedMaterial = bouncyMaterial;

        rb.velocity = new Vector2(-speed, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Check if the heart collided with the screen bounds
        if (coll.gameObject.name == "ScreenBoundary")
        {
            Vector2 direction = new Vector2(-rb.velocity.x, -rb.velocity.y);
            rb.velocity = direction * speed;
        }
    }




    void Update()
    {
        // Check if the heart is near the screen bounds
        if (transform.position.x <= CameraResolution.m_ScWMin.x)
        {
            Vector2 direction = new Vector2(1, rb.velocity.y);
            rb.velocity = direction * speed;
        }
        else if (transform.position.x >= CameraResolution.m_ScWMax.x)
        {
            Vector2 direction = new Vector2(-1, rb.velocity.y);
            rb.velocity = direction * speed;
        }

        if (transform.position.y <= CameraResolution.m_ScWMin.y)
        {
            Vector2 direction = new Vector2(rb.velocity.x, 1);
            rb.velocity = direction * speed;
        }
        else if (transform.position.y >= CameraResolution.m_ScWMax.y)
        {
            Vector2 direction = new Vector2(rb.velocity.x, -1);
            rb.velocity = direction * speed;
        }

        // Destroy the heart after 10 seconds
        Destroy(gameObject, 10f);
    }
}
