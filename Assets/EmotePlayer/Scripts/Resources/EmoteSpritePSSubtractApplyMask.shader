Shader "Emote/SpritePSSubtractApplyMask" {

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
			Blend SrcAlpha One, Zero One
			BlendOp RevSub

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile VERTCOLOR_OFF VERTCOLOR_SINGLE VERTCOLOR_DOUBLE
			#pragma multi_compile GRAYSCALE_OFF GRAYSCALE_ON
                        #pragma multi_compile TEXTURE_AST_OFF TEXTURE_AST_ON
                        #pragma multi_compile MESH_DEFORMATION_OFF MESH_DEFORMATION_ON
			
			#include "_EmoteSpritePSSubtract.cginc"

			ENDCG

		}
	}


}
