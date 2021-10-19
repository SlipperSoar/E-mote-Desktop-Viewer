Shader "Emote/SpriteStencilClearMask" {
	Properties {
		_Stencil("Stencil", Int) = 0
	}

	SubShader {

		Tags { "Queue" = "Transparent" }
		Pass {
			Stencil {
				Ref [_Stencil]
				Comp Always
				Pass Replace
                ZFail Replace 
			}
			ZWrite Off
            ZTest Always
            Cull Off
            ColorMask 0

			CGPROGRAM

       		#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile PROJECTION_OFF PROJECTION_ON

			#include "UnityCG.cginc"

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
				return half4(255, 0, 0, 255);
			}

			ENDCG
		}
	}


}
