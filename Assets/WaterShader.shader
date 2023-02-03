Shader "Custom/WaterShader" {
    Properties{
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _WaveSpeed("Wave Speed", Range(0, 10)) = 1
        _WaveSize("Wave Size", Range(0, 1)) = 0.1
        _Refraction("Refraction", Range(0, 1)) = 0.5
        _Reflection("Reflection", Range(0, 1)) = 0.5
    }

    SubShader{
        Tags {"RenderType" = "Opaque"}
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _WaveSpeed;
            float _WaveSize;
            float _Refraction;
            float _Reflection;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                uv.y += _Time.y * _WaveSpeed;
                uv.x += _Time.y * _WaveSpeed;
                uv.x = uv.x * _WaveSize;
                uv.y = uv.y * _WaveSize;
                fixed4 col = tex2D(_MainTex, uv);
                col.r = col.r * _Refraction;
                col.g = col.g * _Reflection;
                return col;
            }
            ENDCG
        }
    }
        FallBack "Diffuse"  
}
