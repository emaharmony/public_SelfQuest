/**
 * Will have to add a time value to the ping pong but works well
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PulsateLight : MonoBehaviour
{
    Light light;
    float timer;
    bool direction = true;


    [SerializeField] float maxIntensity, minIntensity, maxRange, minRange, pingpongSpeed = 1;
    [SerializeField] bool doIntensity, doRange;

    // Start is called before the first frame update
    void Awake()
    {
        light = GetComponent<Light>();
    }

   // Update is called once per frame
    void Update()
    {

        if (doRange)
            light.range = PingPong(light.range, maxRange, minRange); ;

        if (doIntensity)
            light.intensity = PingPong(light.intensity, maxIntensity, minIntensity);

        timer += Time.deltaTime;
    }

    float PingPong(float val, float max, float min) 
    {
        float result = val;
        if (direction)
        {
            if (result < max)
                result += (Time.deltaTime * pingpongSpeed);
            else
                direction = false;
        }
        else 
        {
            if (result > min)
                result -= (Time.deltaTime * pingpongSpeed);
            else
                direction = true;
        }

        return result;
    }
}
