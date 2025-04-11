using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class ThermalComponent : MonoBehaviour
{
    [Header("Thermal Variables")]
    public float temperature;
    [MinAttribute(0.00001f)]
    public float radius;

    [Header("Heat Direction")]
    public   bool isSpotActive;
    public Vector3 directionSpot;
    public float temperatureSpot;
    [Range(0, 180)] public float angle;
    [Range(0, 180)] public float outerAngle;
    public float spotSize;
    private float cutOff;


    [SerializeField] private bool m_isActive;

    private ThermalManager m_thermalManager;

    public void Start()
    {
        m_thermalManager = FindFirstObjectByType<ThermalManager>();
        SetActiveThermalSource(m_isActive);
    }

    public void SetActiveThermalSource(bool isThermalActive)
    {
        m_isActive = isThermalActive;
        if (m_isActive) m_thermalManager.AddThermalComponent(this);
        else m_thermalManager.RemoveThermalComponent(this);
    }


    public HeatInfo GetHeatInfos()
    {
        HeatInfo info = new HeatInfo();
        info.isActive = m_isActive ? 1 : 0;
        info.position = transform.position;
        info.temperature = temperature;
        info.radius = radius;
        info.isSpotActive =  isSpotActive ? 1 : 0; 
        info.spotDirection = directionSpot;
        info.lengthSpot = spotSize;
        info.cutOffSpot = Mathf.Cos(Mathf.Deg2Rad * angle);
        info.outerCutOffSpot = Mathf.Cos(Mathf.Deg2Rad * outerAngle); ;
        info.temperatureSpot = temperatureSpot;
        return info;
    }
}
