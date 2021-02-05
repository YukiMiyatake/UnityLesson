Shader "MyShader/URPUnlit"
{
	Properties
	{
			[NoScaleOffset] MainTex("_MainTex", 2D) = "white" {}
			[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
			[HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
			[HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}


	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Opaque"
			"UniversalMaterialType" = "Unlit"
			"Queue" = "Geometry"
		}
		Pass
		{
			Name "Pass"
			Tags
			{
			}

			// Render State
			Cull Back
			Blend One Zero
			ZTest LEqual
			ZWrite On


			HLSLPROGRAM

			// Pragmas
			#pragma target 4.5
			#pragma exclude_renderers gles gles3 glcore
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"

			TEXTURE2D(MainTex);
			SAMPLER(samplerMainTex);

			struct Attributes
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
			};
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
			};

			
			float3 TransformObjectToWorld(float3 positionOS)
			{
				return mul(unity_ObjectToWorld, float4(positionOS, 1.0)).xyz;
			}

			float4 TransformWorldToHClip(float3 positionWS)
			{
				return mul(unity_MatrixVP, float4(positionWS, 1.0));
			}

			Varyings vert(Attributes input)
			{
				Varyings output = (Varyings)0;
				output.positionCS = TransformWorldToHClip(TransformObjectToWorld(input.positionOS));
				output.texCoord0 = input.uv0;
				return output;
			}

			half4 frag(Varyings input) : SV_TARGET
			{
				half4 o = SAMPLE_TEXTURE2D(MainTex, samplerMainTex, input.texCoord0.xy);

				return o;
			}

			ENDHLSL
		}


	}
}
