using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        changeDirectionTimer = changeDirectionTime;
        isReadyForNextAction = true;
    }

    void Update()
    {
        Move();
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
        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0)
        {
            //Debug.Log("Changing Direction");
            PickRandomDirection();
            changeDirectionTimer = changeDirectionTime;
        }

        if (!isIdle)
        {
            // Move the enemy smoothly
            rb.velocity = Vector2.Lerp(rb.velocity, movementDirection * movSpeed, Time.deltaTime * 2f);

            // If the velocity is very close to zero, stop the movement and set to idle
            if (rb.velocity.magnitude < 0.01f)
            {
                rb.velocity = Vector2.zero;
                //animator.SetBool("is_walking", false);
            }
            else
            {
                //animator.SetBool("is_walking", true);

                // Flip the sprite based on the movement direction
                if (rb.velocity.x > 0)
                {
                    //mc.flipX = false;
                    //hair.flipX = false;
                }
                else if (rb.velocity.x < 0)
                {
                    //mc.flipX = true;
                    //hair.flipX = true;
                }
            }
        }
        else
        {
            // Instantly set the velocity to zero and switch to idle animation
            rb.velocity = Vector2.zero;
            //animator.SetBool("is_walking", false);
        }
    }

    public void ChooseAction()
    {
        int randomAction = Random.Range(0, 1000);
        if (randomAction < 3)
        {
            isReadyForNextAction = false;
            switch (randomAction)
            {
                case 0:
                    Debug.Log("Feed");
                    break;
                case 1:
                    Debug.Log("Play");
                    break;
                case 2:
                    Debug.Log("Pet");
                    break;
                default:
                    break;
            }
            StartCoroutine(HoldAction());
            Debug.Log("Done");
        }
    }

    private IEnumerator HoldAction()
    {
        yield return new WaitForSeconds(10f);
        isReadyForNextAction = true;
        StopAllCoroutines();
    }

}
