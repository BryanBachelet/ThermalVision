using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDComponent : MonoBehaviour
{
    public Image fadeImage;
    public Action EndFade;
    public float durationFade;
    public AnimationCurve animationCurve;
    private float m_durationFade;
    private bool m_isFadeActive;
    // Start is called before the first frame update
    void Start()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (!m_isFadeActive)
            return;
        m_durationFade += Time.deltaTime;
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, animationCurve.Evaluate(m_durationFade / durationFade));
        if(m_durationFade > durationFade)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
            m_isFadeActive = false;
            EndFade?.Invoke();
        }
    }

    public void ActiveFade()
    {
        m_isFadeActive = true;
        m_durationFade = 0.0f;
    }
}
