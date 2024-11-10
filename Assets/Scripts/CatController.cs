using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CatController : MonoBehaviour
{
    public float movSpeed = 2.0f;            // Movement speed
    public float changeDirectionTime = 3.0f; // Time interval to change direction
    public float idleProbability = 0.3f;     // Probability of staying idle (0.3 = 30%)

    private Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer mc;               // Main character sprite renderer
    public SpriteRenderer hair;             // Hair sprite renderer

    private Vector2 movementDirection;
    private float changeDirectionTimer;
    private bool isIdle;
    private float time = 0;
    private bool isReadyForNextAction;
    public string catAction;
    private UnityEngine.UI.Text catText;
    private GameObject cat;
    private float countResponseTime;
    public bool isPlayerCorrectResponse;
    private ScoreManager scoreManager;

    void Start()
    {
        changeDirectionTimer = changeDirectionTime;
        isReadyForNextAction = true;
        rb = GetComponent<Rigidbody2D>();
        cat = rb.gameObject;
        catText = cat.transform.Find("CatRequest/Canvas/CatRequestText").GetComponent<UnityEngine.UI.Text>();
        catText.color = Color.green;
        countResponseTime = 0f;
        isPlayerCorrectResponse = false;
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        UpdateCatCaptionPos();
        CheckResponseTime();
        if (isReadyForNextAction)
        {
           ChooseAction();
        }

    }

    void PickRandomDirection()
    {
        // Determine if the enemy should idle
        if (Random.value < idleProbability)
        {
            isIdle = true;
            movementDirection = Vector2.zero;
        }
        else
        {
            isIdle = false;

            // Randomize a larger distance and angle for the new position
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            float smallDistance = Random.Range(1f, 2f);  // Adjust these values for movement range
            // Calculate the new movement direction
            movementDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized * smallDistance;
        }
    }

    void Move()
    {
        
        if (changeDirectionTimer <= 0)
        {
                //Debug.Log("Change Direction");
                PickRandomDirection();
                changeDirectionTimer = changeDirectionTime;
        }

        if (!isIdle)
        {
            // Move the enemy smoothly
            rb.velocity = Vector2.Lerp(rb.velocity, movementDirection * movSpeed, Time.deltaTime * 1f);
            

            // If the velocity is very close to zero, stop the movement and set to idle
            if (rb.velocity.magnitude < 0.01f)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isWalking", true);

                // Flip the sprite based on the movement direction
                if (rb.velocity.x > 0)
                {
                    cat.GetComponent<SpriteRenderer>().flipX = true;
                    //mc.flipX = false;
                    //hair.flipX = false;
                }
                else if (rb.velocity.x < 0)
                {
                    cat.GetComponent<SpriteRenderer>().flipX = false;
                    //mc.flipX = true;
                    //hair.flipX = true;
                }
                
            }
        }
        else
        {
            // Instantly set the velocity to zero and switch to idle animation
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
        changeDirectionTimer -= Time.deltaTime;
    }

    public void ChooseAction()
    {
        catText.text = null;
        int randomAction = Random.Range(0, 1000);
        if (randomAction < 3)
        {
            isReadyForNextAction = false;
            switch (randomAction)
            {
                case 0:
                    Debug.Log("Feed");
                    catAction = "Feed";
                    break;
                case 1:
                    Debug.Log("Give a Toy");
                    catAction = "Give a Toy";
                    break;
                case 2:
                    Debug.Log("Pet");
                    catAction = "Pet";
                    break;
                default:
                    break;
            }
            StartCoroutine(HoldAction());
        }
    }

    private IEnumerator HoldAction()
    {
        yield return new WaitForSeconds(3f);
        StopAllCoroutines();
    }

    private void UpdateCatCaptionPos()//update caption pos on top of cat
    {
        if (catAction != null) {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(cat.transform.position);
            screenPos.y += 110;
            screenPos.x += 50;
            catText.transform.position = Vector2.Lerp(catText.transform.position, screenPos, Time.deltaTime * 20);
            catText.text = catAction;
        }
    }

    private void CheckResponseTime()
    {
        if (isPlayerCorrectResponse)
        {
            ConfirmPlayerScore();
            catAction = null;
            isReadyForNextAction = true;
            catText.color = Color.green;
            isPlayerCorrectResponse = false;
            countResponseTime = 0;
        }
        else if (!isReadyForNextAction)
        {
            countResponseTime += Time.deltaTime;
            if (countResponseTime >= 5f)
            {
                countResponseTime = 0;
                if (catText.color == Color.green)
                {
                    catText.color = Color.white;
                }else if (catText.color == Color.white)
                {
                    catText.color = Color.red;
                }
            }
        }
    }

    private void ConfirmPlayerScore()
    {
        if (catText.color == Color.green)
        {
            scoreManager.AddBonusPlayerScore();
        }else if (catText.color == Color.white)
        {
            scoreManager.AddPlayerScore();
        }
        else
        {
            scoreManager.MinusPlayerScore();
        }
    }


}
