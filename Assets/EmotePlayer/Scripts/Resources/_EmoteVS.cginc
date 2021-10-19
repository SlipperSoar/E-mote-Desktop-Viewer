#if defined(MESH_DEFORMATION_ON)
    sampler2D_float _MeshDeformerParamsTex;
    float4 _MeshDeformerParamsTex_ST;
    float4 _MeshDeformerParamsSampleArg;
    float _MeshDeformerIndexArray[16];
    float _MeshDeformerIndexArraySize;
    float4 _MeshDeformerOffset;
    static int MESH_DEFORMER_PARAMS_PER_LINE = (4 + 4 + 4 * 4 * 2) / 4;

    float4 fetchMeshDeformerElement(float index) {
           return tex2Dlod(_MeshDeformerParamsTex, float4(_MeshDeformerParamsSampleArg.xy * (index + 0.5), 0, 0));
    }

    float2 calcBezierPatch(float meshDeformerIndex, float2 pos) {
           float baseIndex = meshDeformerIndex * MESH_DEFORMER_PARAMS_PER_LINE;
           float4 invMtx = fetchMeshDeformerElement(baseIndex);
           float4 invOffset = fetchMeshDeformerElement(baseIndex + 1);
           float4 bp0 = fetchMeshDeformerElement(baseIndex + 2);
           float4 bp1 = fetchMeshDeformerElement(baseIndex + 3);
           float4 bp2 = fetchMeshDeformerElement(baseIndex + 4);
           float4 bp3 = fetchMeshDeformerElement(baseIndex + 5);
           float4 bp4 = fetchMeshDeformerElement(baseIndex + 6);
           float4 bp5 = fetchMeshDeformerElement(baseIndex + 7);
           float4 bp6 = fetchMeshDeformerElement(baseIndex + 8);
           float4 bp7 = fetchMeshDeformerElement(baseIndex + 9);

           float2 uv = pos;
           uv += invOffset.xy;
           uv = float2(invMtx.x * uv.x + invMtx.y * uv.y, invMtx.z * uv.x + invMtx.w * uv.y);

           float2 iuv = 1.0f - uv;

#if 0
           // original code
           float x0 = iuv.x * iuv.x * iuv.x;
           float x1 = iuv.x * iuv.x *  uv.x * 3.0f;
           float x2 = iuv.x *  uv.x *  uv.x * 3.0f;
           float x3 =  uv.x *  uv.x *  uv.x;

           float2 a0 = bp0.xy * x0 + bp0.zw * x1 + bp1.xy * x2 + bp1.zw * x3;
           float2 a1 = bp2.xy * x0 + bp2.zw * x1 + bp3.xy * x2 + bp3.zw * x3;
           float2 a2 = bp4.xy * x0 + bp4.zw * x1 + bp5.xy * x2 + bp5.zw * x3;
           float2 a3 = bp6.xy * x0 + bp6.zw * x1 + bp7.xy * x2 + bp7.zw * x3;

           float y0 = iuv.y * iuv.y * iuv.y;
           float y1 = iuv.y * iuv.y *  uv.y * 3.0f;
           float y2 = iuv.y *  uv.y *  uv.y * 3.0f;
           float y3 =  uv.y *  uv.y *  uv.y;

           uv = a0 * y0 + a1 * y1 + a2 * y2 + a3 * y3;
#else
           // optimized version
           float4 x =
                 float4(iuv.x, iuv.x, iuv.x, uv.x ) *
                 float4(iuv.x, iuv.x,  uv.x, uv.x ) *
                 float4(iuv.x,  uv.x,  uv.x, uv.x ) *
                 float4( 1.0f,  3.0f,  3.0f, 1.0f );

           float4 a0 =
                  float4(bp0.xy, bp2.xy) * x.x +
                  float4(bp0.zw, bp2.zw) * x.y +
                  float4(bp1.xy, bp3.xy) * x.z +
                  float4(bp1.zw, bp3.zw) * x.w;
           float4 a1 =
                  float4(bp4.xy, bp6.xy) * x.x +
                  float4(bp4.zw, bp6.zw) * x.y +
                  float4(bp5.xy, bp7.xy) * x.z +
                  float4(bp5.zw, bp7.zw) * x.w;

           float4 y =
                 float4(iuv.y, iuv.y, iuv.y, uv.y ) *
                 float4(iuv.y, iuv.y,  uv.y, uv.y ) *
                 float4(iuv.y,  uv.y,  uv.y, uv.y ) *
                 float4( 1.0f,  3.0f,  3.0f, 1.0f );

           uv = a0.xy * y.x + a0.zw * y.y + a1.xy * y.z + a1.zw * y.w;
#endif

           return uv;
    }

#endif // defined(MESH_DEFORMATION_ON)

			struct my_appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
#if defined(VERTCOLOR_SINGLE) || defined(VERTCOLOR_DOUBLE)
				fixed4 color : COLOR;
#endif
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
#if defined(VERTCOLOR_SINGLE) || defined(VERTCOLOR_DOUBLE)
				fixed4 color : COLOR;
#endif
#if defined(REFALPHAMASK_ON) || defined(REFALPHAMASK_ON2) || defined(BBM_ENABLED)
                float4 scrPos : TEXCOORD1;
#endif
			};

			v2f vert (my_appdata v)
			{
				v2f o;
                                o.pos = v.vertex;
#if defined(MESH_DEFORMATION_ON)
                                int loopCount = int(_MeshDeformerIndexArraySize);
                                for (int i = 0; i < loopCount; i++)
                                    o.pos.xy = calcBezierPatch(_MeshDeformerIndexArray[i], o.pos.xy);
                                o.pos.xy += _MeshDeformerOffset.xy;
                                
#endif //  defined(MESH_DEFORMATION_ON)
                                o.pos = UnityObjectToClipPos (o.pos);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
#if defined(VERTCOLOR_SINGLE) || defined(VERTCOLOR_DOUBLE)
				o.color = v.color;
#endif
#if defined(REFALPHAMASK_ON) || defined(REFALPHAMASK_ON2) || defined(BBM_ENABLED)
                o.scrPos = ComputeScreenPos(o.pos);
#endif
				return o;
			}
