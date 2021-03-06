using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTextTrigger : MonoBehaviour
{
    public SpriteRenderer animatedSpriteRenderer;
    public TextMeshProUGUI intructionText;

    public AnimationCurve fadeInCurve;
    public float fadeInSpeedMult = 2f;

    private Color spriteColor;
    private Color textColor;

    private bool playedOnce = false;

    void Start()
    {
        spriteColor = animatedSpriteRenderer.color;
        textColor = intructionText.color;

        spriteColor.a = 0;
        textColor.a = 0;

        animatedSpriteRenderer.color = spriteColor;
        intructionText.color = textColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playedOnce && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeIn());
            playedOnce = true;
        }
    }

    private IEnumerator FadeIn()
    {
        //Color spriteColor = animatedSpriteRenderer.color;
        //Color textColor = intructionText.color;
        float t = 0;

        while (t < 1)
        {
            spriteColor.a = fadeInCurve.Evaluate(t);
            textColor.a = fadeInCurve.Evaluate(t);

            animatedSpriteRenderer.color = spriteColor;
            intructionText.color = textColor;

            t += fadeInSpeedMult * Time.deltaTime;
            yield return null;
        }
    }
}
