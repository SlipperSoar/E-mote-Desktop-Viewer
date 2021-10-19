#if defined(TEXTURE_AST_OFF)
    half4 emoteTex2D(sampler2D tex, half2 uv) {
        return tex2D(tex, uv);
    }

    half4 emoteTex2DAlpha(sampler2D tex, half2 uv) {
        half a = tex2D(tex, uv).a;
        return half4(a, a, a, a);
    }
#endif //  defined(TEXTURE_AST_OFF)


#if defined(TEXTURE_AST_ON)
    half4 emoteTex2D(sampler2D tex, half2 uv) {
        half4 texcol;
        texcol.rgb = tex2D(tex, half2(uv.x / 2, uv.y)).rgb;
        texcol.a = tex2D(tex, half2(uv.x / 2 + 0.5, uv.y)).r;
        return texcol;
    }

    half4 emoteTex2DAlpha(sampler2D tex, half2 uv) {
        half a = tex2D(tex, half2(uv.x / 2 + 0.5, uv.y)).r;
        return half4(a, a, a, a);
    }
#endif //  defined(TEXTURE_AST_ON)
