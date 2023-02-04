using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    private List<Material> materials = new List<Material>();
    public Material HitMaterial;
    public Animation HitAnimation;

    [Range(0f,1f)]
    public float alpha;

    private static readonly int Color1 = Shader.PropertyToID("_Color");

    // Start is called before the first frame update
    void Start()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            for (var index = 0; index < renderer.sharedMaterials.Length; index++)
            {
                var sharedMaterial = renderer.sharedMaterials[index];
                if (sharedMaterial.name == HitMaterial.name)
                {
                    materials.Add(renderer.materials[index]);
                }
            }
        }
    }

    public void PlayAnimation()
    {
        HitAnimation.Rewind();
        HitAnimation.Play();
    }

    private void Update()
    {
        foreach (var mat in materials)
        {
            Color color = mat.GetColor(Color1);
            color.a = alpha;
            mat.SetColor(Color1, color);
        }
    }
}
