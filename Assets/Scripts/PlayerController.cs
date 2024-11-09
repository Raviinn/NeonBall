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
    private bool isFeeding;
    private bool isPlaying;
    private bool isPetting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = rb.gameObject;
        cat = GameObject.Find("Cat");
        isFeeding = false;
        isPetting = false;  
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SetLayerOrder();
        ChooseAction();
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
            player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            cat.GetComponent<SpriteRenderer>().sortingOrder = 2;

        }else if (player.GetComponent<BoxCollider2D>().transform.position.y < cat.GetComponent<BoxCollider2D>().transform.position.y)
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = 2;
            cat.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    private void ChooseAction()
    {
        if (Input.GetKeyDown(KeyCode.Z))//To Feed
        {
            if (isFeeding)
            {
                isFeeding = false;
                cat.GetComponent<CatController>().isPlayerCorrectResponse = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))//To Play
        {
            if (isPlaying)
            {
                isPlaying = false;
                cat.GetComponent<CatController>().isPlayerCorrectResponse = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))//To Play
        {
            if (isPetting)
            {
                isPetting = false;
                cat.GetComponent<CatController>().isPlayerCorrectResponse = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cat")
        {
            if (cat.GetComponent<CatController>().catAction == "Feed")
            {
                //Set boolean for Feed
                isFeeding = true;
                isPetting = false;
                isPlaying = false;
                

            }else if (cat.GetComponent<CatController>().catAction == "Play")
            {
                //Set boolean for Play
                isPlaying = true;
                isFeeding = false;
                isPetting = false;

            }
            else if (cat.GetComponent<CatController>().catAction == "Pet")
            {
                //Set boolean for Pet
                isPetting= true;
                isPlaying = false;
                isFeeding = false;

            }
        }
    }
}
