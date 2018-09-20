Shader "Custom/PPHatchShadow"
{

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _RT1 ("Texture", 2D) = "white" {}
        _RT2 ("Texture", 2D) = "white" {}
        _RT3 ("Texture", 2D) = "white" {}
        _RT4 ("Texture", 2D) = "white" {}
        _RT5 ("Texture", 2D) = "white" {}
        _RT6 ("Texture", 2D) = "white" {}
        _RT7 ("Texture", 2D) = "white" {}
	}
    
	SubShader
	{
		Cull Off ZWrite Off ZTest Off
//        Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
//                SHADOW_COORDS(1)
			};

//            uniform float4 _ShadowMapTexture_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
            sampler2D _RT1;
            

			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 shadow = tex2D( _RT1, i.uv);
				fixed4 col = tex2D(_MainTex, i.uv);
//                float l = LIGHT_ATTENUATION(i);
				return float4( col.xyz * shadow.x, 1);
              //  return fixed4( col.xyz, 1);
			}
			ENDCG
		}
	}
}
