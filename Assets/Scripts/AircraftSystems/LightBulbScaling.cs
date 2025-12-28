using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightBulbScaling : MonoBehaviour
{
    private float minSize = 0.3f;
    private float sizeFactor = 0.03f;
    // Start is called before the first frame update
    private void Awake()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }
    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }
    void Start()
    {
        
    }
    public void SetScalingParams(float _minSize, float _sizeFactor)
    {
        minSize = _minSize;
        sizeFactor = _sizeFactor;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /*void OnWillRenderObject()
    {
        Camera cam = Camera.current;
        if (!cam) return;

        // Billboard
        transform.GetChild(0).forward = cam.transform.forward;

        // Distance-based scaling
        float d = Vector3.Distance(cam.transform.position, transform.position);
        float size = Mathf.Max(minSize, d * sizeFactor);
        transform.GetChild(0).localScale = Vector3.one * size;
    }*/
    void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        // Rotate billboard
        transform.forward = cam.transform.forward;

        // Optional distance scaling
        float d = Vector3.Distance(cam.transform.position, transform.position);
        float size = Mathf.Max(0.5f, d * 0.03f);
        transform.localScale = Vector3.one * size;
    }
}
