// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/MyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "LightMode"="ForwardBase" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color: COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //World normal
                float3 normalDirectoin = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
                //World light
                float3 lightDirection = normalize(float4(_WorldSpaceLightPos0.xyz, 0));
                //Diffuse reflection
                float3 diffuseReflection = max(0, dot(lightDirection, normalDirectoin));
                //Local position
                float3 localPos = v.vertex.xyz;
                //World position
                float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                //Camera world position
                float3 viewDirection = _WorldSpaceCameraPos.xyz;
                //ViewDirection
                float3 viewDirectionWorld = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);

                //float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                //float3 viewDirection = normalize(float3(_WorldSpaceCameraPos.xyz -UnityObjectToClipPos(v.vertex).xyz));
                o.color = float4(normalize(_WorldSpaceCameraPos).xyz, 1.0);
                //o.color = normalize(float4(_WorldSpaceCameraPos,1));
                o.color = float4(lightDirection.xyz, 1);
                
                o.color= float4(diffuseReflection.xyz, 1);
                o.color = float4(normalize(viewDirectionWorld), 1);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
