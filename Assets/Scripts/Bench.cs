using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bench : MonoBehaviour
{
    [SerializeField] Sprite brokenBench;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isPlayer = collision.gameObject.CompareTag("Player");
        bool isNPC    = collision.gameObject.CompareTag("NPC");
        if (isPlayer || isNPC)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = brokenBench;
            Debug.Log("bench hit");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
