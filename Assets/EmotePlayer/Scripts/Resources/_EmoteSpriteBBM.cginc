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

            sampler2D _BBMBufferTex;
            float4 _BBMBufferOffset;
#if defined(BBM_BLEND_FLTMOSAIC)
            float4 _MosaicParam;
#endif
#if defined(BBM_BLEND_FLTBLUR1) || defined(BBM_BLEND_FLTBLUR2) || defined(BBM_BLEND_FLTBLUR3) || defined(BBM_BLEND_FLTBLUR6) || defined(BBM_BLEND_FLTBLUR9) 
            float4 _BlurParam;
#endif

        #include "_EmoteVS.cginc"
        #include "_EmotePS.cginc"

half4 bbmTex2D(sampler2D tex, half2 uv) {
#if defined(PMA_OFF)
    return tex2D(tex, uv);
#endif
#if defined(PMA_ON)
    half4 texcol = tex2D(tex, uv);
    if (texcol.a > 0)
       texcol.rgb /= texcol.a;
    return texcol;
#endif    
}
    

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
                float2 uv2 = ((i.scrPos.xy / i.scrPos.w) + _BBMBufferOffset.xy) * _BBMBufferOffset.zw;
#if defined(BBM_BLEND_RAWADD)
                half4 dest = bbmTex2D( _BBMBufferTex, uv2 );
                texcol.rgb = dest.rgb + texcol.rgb * texcol.a;
#endif
#if defined(BBM_BLEND_RAWSUB)
                half4 dest = bbmTex2D( _BBMBufferTex, uv2 );
                texcol.rgb = dest.rgb - (1 - texcol.rgb) * texcol.a;
#endif
#if defined(BBM_BLEND_RAWMULTIPLY)
                half4 dest = bbmTex2D( _BBMBufferTex, uv2 );
                texcol.rgb = texcol.rgb * texcol.a * dest.rgb + dest.rgb * (1 - texcol.a);
#endif
#if defined(BBM_BLEND_RAWSCREEN)
                half4 dest = bbmTex2D( _BBMBufferTex, uv2 );
                texcol.rgb = texcol.rgb * texcol.a * (1 - dest.rgb) + dest.rgb;
#endif
#if defined(BBM_BLEND_PSADDITIVE)
                half4 dest = bbmTex2D( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb, min(1.0,dest.rgb+texcol.rgb), texcol.a);
#endif
#if defined(BBM_BLEND_PSSUBTRACTIVE)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb,max(0.0,dest.rgb+texcol.rgb-1.0),texcol.a);
#endif
#if defined(BBM_BLEND_PSOVERLAY)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                half4 overlay;
                overlay.rgb = dest.rgb<0.5?(dest.rgb*texcol.rgb*2.0):(1.0-(1.0-dest.rgb)*(1.0-texcol.rgb)*2.0);
                texcol.rgb = lerp(dest.rgb,overlay.rgb,texcol.a);
#endif
#if defined(BBM_BLEND_PSHARDLIGHT)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                half4 hardlight;
                hardlight.rgb = texcol.rgb<0.5?(dest.rgb*texcol.rgb*2.0):(1.0-(1.0-dest.rgb)*(1.0-texcol.rgb)*2.0);
                texcol.rgb = lerp(dest.rgb,hardlight.rgb,texcol.a);
#endif
#if defined(BBM_BLEND_PSSOFTLIGHT)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                half4 softlight;
                softlight.rgb = pow(dest.rgb,texcol.rgb>0.5?(0.5/texcol.rgb):(1.0-texcol.rgb)*2.0);
                texcol.rgb = lerp(dest.rgb,softlight.rgb,texcol.a);
#endif
#if defined(BBM_BLEND_PSCOLORDODGE)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb,min(1.0,dest.rgb/max(1.0e-20,1.0-texcol.rgb)),texcol.a);
#endif
#if defined(BBM_BLEND_PSCOLORBURN)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb,max(0.0,1.0-(1.0-dest.rgb)/max(1.0e-20,texcol.rgb)),texcol.a);
#endif
#if defined(BBM_BLEND_PSLIGHTEN)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb,max(dest.rgb,texcol.rgb),texcol.a);
#endif
#if defined(BBM_BLEND_PSDARKEN)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb,min(dest.rgb,texcol.rgb),texcol.a);

