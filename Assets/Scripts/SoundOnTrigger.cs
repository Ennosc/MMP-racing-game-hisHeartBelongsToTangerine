using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnTrigger : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("GameController"))
        {
            
            audioSource.Play();
        }
        else
        {
            //Debug.Log("Collision with non-GameController object.");
        }
    }
}
