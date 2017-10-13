using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HighlightableObject : MonoBehaviour
{
    public GameObject[] Objects;
    public Material material;
    public int index;
    private CommandBuffer buffer;
    private RenderTargetIdentifier rtd;
    private RenderTexture rt;

    // Use this for initialization
    void Start()
    {

    }

    private void CreateBuffer()
    {
        rt = new RenderTexture(Screen.width, Screen.height, 0);
        rtd = new RenderTargetIdentifier(rt);

        buffer = new CommandBuffer();
    }

    private void RenderHighlights()
    {
        buffer.SetRenderTarget(rtd);

        foreach(GameObject o in Objects)
        {
            Renderer render = o.GetComponent<Renderer>();
            buffer.DrawRenderer(render, material, index);
        }

        RenderTexture.active = rt;
        Graphics.ExecuteCommandBuffer(buffer);
        RenderTexture.active = null;
    }

    private void ClearCommandBuffers()
    {
        RenderTexture.active = rt;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        ClearCommandBuffers();

        RenderHighlights();

        RenderTexture rt1 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
        material.SetTexture("_OccludeMap", rt);
        Graphics.Blit(rt1, rt1, material, 0);
        material.SetTexture("_OccludeMap", rt1);
        Graphics.Blit(source, destination, material, 1);

        RenderTexture.ReleaseTemporary(rt1);
    }
}
