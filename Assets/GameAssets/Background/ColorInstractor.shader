Shader "Custom/ColorInstractor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CircleColor1 ("Circle Color 1", Color) = (1,0,0,1)
        _Radius1 ("Circle Radius 1", Float) = 0.2
        _CircleColor2 ("Circle Color 2", Color) = (0,1,0,1)
        _Radius2 ("Circle Radius 2", Float) = 0.4
        _CircleColor3 ("Circle Color 3", Color) = (0,0,1,1)
        _Radius3 ("Circle Radius 3", Float) = 0.6
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _CircleColor1;
            float _Radius1;
            float4 _CircleColor2;
            float _Radius2;
            float4 _CircleColor3;
            float _Radius3;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 baseColor = tex2D(_MainTex, i.uv);
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                half4 color = baseColor;

                if (dist <= _Radius3)
                {
                    //color = lerp(baseColor, _CircleColor3, smoothstep(_CircleRadius3 - 0.01, _CircleRadius3, dist));
                    color = _CircleColor3;
                }
                if (dist <= _Radius2)
                {
                    //color = lerp(baseColor, _CircleColor2, smoothstep(_CircleRadius2 - 0.01, _CircleRadius2, dist));
                    color = _CircleColor2;
                }
                if (dist <= _Radius1)
                {
                    //color = lerp(baseColor, _CircleColor1, smoothstep(_CircleRadius1 - 0.01, _CircleRadius1, dist));
                    color = _CircleColor1;
                }

                return color;
            }
            ENDCG
        }
    }
}
