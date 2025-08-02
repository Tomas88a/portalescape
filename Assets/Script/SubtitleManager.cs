using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance; // 单例方便调用
    public TextMeshProUGUI subtitleText;
    private Coroutine subtitleRoutine;

    private void Awake()
    {
        Instance = this;
        if (subtitleText != null)
            subtitleText.gameObject.SetActive(false);
    }

    public void ShowSubtitle(string text, float duration = 2f)
    {
        if (subtitleRoutine != null)
            StopCoroutine(subtitleRoutine);

        subtitleRoutine = StartCoroutine(ShowSubtitleRoutine(text, duration));
    }

    private IEnumerator ShowSubtitleRoutine(string text, float duration)
    {
        subtitleText.text = text;
        subtitleText.alpha = 1f;
        subtitleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        // 可选：淡出
        float fadeTime = 0.5f;
        float t = 0;
        while (t < fadeTime)
        {
            subtitleText.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        subtitleText.alpha = 0f;
        subtitleText.gameObject.SetActive(false);
    }
}
