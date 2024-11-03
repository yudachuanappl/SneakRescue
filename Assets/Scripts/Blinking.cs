using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    public float BlinkingLength;
    public bool Stop;
    SpriteRenderer sr;
    bool isFadeIn;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        isFadeIn = true;
        Stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop) return;
        if(isFadeIn)
        {
            Color co = sr.color;
            co.a -= Time.deltaTime / BlinkingLength;
            sr.color = co;
            if(co.a<=0)
            {
                isFadeIn = false;
            }
        }
        else
        {
            Color co = sr.color;
            co.a += Time.deltaTime / BlinkingLength;
            sr.color = co;
            if (co.a >= 1)
            {
                isFadeIn = true;
            }
        }
    }

    public void StopBlinking()
    {
        Stop = true;
        Color co = sr.color;
        co.a = 1;
        sr.color = co;
    }
}
