Shader "Emote/AnaglyphComposite" {

	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)  // RGBA
		_MainTex   ("Texture", 2D) = "white" { }
	}

    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Pass {
            ZWrite Off
            ZTest Always
            Blend SrcAlpha One, Zero One
                 
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile GRAYSCALE_OFF GRAYSCALE_ON 

			#include "UnityCG.cginc"

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

            struct my_appdata {
                float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
   	        };  

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (my_appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}



			half4 frag (v2f i) : COLOR
			{
				half4 texcol = tex2D (_MainTex, i.uv);
#if defined(GRAYSCALE_ON)
                float v = 0.298912 * texcol.r + 0.586611 * texcol.g + 0.114478 * texcol.b;
                texcol.rgb = v;
#endif
				texcol *= _Color;
				return texcol;
			}

			ENDCG
		}
	}


}

