using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSpin : MonoBehaviour
{

    float _speed = 0.4f;
     
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * _speed);
        //To set the speed, just multiply Time.time with whatever amount you want.
    }
}
