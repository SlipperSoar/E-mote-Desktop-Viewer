Shader "Emote/SpriteOutline" {
	SubShader {

		Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
		Pass {
			ZWrite Off
            Cull Off
			Blend OneMinusDstAlpha DstAlpha, OneMinusDstAlpha DstAlpha

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
                        #pragma multi_compile REFALPHAMASK_OFF REFALPHAMASK_ON REFALPHAMASK_ON2
                        #pragma multi_compile TEXTURE_AST_OFF TEXTURE_AST_ON
                        #pragma multi_compile MESH_DEFORMATION_OFF MESH_DEFORMATION_ON

	                #include "_EmoteSpriteOutline.cginc"

			ENDCG
		}
	}
}
