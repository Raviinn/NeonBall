using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    public float movSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    private GameObject cat;
    private GameObject player;
    private bool isFeeding;
    private bool isPlaying;
    private bool isPetting;
    private Text playerInsText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = rb.gameObject;
        isFeeding = false;
        isPetting = false;  
        isPlaying = false;
        playerInsText = GameObject.Find("Canvas/PlayerInsText").GetComponent<Text>();
        playerInsText.text = null;
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

        if (inputX < 0)
        {
            spriteRenderer.flipX = false; 
        }
        else if (inputX > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SetLayerOrder()
    {
        if (cat != null)
        {
            if (player.GetComponent<BoxCollider2D>().transform.position.y > cat.GetComponent<BoxCollider2D>().transform.position.y)
            {
                player.GetComponent<SpriteRenderer>().sortingOrder = 1;
                cat.GetComponent<SpriteRenderer>().sortingOrder = 2;

            }
            else if (player.GetComponent<BoxCollider2D>().transform.position.y < cat.GetComponent<BoxCollider2D>().transform.position.y)
            {
                player.GetComponent<SpriteRenderer>().sortingOrder = 2;
                cat.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
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
                playerInsText.text = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))//To Play
        {
            if (isPlaying)
            {
                isPlaying = false;
                cat.GetComponent<CatController>().isPlayerCorrectResponse = true;
                playerInsText.text = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))//To Play
        {
            if (isPetting)
            {
                isPetting = false;
                cat.GetComponent<CatController>().isPlayerCorrectResponse = true;
                playerInsText.text = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CircleCollider2D>() != null)
        {
            if (collision.tag == "Cat")
            {
                cat = collision.gameObject;
                if (cat.GetComponent<CatController>().catAction == "Feed")
                {
                    //Set boolean for Feed
                    isFeeding = true;
                    isPetting = false;
                    isPlaying = false;
                    UpdatePlayerCaptionPos("Press Z to feed Cat");


                }
                else if (cat.GetComponent<CatController>().catAction == "Give a Toy")
                {
                    //Set boolean for Play
                    isPlaying = true;
                    isFeeding = false;
                    isPetting = false;
                    UpdatePlayerCaptionPos("Press X to play with Cat");

                }
                else if (cat.GetComponent<CatController>().catAction == "Pet")
                {
                    //Set boolean for Pet
                    isPetting = true;
                    isPlaying = false;
                    isFeeding = false;
                    UpdatePlayerCaptionPos("Press C to Pet Cat");

                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cat")
        {
            cat = null;
            playerInsText.text= null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<CircleCollider2D>() != null)
        {
            if (collision.tag == "Cat")
            {
                cat = collision.gameObject;
                if (cat.GetComponent<CatController>().catAction == "Feed")
                {
                    //Set boolean for Feed
                    isFeeding = true;
                    isPetting = false;
                    isPlaying = false;
                    UpdatePlayerCaptionPos("Press Z to feed Cat");


                }
                else if (cat.GetComponent<CatController>().catAction == "Give a Toy")
                {
                    //Set boolean for Play
                    isPlaying = true;
                    isFeeding = false;
                    isPetting = false;
                    UpdatePlayerCaptionPos("Press X to play with Cat");

                }
                else if (cat.GetComponent<CatController>().catAction == "Pet")
                {
                    //Set boolean for Pet
                    isPetting = true;
                    isPlaying = false;
                    isFeeding = false;
                    UpdatePlayerCaptionPos("Press C to Pet Cat");

                }
            }
            else
                playerInsText.text = null;
        }
    }

    private void UpdatePlayerCaptionPos(string action)//update caption pos on top of cat
    {
        if (playerInsText.text != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
            screenPos.y += 180;
            screenPos.x += 30;
            playerInsText.transform.position = screenPos;
            playerInsText.text = action;
        }
    }
}
