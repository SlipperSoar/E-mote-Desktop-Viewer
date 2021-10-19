Shader "Emote/SpriteEmbed" {

	SubShader {

		Tags { "Queue" = "Transparent" }
		Pass {
			ZWrite Off
            Cull Off
			Blend OneMinusDstAlpha DstAlpha, Zero One

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile VERTCOLOR_OFF VERTCOLOR_SINGLE VERTCOLOR_DOUBLE
			#pragma multi_compile GRAYSCALE_OFF GRAYSCALE_ON
                        #pragma multi_compile REFALPHAMASK_OFF REFALPHAMASK_ON REFALPHAMASK_ON2
                        #pragma multi_compile TEXTURE_AST_OFF TEXTURE_AST_ON
                        #pragma multi_compile MESH_DEFORMATION_OFF MESH_DEFORMATION_ON
			
			#include "_EmoteSpriteEmbed.cginc"

			ENDCG

		}
	}


}
