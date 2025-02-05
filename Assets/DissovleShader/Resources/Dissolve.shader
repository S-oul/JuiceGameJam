Shader "Custom/Dissolve" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0
        _EdgeColor("Edge Color", Color) = (1, 0, 0, 1)
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _GlowIntensity("Glow Intensity", Range(0, 15)) = 1
    }

    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _DissolveThreshold;
            float4 _EdgeColor;
            float _GlowIntensity;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // Calculate distance from center
                float2 center = float2(0.5, 0.5);
                float dist = length(i.uv - center);
                
                // Get noise value for irregular dissolve
                fixed noise = tex2D(_NoiseTex, i.uv).r;
                noise = noise * 0.5 + 0.5; // Normalize noise value
                
                // Combine distance and noise to create non-round dissolve
                float dissolveValue = dist + (noise - 0.5) * 3.3;
                
                fixed4 col = tex2D(_MainTex, i.uv);
                
                if (dissolveValue < _DissolveThreshold) {
                    clip(-1);
                }
                
                // Glow effect at the edge of dissolve
                float edgeFactor = smoothstep(_DissolveThreshold, _DissolveThreshold + 0.05, dissolveValue);
                if (edgeFactor < 1) {
                    return lerp(_EdgeColor * _GlowIntensity, col, edgeFactor);
                }
                
                return col;
            }
            ENDCG
        }
    }
}
