using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    public MeshRenderer Renderer;

    public bool OnAwake;

    public Material[] Materials;

    void Awake()
    {
        if (OnAwake)
        {
            SetRandomMaterial();
        }
    }

    public void SetRandomMaterial()
    {
        Renderer.material = Materials[Random.Range(0, Materials.Length)];
    }
}
