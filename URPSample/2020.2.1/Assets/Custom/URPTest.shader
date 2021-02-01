Shader "MyShader/URPTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
//            Tags { "LightMode" = "ForwardLit" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
           // #pragma multi_compile_fog

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
//#pragma shader_feature _RECEIVE_SHADOWS_OFF

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
//#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile _ _SHADOWS_SOFT

//#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
//#pragma multi_compile _ DIRLIGHTMAP_COMBINED
//#pragma multi_compile _ LIGHTMAP_ON

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            //#include "Assets/Shader/MyAutoLight.cginc"



#pragma multi_compile_fwdbase



            struct appdata
            {
                float4 vertex               : POSITION;
                float2 uv                   : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv                   : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                SHADOW_COORDS(2)
                float4 vertex               : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

       

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                TRANSFER_SHADOW(o);
//                o._ShadowCoord = float4(worldPos, 1);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //LightColor
//                fixed4 lightCol = (_LightColor0.rgb * LIGHT_ATTENUATION(i), 1);
                // sample the texture
                fixed shadow = SHADOW_ATTENUATION(i);
//                fixed4 col = tex2D(_MainTex, i.uv) * shadow;
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= shadow;
//                col.rgb *= clamp(shadow + 0.5, 0.0, 1.0);

                // apply fog
               // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual

            CGPROGRAM
            #pragma target 3.0


            #pragma vertex vertShadowCaster
            #pragma fragment fragShadowCaster


            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };

            VertexOutput vertShadowCaster(VertexInput v) {

                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 fragShadowCaster(VertexOutput i) : SV_TARGET {
                return 0;
            }


            ENDCG

        }

    }
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.SimpleLitRimShader"
}
