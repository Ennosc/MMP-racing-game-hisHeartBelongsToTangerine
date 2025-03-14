using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    // Start is called before the first frame update
    private UIManager uiManager;
    private Checkpoints checkpoints;
    void Start()
    {
        uiManager = gameObject.GetComponent<UIManager>();
        checkpoints = gameObject.GetComponent<Checkpoints>();
    }

    // Update is called once per frame
    void Update()
    {
        calculatePosition();
    }

    private void calculatePosition()
    {
        
    }

}
