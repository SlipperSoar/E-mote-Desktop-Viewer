			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;

                        float4 _OutlineParam;
                        float4 _OutlineColor;
                        

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
                half neighbourA = 0;
                neighbourA = max(neighbourA, emoteTex2D (_MainTex, i.uv + half2(-_OutlineParam.x, 0)).a);
                neighbourA = max(neighbourA, emoteTex2D (_MainTex, i.uv + half2(+_OutlineParam.x, 0)).a);
                neighbourA = max(neighbourA, emoteTex2D (_MainTex, i.uv + half2(0, -_OutlineParam.y)).a);
                neighbourA = max(neighbourA, emoteTex2D (_MainTex, i.uv + half2(0, +_OutlineParam.y)).a);

                if (texcol.a < 32.0/256)
                   texcol.a = neighbourA;
                else
                   texcol.a = 0;
                                
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
                clip(texcol.a - 16.0/256);

        	return _OutlineColor;
	}

