using UnityEngine;

public class ScaleSin : MonoBehaviour
{
    [SerializeField]
    private float pulseSpeed = .01f;
    [SerializeField]
    private float scaleMultiplier = 3f;

    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.localScale = Vector3.one * (Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed)) + 1) * scaleMultiplier;
    }
}
