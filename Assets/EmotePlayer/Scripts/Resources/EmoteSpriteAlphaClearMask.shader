Shader "Emote/SpriteAlphaClearMask" {
	SubShader {

		Tags { "Queue" = "Transparent" }
		Pass {
			ZWrite Off
            Cull Off
            Blend One Zero

			CGPROGRAM

       		#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile PROJECTION_OFF PROJECTION_ON

			#include "UnityCG.cginc"

            float4 _ClearColor;

			struct my_appdata {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert (my_appdata v)
			{
				v2f o;
#if PROJECTION_ON
				o.pos = UnityObjectToClipPos (v.vertex);
#endif
#if PROJECTION_OFF
                o.pos = v.vertex;
#endif
				return o;
			}


			half4 frag (v2f i) : COLOR
			{
				return _ClearColor;
			}

			ENDCG
		}
	}


}
