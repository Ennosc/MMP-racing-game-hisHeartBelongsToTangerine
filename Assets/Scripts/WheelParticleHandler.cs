using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    float particleEmissionRate = 0;

    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;
    ParticleSystem.MainModule particleSystemMainModule;

    void Awake()
    {
        //get the components
        topDownCarController = GetComponentInParent<TopDownCarController>();

        particleSystemSmoke = GetComponent<ParticleSystem>();

        particleSystemEmissionModule = particleSystemSmoke.emission;

        particleSystemMainModule = particleSystemSmoke.main;

        //start with zero
        particleSystemEmissionModule.rateOverTime = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //reduce particle over time
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        //check what surface we on and apply different settings
        //switch (topDownCarController.GetSurface())
        //{
        //    case Surface.SurfaceTypes.Road:
        //        particleSystemMainModule.startColor = new Color(0.83f, 0.83f, 0.83f);
        //        break;
        //    case Surface.SurfaceTypes.Grass:
        //        particleEmissionRate = topDownCarController.GetVelocityMagnitude();
        //        particleSystemMainModule.startColor = new Color(1.0f, 0.0f, 0.0f);
        //        break;
        //    case Surface.SurfaceTypes.Oil:
        //        particleSystemMainModule.startColor = new Color(0.2f, 0.2f, 0.2f);
        //        break;

        //}

        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {  
            //drift is smoke , brake is more smoke
            if(isBraking)
                particleEmissionRate = 30;
            
            else particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
        }
    }
}
