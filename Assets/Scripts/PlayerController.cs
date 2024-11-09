using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    private GameObject cat;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = rb.gameObject;
        cat = GameObject.Find("Cat");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SetLayerOrder();
    }

    public void PlayerMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(inputX, inputY).normalized * movSpeed;

        rb.velocity = movement;
    }

    public void SetLayerOrder()
    {
        if (player.GetComponent<BoxCollider2D>().transform.position.y > cat.GetComponent<BoxCollider2D>().transform.position.y)
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = 0;
            cat.GetComponent<SpriteRenderer>().sortingOrder = 1;

        }else if (player.GetComponent<BoxCollider2D>().transform.position.y < cat.GetComponent<BoxCollider2D>().transform.position.y)
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            cat.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }   
}
