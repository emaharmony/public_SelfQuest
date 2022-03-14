using System;
using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class DayNightManager : MonoBehaviour
{
    public static DayNightManager INSTANCE { get; private set; }
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [SerializeField] Material  daySkyBox, nightSkyBox;
    [SerializeField] Color[] skyboxColors;
    [SerializeField] float skyboxSpeed;
    [SerializeField] Transform nightLightParent;

    Material current; float timer = 0;

    public bool EVENING { get; private set; }

    bool lightsOn = false;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Start()
    {
        current = RenderSettings.skybox;
    }

    private void Update()
    {
        SpinningCloud();

        if (Preset == null)
            return;
        if (Application.isPlaying)
        {
            #region Evening
            EVENING = TimeOfDay >= 17 || TimeOfDay < 8;
            if (EVENING && !lightsOn)
            {
                lightsOn = true;
                AuxLightsState();
            }
            else if (!EVENING && lightsOn)
            {
                lightsOn = false;
                AuxLightsState();
            }
            #endregion



#if UNITY_EDITOR
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime * .25f;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
#else
            //(Replace with a reference to the game time)
            TimeOfDay = DateTime.Now.Hour;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
#endif

            #region Skybox
            if (Mathf.CeilToInt(TimeOfDay) >= 19 || Mathf.CeilToInt(TimeOfDay) < 5)
            {
                RenderSettings.skybox = nightSkyBox;
                nightSkyBox.color = skyboxColors[1];
            }
            else
            {
                RenderSettings.skybox = daySkyBox;
            }

          
            if (Mathf.CeilToInt(TimeOfDay) == 5)
            {
                RenderSettings.skybox.color = skyboxColors[0];
            }
            else if (Mathf.CeilToInt(TimeOfDay) == 8)
            {

                RenderSettings.skybox.color = skyboxColors[1];
            }
            else if (Mathf.CeilToInt(TimeOfDay) == 17)
            {
                RenderSettings.skybox.color = skyboxColors[2];

            }
            #endregion

        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    void AuxLightsState()
    {
        StartCoroutine(ChangeSecondaryLightState());
    }

    void LerpingSkybox( Material b)
    {
        if (timer >= 1)
        {
            RenderSettings.skybox = b;
            current = b;
            timer = 0;
            return;
        }
        RenderSettings.skybox.Lerp(current, b, timer);
        timer += Time.deltaTime * skyboxSpeed;
    }

    IEnumerator ChangeSecondaryLightState() 
    {
        for (int i = 0; i < nightLightParent.childCount; i++) 
        {
            nightLightParent.GetChild(i).gameObject.SetActive(lightsOn);
            yield return new WaitForSeconds(.1f);
        }
    }

    void SpinningCloud() 
    {
       // RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxSpeed);
    }
}