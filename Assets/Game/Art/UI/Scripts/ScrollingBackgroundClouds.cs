using UnityEngine;
using UnityEngine.UI;

public class UIScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private RawImage rawImage;
    private float offset;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = new Rect(offset, 0f, 1f, 1f);
    }
}
