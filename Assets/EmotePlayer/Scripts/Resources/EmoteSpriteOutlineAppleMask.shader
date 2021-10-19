Shader "Emote/SpriteOutlineApplyMask" {

	Properties {
		_Stencil("Stencil", Int) = 0
	}

	SubShader {

		Tags { "Queue" = "Transparent" }
		Pass {
			Stencil {
				Ref [_Stencil]
				Comp Equal
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			ZWrite Off
                        Cull Off
			Blend OneMinusDstAlpha DstAlpha, OneMinusDstAlpha DstAlpha

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
                        #pragma multi_compile TEXTURE_AST_OFF TEXTURE_AST_ON
                        #pragma multi_compile MESH_DEFORMATION_OFF MESH_DEFORMATION_ON
			
			#include "_EmoteSpriteOutline.cginc"

			ENDCG

		}
	}


}