#endif
#if defined(BBM_BLEND_PSDIFFERENCE)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb,abs(dest.rgb-texcol.rgb),texcol.a);
#endif
#if defined(BBM_BLEND_PSEXCLUSION)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                texcol.rgb = lerp(dest.rgb,dest.rgb+texcol.rgb-2.0*texcol.rgb*dest.rgb,texcol.a);
#endif
#if defined(BBM_BLEND_FLTGRAYSCALE)
                half4 dest = bbmTex2D ( _BBMBufferTex, uv2 );
                float g = 0.298912 * dest.r + 0.586611 * dest.g + 0.114478 * dest.b;
                texcol.rgb = lerp(dest.rgb, g * texcol.rgb, texcol.a);
#endif
#if defined(BBM_BLEND_FLTMOSAIC)
                half4 dest = bbmTex2D(_BBMBufferTex, uv2);
                float2 pix_vpos = float2(int(uv2.x * _MosaicParam.x) / _MosaicParam.x + _MosaicParam.z, 
                                         int(uv2.y * _MosaicParam.y) / _MosaicParam.y + _MosaicParam.w);
                half4 pix_val = bbmTex2D(_BBMBufferTex, pix_vpos);
                texcol.rgb = lerp(dest.rgb, pix_val.rgb, texcol.a);
#endif
#if defined(BBM_BLEND_FLTBLUR1)
                half4 dest = bbmTex2D(_BBMBufferTex, uv2 );
                float2 blur_ofst1 = _BlurParam.xy * 0.5;
                texcol.rgb = 0;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst1).rgb * 0.5;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst1).rgb * 0.5;
                texcol.rgb = lerp(dest.rgb, texcol.rgb, texcol.a);
#endif
#if defined(BBM_BLEND_FLTBLUR2)
                half4 dest = bbmTex2D(_BBMBufferTex, uv2);
                texcol.rgb = dest.rgb * 0.375;
                float2 blur_ofst1 = _BlurParam.xy * 1.2;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst1).rgb * 0.3125;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst1).rgb * 0.3125;
                texcol.rgb = lerp(dest.rgb, texcol.rgb, texcol.a);
#endif
#if defined(BBM_BLEND_FLTBLUR3)
                half4 dest = bbmTex2D(_BBMBufferTex, uv2);
                texcol.rgb = dest.rgb * 0.32258064516129;
                float2 blur_ofst1 = _BlurParam.xy * 1.28571428571429;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst1).rgb * 0.338709677419355;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst1).rgb * 0.338709677419355;
                texcol.rgb = lerp(dest.rgb, texcol.rgb, texcol.a);
#endif
#if defined(BBM_BLEND_FLTBLUR6)
                half4 dest = tex2D(_BBMBufferTex, uv2);
                texcol.rgb = dest.rgb * 0.227027027027027;
                float2 blur_ofst1 = _BlurParam.xy * 1.38461538461538;
                float2 blur_ofst2 = _BlurParam.xy * 3.23076923076923;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst1).rgb * 0.316216216216216;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst1).rgb * 0.316216216216216;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst2).rgb * 0.0702702702702703;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst2).rgb * 0.0702702702702703;
                texcol.rgb = lerp(dest.rgb, texcol.rgb, texcol.a);
#endif
#if defined(BBM_BLEND_FLTBLUR9)
                half4 dest = bbmTex2D(_BBMBufferTex, uv2);
                texcol.rgb = dest.rgb * 0.185714285714286;
                float2 blur_ofst1 = _BlurParam.xy * 1.42105263157895;
                float2 blur_ofst2 = _BlurParam.xy * 3.31578947368421;
                float2 blur_ofst3 = _BlurParam.xy * 5.21052631578947;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst1).rgb * 0.288701298701299;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst1).rgb * 0.288701298701299;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst2).rgb * 0.103636363636364;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst2).rgb * 0.103636363636364;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 + blur_ofst3).rgb * 0.0148051948051948;
                texcol.rgb += bbmTex2D(_BBMBufferTex, uv2 - blur_ofst3).rgb * 0.0148051948051948;
                texcol.rgb = lerp(dest.rgb, texcol.rgb, texcol.a);
#endif
#if defined(PMA_ON)
                texcol.rgb *= dest.a;
#endif                
                return texcol;
			}

