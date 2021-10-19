			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
            float _Grayscale;
            float _AlphaCutoff;

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

             half4 frag (v2f i) 
#if !defined(SHADER_API_PSSL)
                : COLOR
#endif
			{
				half4 texcol = emoteTex2D (_MainTex, i.uv);
#if defined(REFALPHAMASK_ON)
                half2 uv2 = i.scrPos.xy / i.scrPos.w;
                half4 alphaMask = tex2D (_AlphaMaskTex, uv2);
                texcol.a *= alphaMask.r;
#endif
#if defined(REFALPHAMASK_ON2)
                half2 uv2 = i.scrPos.xy / i.scrPos.w;
                half4 alphaMask = tex2D (_AlphaMaskTex2, uv2);
                texcol.a *= alphaMask.r;
#endif
                clip(texcol.a - _AlphaCutoff);
#if defined(GRAYSCALE_ON)
                float v = 0.298912 * texcol.r + 0.586611 * texcol.g + 0.114478 * texcol.b;
                texcol.rgb = lerp(texcol.rgb, v, _Grayscale);
#endif                
#if defined(VERTCOLOR_SINGLE)
				texcol *= i.color;
#endif
#if defined(VERTCOLOR_DOUBLE)
				texcol *= i.color;
				texcol.rgb += texcol.rgb;
#endif
                texcol.rgb *= texcol.a;
				return texcol;
			}

