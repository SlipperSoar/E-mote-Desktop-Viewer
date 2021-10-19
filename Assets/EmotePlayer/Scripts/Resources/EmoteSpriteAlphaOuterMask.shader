Shader "Emote/SpriteAlphaOuterMask" {
    SubShader {
		Tags { "Queue" = "Transparent" }
        Pass { 
             ZWrite Off
             Cull Off
             Blend Zero OneMinusSrcColor

             CGPROGRAM

	         #pragma vertex vert
			 #pragma fragment frag

             #pragma multi_compile REFALPHAMASK_OFF REFALPHAMASK_ON REFALPHAMASK_ON2
             #pragma multi_compile TEXTURE_AST_OFF TEXTURE_AST_ON
             #pragma multi_compile MESH_DEFORMATION_OFF MESH_DEFORMATION_ON
			
             #include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

#if defined(REFALPHAMASK_ON)
            sampler2D _AlphaMaskTex;
            float4 _AlphaMaskTex_ST;
#endif
#if defined(REFALPHAMASK_ON2)
            sampler2D _AlphaMaskTex2;
            float4 _AlphaMaskTex2_ST;
#endif

                        #include "_EmoteVS.cginc"
                        #include "_EmotePS.cginc"

			half4 frag (v2f i) : COLOR
			{
				half4 texcol = emoteTex2DAlpha (_MainTex, i.uv);
                texcol.r = texcol.a;
#if defined(REFALPHAMASK_ON)
                half2 uv2 = i.scrPos.xy / i.scrPos.w;
                half4 alphaTexCol = tex2D (_AlphaMaskTex, uv2);
                texcol.r *= alphaTexCol.r;
#endif
#if defined(REFALPHAMASK_ON2)
                half2 uv2 = i.scrPos.xy / i.scrPos.w;
                half4 alphaTexCol = tex2D (_AlphaMaskTex2, uv2);
                texcol.r *= alphaTexCol.r;
#endif
                clip(texcol.r - 1.0/256);
				return texcol;
			}

			ENDCG

		}
	}
}
                                                       