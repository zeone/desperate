using UnityEngine;
using System.Collections;

public class PreloadSc : MonoBehaviour
{

    public GameObject[] PreloadItems;
    public AudioListener CameraListener;
    public float WaitSeconds = 2;
    // Use this for initialization

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("Level is loaded " + level);
        CameraListener.enabled = false;
        // StartCoroutine(UnloadCourt());
        Unload();
    }
    /* void Awake()
     {
         Debug.Log("Test load");
     }*/

    void OnEnable()
    {
        CameraListener.enabled = false;
        // StartCoroutine(UnloadCourt());
        Unload();
    }
    void Load()
    {
        CameraListener.enabled = false;
        foreach (GameObject item in PreloadItems)
        {
            item.SetActive(true);
        }
        foreach (GameObject o in transform)
        {
            o.SetActive(true);
        }
    }

    void Unload()
    {
        foreach (GameObject item in PreloadItems)
        {
            item.SetActive(false);
        }
        CameraListener.enabled = true;

    }

    IEnumerator UnloadCourt()
    {
        yield return new WaitForSeconds(WaitSeconds);
        Unload();
    }
}
