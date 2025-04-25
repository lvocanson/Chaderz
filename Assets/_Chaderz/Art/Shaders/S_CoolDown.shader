Shader "Unlit/S_CoolDown"
{
    Properties
    {
        _HighColor ("High Color", Color) = (1,0,0,1)
        _LowColor ("Low Color", Color) = (1,.6,0,1)
        _MaxHeight ("Max Height", Float) = 1
        _MinHeight ("Min Height", Float) = 0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _HighColor;
            float4 _LowColor;
            float _MaxHeight;
            float _MinHeight;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float height = saturate((i.worldPos.y - _MinHeight) / (_MaxHeight - _MinHeight));
                fixed4 col = lerp(_LowColor, _HighColor, height);
                return col;
            }
            ENDCG
        }
    }
}
