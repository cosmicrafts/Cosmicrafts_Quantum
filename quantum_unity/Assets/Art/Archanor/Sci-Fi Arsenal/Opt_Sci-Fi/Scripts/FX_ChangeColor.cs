using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_ChangeColor : MonoBehaviour
{
    public ParticleSystemRenderer[] particleSystemsRenderers;
    public Color color = new Color(1,1,1,0.5f);

    void Start()
    {
        if (particleSystemsRenderers.Length == 0)
        {
            particleSystemsRenderers = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
        }
       
        UpdateColor();
    }

    public void UpdateColor()
    {
        foreach (var particleSystemRenderer in particleSystemsRenderers)
        {
            var material =  particleSystemRenderer.material;
            material.SetColor ("_TintColor", color);
            particleSystemRenderer.material = material;
        }
    }

}
