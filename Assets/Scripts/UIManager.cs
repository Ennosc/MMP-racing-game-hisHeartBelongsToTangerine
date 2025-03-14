using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image placeHolder;
    [SerializeField] private Sprite newItem;
    [SerializeField] private TMP_Text roundLapsText;
    [SerializeField] private TMP_Text placementText;
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject endScreen;

    [SerializeField] private GameObject endScreenNPC;
    [SerializeField] private Sprite placeHolderSprite;
    public bool itemInventory = false;

    private int laprounds = 1;

    public void itemChange()
    {
        Debug.Log("new image loaded");
        placeHolder.sprite = newItem;
    }

    public void itemUsed()
    {
        Debug.Log("item used");

    }

    public void loadPlceHolder()
    {
        Debug.Log("placeholder loaded");
        placeHolder.sprite = placeHolderSprite;
    }

    public void changeLapRound()
    {
        laprounds++;
        roundLapsText.text = "Laps " + System.Convert.ToString(laprounds) + "/3";
        if(laprounds == 4)
        {
            Debug.Log("Finished race");
            loadEndScreen();
        }
    }

    public void loadEndScreen()
    {
        Debug.Log("endscreen loaded");
        gameObject.SetActive(false);
        game.SetActive(false);
        endScreen.SetActive(true);
    }

    public void loadEndScreenNPC()
    {
        Debug.Log("endscreen loaded");
        gameObject.SetActive(false);
        game.SetActive(false);
        endScreenNPC.SetActive(true);
    }

    public void ChangePlacement(int placement) {
        switch(placement) {
            case 1: 
                placementText.text = "first";
                break;
            case 2: 
                placementText.text = "second";
                break;
            case 3: 
                placementText.text = "third";
                break;
            case 4: 
                placementText.text = "fourth";
                break;
            case 5: 
                placementText.text = "fifth";
                break;
            case 6: 
                placementText.text = "sixth";
                break;
            case 7: 
                placementText.text = "seventh";
                break;
            case 8: 
                placementText.text = "eighth";
                break;
        }
    }

    public bool getBool()
    {
        return itemInventory;
    }

    public void setBool(bool newBool)
    {
        Debug.Log("set bool true");
        itemInventory = newBool;
    }
}
