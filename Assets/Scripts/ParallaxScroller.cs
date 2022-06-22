using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public Renderer[] layers;
    public float[] speedMultiplier;
    private float prevXPosMario;
    private float prevXPosCam;
    public FloatVariable marioXPos;
    public FloatVariable camXPos;
    private float[] offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new float[layers.Length];
        for(int i = 0; i < layers.Length; i++)
        {
            offset[i] = 0.0f;
        }
        prevXPosMario = marioXPos.value;
        prevXPosCam = camXPos.value;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(prevXPosCam - camXPos.value) > 0.001f)
        {
            for(int i = 0; i < layers.Length; i++)
            {
                if(offset[i] > 1f || offset[i] < -1f)
                {
                    offset[i] = 0f;
                }
                float newOffset = marioXPos.value - prevXPosMario;
                offset[i] = offset[i] + newOffset * speedMultiplier[i];
                layers[i].material.mainTextureOffset = new Vector2(offset[i],0);
            }
            prevXPosMario = marioXPos.value;
            prevXPosCam = camXPos.value;
        }
    }
}
