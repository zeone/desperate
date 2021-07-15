using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{


    public float scrollSpeed = 0.5f;
    public float pulseSpeed = 1.5f;

    public float noiseSize = 1.0f;

    public float maxWidth = 0.5f;
    public float minWidth = 0.2f;

    public GameObject pointer = null;

    private Renderer material;

    private LineRenderer lRenderer;
    private float aniTime = 0;
    private float aniDir = 1;

    private PerFrameRaycast raycast;

    private void Start()
    {
        material = gameObject.GetComponent<Renderer>();
        lRenderer = gameObject.GetComponent<LineRenderer>();

        aniTime = 0.0f;

        // Change some animation values here and there
        ChoseNewAnimationTargetCoroutine();

        raycast = GetComponent<PerFrameRaycast>();
    }

    private IEnumerator ChoseNewAnimationTargetCoroutine()
    {
        while (true)
        {
            aniDir = aniDir * 0.9f + Random.Range(0.5f, 1.5f) * 0.1f;
            yield return null;

            minWidth = minWidth * 0.8f + Random.Range(0.1f, 1.0f) * 0.2f;
            yield return new WaitForSeconds(1.0f + Random.value * 2.0f - 1.0f);
        }
    }

    private void Update()
    {
        Vector3 _matOffset = material.material.mainTextureOffset;
        _matOffset.x += Time.deltaTime * aniDir * scrollSpeed;
        material.material.mainTextureOffset = _matOffset;
        // GetComponent<Renderer>().material.mainTextureOffset.x += Time.deltaTime * aniDir * scrollSpeed;
        material.material.SetTextureOffset("_NoiseTex", new Vector2(-Time.time * aniDir * scrollSpeed, 0.0f));

        float aniFactor = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
        aniFactor = Mathf.Max(minWidth, aniFactor) * maxWidth;
        lRenderer.SetWidth(aniFactor, aniFactor);

        // Cast a ray to find out the end point of the laser
        RaycastHit hitInfo = raycast.GetHitInfo();
        if (hitInfo.transform)
        {
            lRenderer.SetPosition(1, (hitInfo.distance * Vector3.forward));
            Vector3 _matScale = material.material.mainTextureScale;
            _matScale.x = 0.1f * (hitInfo.distance);
            material.material.mainTextureScale = _matScale;
            // GetComponent<Renderer>().material.mainTextureScale.x = 0.1f * (hitInfo.distance);
            material.material.SetTextureScale("_NoiseTex", new Vector2(0.1f * hitInfo.distance * noiseSize, noiseSize));

            // Use point and normal to align a nice & rough hit plane
            if (pointer)
            {
                pointer.GetComponent<Renderer>().enabled = true;
                pointer.transform.position = hitInfo.point + (transform.position - hitInfo.point) * 0.01f;
                pointer.transform.rotation = Quaternion.LookRotation(hitInfo.normal, transform.up);
                Vector3 _pointerAngle = pointer.transform.eulerAngles;
                _pointerAngle.x = 90f;
                pointer.transform.eulerAngles = _pointerAngle;
            }
        }
        else
        {
            if (pointer)
                pointer.GetComponent<Renderer>().enabled = false;
            float maxDist = 200.0f;
            lRenderer.SetPosition(1, (maxDist * Vector3.forward));
            Vector3 _textScale = material.material.mainTextureScale;
            _textScale.x = 0.1f * (maxDist);
            material.material.mainTextureScale = _textScale;
            //  GetComponent<Renderer>().material.mainTextureScale.x = 0.1 * (maxDist);
            material.material.SetTextureScale("_NoiseTex", new Vector2(0.1f * (maxDist) * noiseSize, noiseSize));
        }
    }
}
