using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMoveScript : MonoBehaviour
{
    [SerializeField] public Sprite cow1;
    [SerializeField] public Sprite cow2;
    [SerializeField] public Sprite cow3;
    [SerializeField] public Sprite cow4;
    private Rigidbody2D rb;
    public bool coweating = false;
    public float cowspeed;
    public int moveCounter = 0;
    public int eatCounter = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("changeSprite", 0, 1);
        InvokeRepeating("moveCow", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void moveCow()
    {
     
        if (coweating == false)
        {
            if (moveCounter < 10)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                rb.velocity = new Vector2(cowspeed, 0f);
                moveCounter++;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                rb.velocity = new Vector2(-cowspeed, 0f);
                moveCounter++;
            }
            if (moveCounter == 20)
            {
                moveCounter = 0;
            }
        }
        if (moveCounter == 10 && coweating == false)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cow3;
            print("cow now eating");
            coweating = true;
            resetVelocity();
        }
        if (coweating == true) { 
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "cow3")
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = cow4;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = cow3;
                eatCounter++;
            };
            if(eatCounter == 10)
            {
                coweating = false;
                    eatCounter = 0;
            }
           }
        }
    }

    private void resetVelocity()
    {
        rb.Sleep();
    }
    private void changeSprite()
    {
        if (coweating == false)
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "cow1")
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = cow2;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = cow1;
            };
        }
    }
}
