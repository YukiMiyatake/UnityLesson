Shader "Instancing"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" "DisableBatching" = "True"}

		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_instancing

			#include "UnityCG.cginc"
			#include "UnityStandardParticleInstancing.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 color : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};


			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)

			v2f vert(appdata v)
			{
				UNITY_SETUP_INSTANCE_ID(v);

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}

			ENDCG
		}

		Pass
		{
			Tags { "LightMode" = "ForwardAdd" }
			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_instancing

			#include "UnityCG.cginc"
			#include "UnityStandardParticleInstancing.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 color : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};


			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)

			v2f vert(appdata v)
			{
				UNITY_SETUP_INSTANCE_ID(v);

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}

			ENDCG
		}

//*
		Pass
		{
			Name "SHADOW_CASTER"
			Tags{ "LightMode" = "ShadowCaster" }

			Lighting Off
			ColorMask RGB
			Fog{ Mode Off }

			Offset 1, 1
			Cull Off

			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#pragma multi_compile_instancing

			#include "UnityCG.cginc"
	//		#include "UnityStandardParticleInstancing.cginc"

			//#pragma multi_compile_shadowcaster


			struct v2f {
				V2F_SHADOW_CASTER;
				//UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			v2f vertShadowCaster(appdata_base v) {
				//UNITY_SETUP_INSTANCE_ID(v);

				v2f o = (v2f)0;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 fragShadowCaster(v2f i) : SV_TARGET{
				SHADOW_CASTER_FRAGMENT(i)
			}


			ENDCG


		}

		//*/
	}
}