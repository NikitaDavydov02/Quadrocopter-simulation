using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : MonoBehaviour
{
    [Header("Appearance")]
    public Color lightColor = Color.white;
    [Range(1f, 100f)]
    public float intensity = 40f;

    [Header("Scaling")]
    [SerializeField]
    private float minSize = 0.3f;
    [SerializeField]
    private float sizeFactor = 0.03f;

    private Renderer glowRenderer;
    private Material glowMaterial;
    private Light lightSource;

    [SerializeField]
    private float lightSourceIntensityCoeffitient = 1f;
    // Start is called before the first frame update
    private void Awake()
    {
        glowRenderer = GetComponentInChildren<MeshRenderer>();

        // Create unique material instance
        glowMaterial = glowRenderer.material;
        lightSource = GetComponentInChildren<Light>();
    }
    void ApplyColor()
    {
        // HDR color = color * intensity
        glowMaterial.SetColor("_UnlitColor", lightColor* intensity);
        // Built-in pipeline fallback:
        glowMaterial.SetColor("_Color", lightColor* intensity);
        //glowMaterial.color = lightColor;
        glowMaterial.SetColor("_EmissionColor", lightColor* intensity);
        glowMaterial.SetColor("_EmissiveColor", lightColor * intensity);
        //_EmissionColor
        lightSource.color = lightColor;
        lightSource.intensity = intensity*lightSourceIntensityCoeffitient;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LightBulbScaling scalingScript = transform.GetChild(0).GetComponent<LightBulbScaling>();
        scalingScript.SetScalingParams(minSize, sizeFactor);
        ApplyColor();
    }
    void LateUpdate()
    {
        //transform.forward = Camera.main.transform.forward;
        //float d = Vector3.Distance(Camera.main.transform.position, transform.position);
        //float size = Mathf.Max(minSize, d * sizeFactor);
        //transform.localScale = Vector3.one * size;
    }
    void OnWillRenderObject()
    {
        Camera cam = Camera.current;
        if (!cam) return;

        // Billboard
        transform.GetChild(0).forward = cam.transform.forward;

        // Distance-based scaling
        float d = Vector3.Distance(cam.transform.position, transform.position);
        float size = Mathf.Max(minSize, d * sizeFactor);
        transform.GetChild(0).localScale = Vector3.one * size;
    }
}
