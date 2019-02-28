﻿Shader "Instancing"
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
	}
}