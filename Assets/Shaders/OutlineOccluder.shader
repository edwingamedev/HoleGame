Shader "Custom/OutlineOnlyWhenOccluding"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.05
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }

        // Outline pass
        Pass
        {
            Name "Outline"
            Cull Front     // Render backfaces only
            ZWrite Off
            ZTest Always   // Always draw outline
            ColorMask RGB

            Stencil
            {
                Ref 1
                Comp NotEqual    // Only draw if stencil != 1 (i.e., player is hidden)
                Pass Keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineThickness;
            fixed4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                float3 offset = norm * _OutlineThickness;
                float4 displaced = v.vertex + float4(offset, 0);
                o.pos = UnityObjectToClipPos(displaced);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
