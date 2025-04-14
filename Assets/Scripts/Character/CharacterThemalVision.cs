using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CharacterThemalVision : MonoBehaviour
{
    public bool isThermalVisionActive;
    public ThermalManager manager;
    public PostProcessVolume postProcessProfile;
    public ColorParameter colorParameterA;
    public ColorParameter colorParameterB;

    private ColorGrading m_colorGrading;

    public PlayerHUDComponent m_playerHUDComponent;

    void Start()
    {
       postProcessProfile.profile.TryGetSettings<ColorGrading>( out  m_colorGrading);
        DeactivateThermalVision(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isThermalVisionActive = !isThermalVisionActive;
          if (isThermalVisionActive)
                ActivateThermalVision();
          else
                DeactivateThermalVision();
        }
    }


    public void ActivateThermalVision()
    {
        m_playerHUDComponent.ActiveFade();
        manager.ChangeViewMode(true);
        m_colorGrading.colorFilter.value = colorParameterA;
    }

    public void DeactivateThermalVision(bool hasTransition = true)
    {
        if(hasTransition)
            m_playerHUDComponent.ActiveFade();
        manager.ChangeViewMode(false);
        m_colorGrading.colorFilter.value = colorParameterB;
    }

}
