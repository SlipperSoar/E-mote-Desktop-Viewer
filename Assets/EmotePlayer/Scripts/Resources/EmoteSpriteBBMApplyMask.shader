Shader "Emote/SpriteBBMApplyMask" {

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
			Blend One Zero, Zero One

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
                        #define BBM_ENABLED
			#pragma multi_compile VERTCOLOR_OFF VERTCOLOR_SINGLE VERTCOLOR_DOUBLE
			#pragma multi_compile GRAYSCALE_OFF GRAYSCALE_ON
                        #pragma multi_compile TEXTURE_AST_OFF TEXTURE_AST_ON
                        #pragma multi_compile MESH_DEFORMATION_OFF MESH_DEFORMATION_ON
                        #pragma multi_compile PMA_OFF PMA_ON
                        #pragma multi_compile BBM_BLEND_RAWADD BBM_BLEND_RAWSUB BBM_BLEND_RAWMULTIPLY BBM_BLEND_RAWSCREEN BBM_BLEND_PSADDITIVE BBM_BLEND_PSSUBTRACTIVE BBM_BLEND_PSOVERLAY BBM_BLEND_PSHARDLIGHT BBM_BLEND_PSSOFTLIGHT BBM_BLEND_PSCOLORDODGE BBM_BLEND_PSCOLORBURN BBM_BLEND_PSLIGHTEN BBM_BLEND_PSDARKEN BBM_BLEND_PSDIFFERENCE BBM_BLEND_PSEXCLUSION BBM_BLEND_FLTGRAYSCALE BBM_BLEND_FLTMOSAIC BBM_BLEND_FLTBLUR1 BBM_BLEND_FLTBLUR2 BBM_BLEND_FLTBLUR3 BBM_BLEND_FLTBLUR6 BBM_BLEND_FLTBLUR9

			#include "_EmoteSpriteBBM.cginc"

			ENDCG

		}
	}


}
