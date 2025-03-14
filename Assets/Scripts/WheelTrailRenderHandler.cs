using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderHandler : MonoBehaviour
{
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;

    void Awake()
    {
        //Get car controller
        topDownCarController = GetComponentInParent<TopDownCarController>();

        //Get trail renderer
        trailRenderer = GetComponent<TrailRenderer>();

        //Set trail renderer
        trailRenderer.emitting = false;


    }


    // Update is called once per frame
    void Update()
    {
        //if yes, emitt trail
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
            trailRenderer.emitting = true;
        else trailRenderer.emitting = false;
    }
}
