Shader "Custom/GradientBackground"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.96, 0.94, 1, 1) // Lavender
        _BottomColor ("Bottom Color", Color) = (1, 1, 1, 1) // White
    }
    SubShader
    {
        Tags { "RenderType"="Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _TopColor;
            float4 _BottomColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float blendFactor = i.pos.y / 1.0; // Adjust this to control gradient direction
                return lerp(_BottomColor, _TopColor, blendFactor);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
