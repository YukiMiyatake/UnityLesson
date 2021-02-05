Shader "MyShader/URPUnlit"
{
	Properties
	{
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


			struct Attributes
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
			};
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
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
				return output;
			}

			half4 frag(Varyings input) : SV_TARGET
			{
				return half4(1,1,1,1);
			}

			ENDHLSL
		}


	}
}
