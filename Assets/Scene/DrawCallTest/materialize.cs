using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialize : MonoBehaviour
{
    public Color color = Color.white;

    private MaterialPropertyBlock materialPropertyBlock = null;
    private Renderer renderer = null;
    static readonly int id = Shader.PropertyToID("_Color");

    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        materialPropertyBlock.SetColor(id, color);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }
}
