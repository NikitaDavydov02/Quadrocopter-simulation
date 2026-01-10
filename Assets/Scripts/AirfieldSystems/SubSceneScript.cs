using UnityEngine;

public class SubSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.OnSubsceneEnable();
    }
    private void OnDisable()
    {
        Debug.Log("Scene disable!");
        MainManager.Instance.OnSubsceneDisnable();
    }
}
