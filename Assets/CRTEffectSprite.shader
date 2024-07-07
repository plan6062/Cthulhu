Shader "Custom/CRTEffectSprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _ScanlineIntensity ("Scanline Intensity", Float) = 0.1
        _ScanlineCount ("Scanline Count", Float) = 100
        _GreenTint ("Green Tint", Color) = (0, 1, 0, 1)
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };
            
            fixed4 _Color;
            float _ScanlineIntensity;
            float _ScanlineCount;
            fixed4 _GreenTint;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                float scanline = sin(IN.texcoord.y * _ScanlineCount) * 0.5 + 0.5;
                c *= lerp(1, scanline, _ScanlineIntensity);
                c.rgb = lerp(c.rgb, c.rgb * _GreenTint.rgb, _GreenTint.a);
                c.rgb *= c.a;
                return c;
            }
        ENDCG
        }
    }
}