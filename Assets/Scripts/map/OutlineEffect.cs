using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineEffect : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor = 1.05f;
    [SerializeField] private Color outlineColor = Color.yellow;

    private Renderer outlineRenderer;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        outlineRenderer.enabled = false;
    }

    public void EnableOutline()
    {
        if (outlineRenderer != null)
        {
            outlineRenderer.enabled = true;
        }
    }

    public void DisableOutline()
    {
        if (outlineRenderer != null)
        {
            outlineRenderer.enabled = false;
        }
    }

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        // GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, Quaternion.Euler(0, 90, 0), transform);
        Renderer rend = outlineObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_Scale", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        outlineObject.GetComponent<OutlineEffect>().enabled = false;
        outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;
    }
}