// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Altitude("Altitude", Range(0,1)) = 1
        //[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        //ZTest Equal
        Pass
        {
            //ZWrite Off
            //ZTest GEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _Altitude;
            struct v2f {
                float4 pos : SV_POSITION;
                float2 depth : TEXCOORD0;
            };

            v2f vert(appdata_base v) {
                v2f o;
                //o.pos = UnityObjectToClipPos(v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                //UNITY_TRANSFER_DEPTH(o.depth);
                return o;
            }

            half4 frag(v2f i) : SV_Target{
                //UNITY_OUTPUT_DEPTH(i.depth);
                return half4(1,0,0,1);
            }
            ENDCG
        }
    }
}
