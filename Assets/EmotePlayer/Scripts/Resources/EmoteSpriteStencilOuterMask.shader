Shader "Emote/SpriteStencilOuterMask" {

	Properties {
		_Stencil("Stencil", Int) = 0
	}

	SubShader {

		Tags { "Queue" = "Transparent" }
		Pass {
			Stencil {
				Ref [_Stencil]
				Comp Equal
				Pass DecrWrap
				Fail Keep
				ZFail DecrWrap
			}
			ZWrite Off
            ZTest Always
            Cull Off
			ColorMask 0

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
                        #pragma multi_compile TEXTURE_AST_OFF TEXTURE_AST_ON
                        #pragma multi_compile MESH_DEFORMATION_OFF MESH_DEFORMATION_ON
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _TestAlpha;

                        #include "_EmoteVS.cginc"
                        #include "_EmotePS.cginc"

			half4 frag (v2f i) : COLOR
			{
				half4 texcol = emoteTex2DAlpha (_MainTex, i.uv);
				if (texcol.a < _TestAlpha) discard;
				return texcol;
			}

			ENDCG

		}
	}


}
