Shader "Custom/ThermalShader_V4"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        [MaterialToggle] _ActiveThermalMode("ActiveThermalMod",int) = 1
        _ThermalTexture ("_ThermalTexture", 2D) = "white" {}
        _BaseTemperature("Base Temperature",float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 200
        CGPROGRAM

       

        #include "UnityCG.cginc"
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows  vertex:vert finalcolor:mycolor

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _ThermalTexture;

        struct HeatInfo
        {
            int isActive;
            float3 position;
            float radius;
            float temperature;
            int isSpotActive;
            float3 spotDirection;
            float lengthSpot;
            float cutOffSpot;
            float outerCutOffSpot;
            float temperatureSpot;
        };
        
        #ifdef SHADER_API_D3D11
          StructuredBuffer<HeatInfo> heatInfos ;
        #endif

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _ColorThermal;
        fixed4 _ColorThermal1;
        int _ActiveThermalMode;
        int _ActiveThermalMode2;
        float3 _Position;
        float _BaseTemperature;
        
        struct Input
        {
            float2 uv_MainTex;
            float4 pos ;
            float3 worldPos;
        };

        
        void vert(inout appdata_full i, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.pos = mul (unity_ObjectToWorld, i.vertex);
         }

        void mycolor (Input IN, SurfaceOutputStandard o, inout fixed4 color)
          {
            float finalTemperature = _BaseTemperature;
            #ifdef SHADER_API_D3D11
               
                for(int j = 0 ; j < 32 ; j++)
                {
                    float3 directionElement =  IN.pos - heatInfos[j].position;
                    float dotresult =  dot(normalize(directionElement), heatInfos[j].spotDirection);
                    dotresult = saturate(dotresult) * heatInfos[j].isSpotActive;
                    float epsilon =  heatInfos[j].cutOffSpot -  heatInfos[j].outerCutOffSpot;
                    float intensity = saturate((dotresult - heatInfos[j].outerCutOffSpot)/epsilon);
                    
                    float distancePosition =   distance(heatInfos[j].position , IN.pos);

                    

                    float ratioDistance = 1.0f - (distancePosition /  heatInfos[j].lengthSpot);
                    float invDist =  1.0f /(1.0f + distancePosition);
                    float curTemperature =   (14*  log10(invDist)  +  heatInfos[j].temperatureSpot)/100;
                    curTemperature =  ratioDistance * curTemperature  ;
                    finalTemperature += saturate(curTemperature  ) * heatInfos[j].isActive * intensity;   
                 
                    ratioDistance = 1.0f - (distancePosition /  heatInfos[j].radius);
                    invDist =  1.0f /(1.0f + distancePosition);
                    curTemperature =   (14*  log10(invDist)  +  heatInfos[j].temperature)/100;
                    curTemperature =  ratioDistance * curTemperature  ;
                    finalTemperature += saturate(curTemperature  ) * heatInfos[j].isActive *(1-intensity)  ;   
                }

            #endif
           color = color  * (1- _ActiveThermalMode)  + (tex2D(_ThermalTexture, float2( finalTemperature ,1)) * _ActiveThermalMode);
       
        }


        void surf (Input IN, inout SurfaceOutputStandard o)
        {  
       
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            
            float finalTemperature = 0;
            #ifdef SHADER_API_D3D11
               

                o.Albedo = c.rgb * (1- _ActiveThermalMode);

                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;

                o.Alpha = c.a *(1- _ActiveThermalMode);
            #endif
        }
        ENDCG
    }
    FallBack "Diffuse"
}
