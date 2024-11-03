using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject Light;

    private bool LightOn;
    // Start is called before the first frame update
    void Start()
    {
        Light.SetActive(false);
        LightOn = false;
    }

    public void HandleLightOn()
    {
        LightOn = true;
        Light.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(LightOn)
        {
            Light.transform.Rotate(new Vector3(0, 0, 360 * Time.deltaTime));
        }
    }
}
