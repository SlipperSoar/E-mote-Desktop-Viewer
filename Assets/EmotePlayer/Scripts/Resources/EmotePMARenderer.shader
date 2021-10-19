Shader "Emote/PMARenderer" {
	SubShader {

		Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
		Pass {
			ZWrite Off
            Cull Off
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
                        float4 _Color;

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
                        };

			struct my_appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

                        v2f vert (my_appdata v)
			{
				v2f o;
                                o.pos = v.vertex;
                                o.pos = UnityObjectToClipPos (o.pos);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

             half4 frag (v2f i) 
#if !defined(SHADER_API_PSSL)
                : COLOR
#endif
			{
				half4 texcol = tex2D (_MainTex, i.uv);
                                texcol *= _Color;
                                return texcol;
			}

			ENDCG
		}
	}
}
