using UnityEngine;
using System.Collections;

/// <summary>
/// Randomize the color and intensity of the directional light in the scene.
/// </summary>
public class RandomizeLights : MonoBehaviour
{
    [SerializeField]
    private float lightColorChangeSpeed = .2f;
    [SerializeField]
    private float lightIntensityChangeSpeed = .2f;


    private Light sceneLight;
    private WaitForSeconds waitForSecondsColor = new WaitForSeconds(5f);
    private WaitForSeconds waitForSecondsIntensity = new WaitForSeconds(5f);

    private void Awake() {
        sceneLight = GetComponent<Light>();
        StartCoroutine(RandomizeLightColor());
        StartCoroutine(RandomizeLightIntensity());
    }

    IEnumerator RandomizeLightColor() {
        yield return new WaitForSeconds(Random.Range(0f, 4f));
        while (true) {
            yield return waitForSecondsColor;
            Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            float currentTime = 0f;
            while (currentTime <= 1f) {
                currentTime += Time.deltaTime;
                Color newColor = Color.Lerp(sceneLight.color, randomColor, lightColorChangeSpeed * currentTime);
                sceneLight.color = newColor;
                yield return null;
            }
        }
    }

    IEnumerator RandomizeLightIntensity() {
        yield return new WaitForSeconds(Random.Range(0f, 4f));
        while (true) {
            yield return waitForSecondsColor;
            float randomIntensity = Random.Range(0f, 4f);
            while (Mathf.Approximately(sceneLight.intensity, randomIntensity)) {
                float newItensity = Mathf.Lerp(sceneLight.intensity, randomIntensity, lightIntensityChangeSpeed * Time.deltaTime);
                sceneLight.intensity = newItensity;
                yield return null;
            }
        }
    }
}
