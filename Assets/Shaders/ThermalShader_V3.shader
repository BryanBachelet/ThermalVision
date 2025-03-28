

Shader "Unlit/ThermalShader_V3"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Temperature("Temperature",float) = 0
        _Position("Position",Vector) = (0,0,0)
        _Radius("Radius", float) = 0
        _Color1("Color1",Color) = (1,1,1,1)
        _Color2("Color2",Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
               float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            struct HeatInfo
            {
                int isActive;
                Vector position;
                float radius;
                float temperature;
            };

            StructuredBuffer<HeatInfo> heatInfos;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Temperature;
            float3 _Position;
            float _Radius;
            float4 _Color1;
            float4 _Color2;
           


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                return o;

            }

            fixed4 frag (v2f i) : SV_Target
            {
                float finalTemperature = 0;
                fixed4 col  = (1,1,1,1);
                for(int j = 0 ; j < 5 ; j++)
                {
                        
                    float distancePosition =   distance(heatInfos[j].position , i.worldPos);
                    float ratioDistance = 1.0f - (distancePosition /  heatInfos[j].radius);
                    float invDist =  1.0f /(1.0f + distancePosition);
                    float curTemperature =   (14*  log10(invDist)  +  heatInfos[j].temperature)/100;
                    curTemperature =  ratioDistance * curTemperature  ;
                    finalTemperature += saturate(curTemperature  ) * heatInfos[j].isActive ;
                }
                col = tex2D(_MainTex, float2( finalTemperature ,1));
                return col;
            }
            ENDCG
        }
    }
}
