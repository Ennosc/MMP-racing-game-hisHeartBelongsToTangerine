using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    UIManager uiManager;
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayer = collision.gameObject.CompareTag("Player");
        bool isNPC = collision.gameObject.CompareTag("NPC");
        if (!isPlayer && !isNPC)
            return;

        if (isPlayer)
        {
            StartCoroutine(PlaySoundAndDeactivate());
        } else {
            DeactivateItem();
        }
    }

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<UIManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator PlaySoundAndDeactivate()
    {
        DeactivateItem();
        audioSource.Play();
        Debug.Log("collected item");

        // Ensure the UIManager actions are done before deactivation
        uiManager.itemChange();
        uiManager.itemInventory = true;

        // Wait for the duration of the audio clip
        yield return new WaitForSeconds(audioSource.clip.length);
    }

    private void DeactivateItem() {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

}
