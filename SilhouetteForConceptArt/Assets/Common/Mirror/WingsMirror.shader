Shader "Hidden/WingsMirror"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int _MirrorMode;
            uniform float4 _MainTex_TexelSize;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                if(_MirrorMode == 1)
                {
                    uv *= 2.0f;
                    uv.x -=1.0f;
                    uv.x = abs(uv.x);
                    uv.y -= 0.5f;
                    //uv.x = 0.5f + abs(uv.x);
                }

                fixed4 col = tex2D(_MainTex, uv);

                if(_MirrorMode == 1)
                {
                    float f = step(_MainTex_TexelSize.y,uv.y);
                    float f2 = step(uv.y,1-_MainTex_TexelSize.y);
                    col = lerp( 1, col, f*f2  );
                }

                return col;
            }
            ENDCG
        }
    }
}
