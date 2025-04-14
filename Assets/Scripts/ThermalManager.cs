using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using System.Drawing;

[System.Serializable]
public struct HeatInfo
{
    public int isActive;
    public Vector3 position;
    [MinAttribute(0.00001f)]
    public float radius;
    public float temperature;
    public int isSpotActive;
    public Vector3 spotDirection;
    public float lengthSpot;
    public float cutOffSpot;
    public float outerCutOffSpot;
    public float temperatureSpot;

    public static unsafe int GetSize()
    {
        return sizeof(HeatInfo);
    }
}


public class ThermalManager : MonoBehaviour
{
    const int MaxThermalSource = 32;

    [SerializeField] private Material m_thermalMaterial;
    [SerializeField] private Material m_thermalMaterial2;
    private ComputeBuffer m_thermalBuffer;

    private ThermalComponent[] m_thermalComponentsArray = new ThermalComponent[32];
    private HeatInfo[] m_thermalSourceInfos = new HeatInfo[32];
    private int m_thermalComponentsActiveCount;
    public bool isThermalModeActive;
    private bool isEditorUdapteTrigger;
    public ThermalValue thermalValue;

    // Temps
    private bool isOneTime;

    #region Unity Functions
    private void Awake()
    {
        m_thermalComponentsActiveCount = 0;
        isThermalModeActive = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_thermalBuffer = new ComputeBuffer(MaxThermalSource, HeatInfo.GetSize(), ComputeBufferType.Default, ComputeBufferMode.Dynamic);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateThermalSourceData();
   
    }

#if UNITY_EDITOR
    public void UpdateThermalBufferEditor()
    {

        isEditorUdapteTrigger = true;
        m_thermalComponentsActiveCount = 0;
        m_thermalBuffer = new ComputeBuffer(MaxThermalSource, HeatInfo.GetSize(), ComputeBufferType.Default, ComputeBufferMode.Dynamic);
        ThermalComponent[] thermalComponentArray = FindObjectsByType<ThermalComponent>(FindObjectsSortMode.InstanceID);

        for (int i = 0; i < thermalComponentArray.Length; i++)
        {
            if (thermalComponentArray[i].enabled)
                AddThermalComponent(thermalComponentArray[i]);
        }
        UpdateThermalSourceData();
        for (int i = 0; i < thermalComponentArray.Length; i++)
        {
            if (thermalComponentArray[i].enabled)
                RemoveThermalComponent(thermalComponentArray[i]);
        }



    }

    public void OnPostRender()
    {
        if (!isEditorUdapteTrigger) return;
        isEditorUdapteTrigger = false;
        m_thermalBuffer?.Dispose();
    }


#endif




    public void OnValidate()
    {
       m_thermalBuffer?.Release();
    }
    

    public void ChangeViewMode()
    {
        isThermalModeActive = !isThermalModeActive;
        m_thermalMaterial.SetInt("_ActiveThermalMode", isThermalModeActive ? 1 : 0);
        m_thermalMaterial2.SetInt("_ActiveThermalMode", isThermalModeActive ? 1 : 0);
        if (thermalValue != null)
        {
            thermalValue.materialTest.SetInt("_ActiveThermalMode", isThermalModeActive ? 1 : 0);
        }
    }

    public void ChangeViewMode(bool state)
    {
        isThermalModeActive = state;
        m_thermalMaterial.SetInt("_ActiveThermalMode", isThermalModeActive ? 1 : 0);
        m_thermalMaterial2.SetInt("_ActiveThermalMode", isThermalModeActive ? 1 : 0);
        if (thermalValue != null)
        {
            thermalValue.materialTest.SetInt("_ActiveThermalMode", isThermalModeActive ? 1 : 0);
        }
    }
        private void OnDisable()
    {
        m_thermalBuffer?.Release();
    }
    private void OnDestroy()
    {

        m_thermalBuffer?.Release();

    }

    #endregion 

    public void UpdateThermalSourceData()
    {
  
        HeatInfo[] heatInfos = new HeatInfo[m_thermalComponentsActiveCount];
        for (int i = 0; i < m_thermalComponentsActiveCount; i++)
        {
            heatInfos[i] = m_thermalComponentsArray[i].GetHeatInfos();
        }

        m_thermalBuffer.SetData(heatInfos);
        Shader.SetGlobalBuffer("heatInfos", m_thermalBuffer);
        Shader.SetGlobalFloat("numberHeatSource", m_thermalComponentsActiveCount);
        //m_thermalMaterial.SetBuffer("heatInfos", m_thermalBuffer);
        //m_thermalMaterial2.SetBuffer("heatInfos", m_thermalBuffer);
        if (thermalValue != null)
        {
            thermalValue.materialTest.SetBuffer("heatInfos", m_thermalBuffer);
        }
    }

    public void AddThermalComponent(ThermalComponent newThermalComponent)
    {
        m_thermalComponentsArray[m_thermalComponentsActiveCount] = newThermalComponent;
        m_thermalComponentsActiveCount++;
    }

    public void RemoveThermalComponent(ThermalComponent newThermalComponent)
    {
        for (int i = 0; i < m_thermalComponentsActiveCount; i++)
        {
            if (m_thermalComponentsArray[i] == newThermalComponent)
            {
                m_thermalComponentsArray[i] = null;
                m_thermalComponentsActiveCount--;
                return;
            }
        }
    }

}
