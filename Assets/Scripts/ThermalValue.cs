using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalValue : MonoBehaviour
{

    public Material materialTest;
    // Start is called before the first frame update
    void Start()
    {
        materialTest =  GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
