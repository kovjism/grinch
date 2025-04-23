Shader "Custom/WorldSpaceCutoutBoxAlpha"
{
    Properties
    {
        _Color ("Tint Color", Color) = (0,0,0,0.7)
        _HoleCenter ("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleSize ("Hole Size", Vector) = (0.2, 0.2, 0, 0)
        _Softness ("Soft Edge", Float) = 0.05
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        ZWrite Off
        Cull Off

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
            };

            fixed4 _Color;
            float4 _HoleCenter;
            float4 _HoleSize;
            float _Softness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy; // World-space quads: using position as UVs
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv.xy;

                float2 halfSize = _HoleSize.xy / 2.0;
                float2 diff = abs(uv - _HoleCenter.xy);

                float2 fade = smoothstep(halfSize, halfSize + _Softness, diff);
                float mask = max(fade.x, fade.y); // 0 in center (clear), 1 outside (dim)

                fixed4 finalColor = _Color;
                finalColor.a *= mask;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
