// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CreationSilexars"
{
    Properties
    {
        _AspectRatio ("Aspect Ratio", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"
                "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        

        Pass
        {
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
                
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;


                // this applies the model transformation matrix to the vertex position, converting
                // the position to world (model) coordinates
                o.worldPos = mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 1));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            float _AspectRatio;

            fixed4 frag (v2f iin) : SV_Target
            {
                float t = _Time.y * .4;

                float3 c;
                float l, z=t;

                for (int i = 0; i < 3; i++)
                {
                    float2 uv = iin.uv;
                    float2 p = iin.uv -.5;

                    z += .07;

                    p.x *= _AspectRatio;
                    // distance from center, given the centered coordinate system.
                    // much better than distance method.
                    l = length(p);
                    
                    uv += p*8/l*(sin(z*0.7)+1)*abs(sin(l*9-z-z));

                    //float2 lengthvector = float2((uv.x % 1), (uv.y % 1)) - .5;
                    //float outp = 0.01/length(lengthvector);
                    float outp = 0.01/length((uv % 1) * .5);
                    c[i] = outp;
                }

                float3 col =  float3(c/l);

                if (length(col) < 0.5)
                {
                    return float4(1,1,1,0);
                }

                float4 colAlpha = float4(col.xyz, 1 - l);

                
                /// gaussian blur?

                return colAlpha;


            }
            ENDCG
        }
        // Vertical blur
            GrabPass {                         
                Tags { "LightMode" = "Always" }
            }
            Pass {
                Tags { "LightMode" = "Always" }
               
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
               
                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord: TEXCOORD0;
                };
               
                struct v2f {
                    float4 vertex : POSITION;
                    float4 uvgrab : TEXCOORD0;
                };
               
                v2f vert (appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                    #else
                    float scale = 1.0;
                    #endif
                    o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                    o.uvgrab.zw = o.vertex.zw;
                    return o;
                }
               
                sampler2D _GrabTexture;
                float4 _GrabTexture_TexelSize;
                float _Size;
               
                half4 frag( v2f i ) : COLOR {
//                  half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
//                  return col;
                   
                    half4 sum = half4(0,0,0,0);
 
                    #define GRABPIXEL(weight,kernely) tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely*_Size, i.uvgrab.z, i.uvgrab.w))) * weight
 
                    //G(X) = (1/(sqrt(2*PI*deviation*deviation))) * exp(-(x*x / (2*deviation*deviation)))
                   
                    sum += GRABPIXEL(0.05, -4.0);
                    sum += GRABPIXEL(0.09, -3.0);
                    sum += GRABPIXEL(0.12, -2.0);
                    sum += GRABPIXEL(0.15, -1.0);
                    sum += GRABPIXEL(0.18,  0.0);
                    sum += GRABPIXEL(0.15, +1.0);
                    sum += GRABPIXEL(0.12, +2.0);
                    sum += GRABPIXEL(0.09, +3.0);
                    sum += GRABPIXEL(0.05, +4.0);
                   
                    return sum;
                }
                ENDCG
            }
           
            // Distortion
            GrabPass {                         
                Tags { "LightMode" = "Always" }
            }
            Pass {
                Tags { "LightMode" = "Always" }
               
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
               
                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord: TEXCOORD0;
                };
               
                struct v2f {
                    float4 vertex : POSITION;
                    float4 uvgrab : TEXCOORD0;
                    float2 uvbump : TEXCOORD1;
                    float2 uvmain : TEXCOORD2;
                };
               
                float _BumpAmt;
                float4 _BumpMap_ST;
                float4 _MainTex_ST;
               
                v2f vert (appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                    #else
                    float scale = 1.0;
                    #endif
                    o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                    o.uvgrab.zw = o.vertex.zw;
                    o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
                    o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
                    return o;
                }
               
                fixed4 _Color;
                sampler2D _GrabTexture;
                float4 _GrabTexture_TexelSize;
                sampler2D _BumpMap;
                sampler2D _MainTex;
               
                half4 frag( v2f i ) : COLOR {
                    // calculate perturbed coordinates
                    half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg; // we could optimize this by just reading the x  y without reconstructing the Z
                    float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
                    i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
                   
                    half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                    half4 tint = tex2D( _MainTex, i.uvmain ) * _Color;
                   
                    return col * tint;
                }
                ENDCG
            }
        }

    }

