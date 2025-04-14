using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour
{
    public ThermalComponent thermalComponent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            thermalComponent.temperature = 75.0f;
            thermalComponent.m_isActive = true;
        }
        if (thermalComponent.temperature >= 0.0f)
        {
            thermalComponent.temperature -= 25 * Time.deltaTime;
            thermalComponent.temperature = Mathf.Clamp(thermalComponent.temperature, 0, 100);
        }
        if (thermalComponent.temperature == 0.0f)
        {
            thermalComponent.m_isActive = false;
        }
    }
}
