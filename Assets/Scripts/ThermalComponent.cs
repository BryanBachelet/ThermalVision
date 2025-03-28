using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;



[System.Serializable]
public struct HeatInfo
{
    public int isActive;
    public Vector4 position;
    [MinAttribute(0.00001f)]
    public float radius;
    public float temperature;


    public static unsafe int GetSize() => sizeof(HeatInfo);
}

public class ThermalComponent : MonoBehaviour
{
    const int MaxHeat = 5;

    public Material material;
    private ComputeBuffer m_buffer;
    public HeatInfo[] heatInfos = new HeatInfo[5]; 

    // Start is called before the first frame update
    void Start()
    {
     //   material = GetComponent<Renderer>().material;
        m_buffer = new ComputeBuffer(MaxHeat, HeatInfo.GetSize(),ComputeBufferType.Structured);

    }

    // Update is called once per frame
    void Update()
    {
        m_buffer.SetData(heatInfos);
        material.SetBuffer("heatInfos", m_buffer);
    }

    private void OnDisable()
    {
        m_buffer?.Dispose();
    }
}
