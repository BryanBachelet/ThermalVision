using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ThermalComponent : MonoBehaviour
{
    public Material material;
  public  Texture2D texture;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        texture = new Texture2D(512, 512,TextureFormat.RGBAFloat,false);

    }

    // Update is called once per frame
    void Update()
    {

        float value = Mathf.Abs( Mathf.Sin(Time.realtimeSinceStartup)) ;
        Debug.Log(value);
        for (int i = 0; i < 512; i++)
        {
          
            for (int j = 0; j < 512; j++)
            {
                texture.SetPixel(i, j, new Color(value, value, value, 1));
               
            }
        }
        texture.Apply();
        material.SetTexture("_Texture2D", texture);
    }
}
