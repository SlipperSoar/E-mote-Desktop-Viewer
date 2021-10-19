Shader "Emote/Test" {

	Properties {
//		_TestTex ("Texture", 2D) = "white" { }
        _Grayscale("Grayscale", Float) = 0.0
	}

	SubShader {

		Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
		Pass {
			ZWrite Off
            Cull Off
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile VERTCOLOR_OFF VERTCOLOR_SINGLE VERTCOLOR_DOUBLE
			#pragma multi_compile GRAYSCALE_OFF GRAYSCALE_ON

			#include "UnityCG.cginc"
			
			sampler2D _TestTex;
			float4 _TestTex_ST;
            float _Grayscale;

			struct my_appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
#if !defined(VERTCOLOR_OFF)
				fixed4 color : COLOR;
#endif
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
#if !defined(VERTCOLOR_OFF)
				fixed4 color : COLOR;
#endif
			};

			v2f vert (my_appdata v)
			{
				v2f o;
				o.pos = v.vertex;
				o.uv = TRANSFORM_TEX (v.texcoord, _TestTex);
#if !defined(VERTCOLOR_OFF)
				o.color = v.color;
#endif
				return o;
			}

			half4 frag (v2f i) 
#if !defined(SHADER_API_PSSL)
                : COLOR
#endif
			{
				half4 texcol = tex2D (_TestTex, i.uv);
#if defined(GRAYSCALE_ON)
                float v = 0.298912 * texcol.r + 0.586611 * texcol.g + 0.114478 * texcol.b;
                texcol.rgb = lerp(texcol.rgb, v, _Grayscale);
#endif                
#if defined(VERTCOLOR_SINGLE)
				texcol *= i.color;
#endif
#if defined(VERTCOLOR_DOUBLE)
				texcol *= i.color;
				texcol.rgb += texcol.rgb;
#endif

				return texcol;
			}



			ENDCG
		}
	}
}
