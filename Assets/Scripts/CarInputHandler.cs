using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    //Components
    TopDownCarController topDownCarController;
    UIManager uiManager;
    CarSFXHandler carSFXHandler;

    //Awake is called when the script instance is being loaded
    void Awake() 
    {
        topDownCarController = GetComponent<TopDownCarController>();
        carSFXHandler = GetComponentInChildren<CarSFXHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        topDownCarController.SetInputVector(inputVector);

        if(Input.GetKeyDown("space") && uiManager.itemInventory )
        {
            Debug.Log("space key was pressed");
            topDownCarController.useRocketBooster();
            uiManager.itemInventory = false;
            uiManager.loadPlceHolder();
            carSFXHandler.PlayRocketBoostSound();
        }
    }

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<UIManager>();
    }
}
