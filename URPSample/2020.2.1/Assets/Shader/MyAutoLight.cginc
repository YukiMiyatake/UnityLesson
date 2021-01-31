//sampler2D _ShadowMapTexture;
//sampler2D _ScreenSpaceShadowMap;
//sampler2D _ScreenSpaceShadowmapTexture;

sampler2D _MainLightShadowmapTexture;
//UNITY_DECLARE_SCREENSPACE_SHADOWMAP(_ScreenSpaceShadowmapTexture);

#define TRANSFER_SHADOW(a) a._ShadowCoord = mul( unity_WorldToShadow[0], mul( unity_ObjectToWorld, v.vertex ) );

inline fixed unitySampleShadow2(unityShadowCoord4 shadowCoord)
{
    fixed shadow = UNITY_SAMPLE_SCREEN_SHADOW(_MainLightShadowmapTexture, shadowCoord);
    return shadow;
}
/*
inline float4 unitySampleShadow2(float4 pos) {
	float4 o = pos * 0.5f;
	o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
	o.zw = pos.zw;
	return o;
}

inline fixed unitySampleShadow2(unityShadowCoord4 shadowCoord)
{

    
    unityShadowCoord dist = SAMPLE_DEPTH_TEXTURE(_MainLightShadowmapTexture, shadowCoord.xy);
    // tegra is confused if we use _LightShadowData.x directly
    // with "ambiguous overloaded function reference max(mediump float, float)"
    unityShadowCoord lightShadowDataX = _LightShadowData.x;
    unityShadowCoord threshold = shadowCoord.z;
    return max(dist > threshold, lightShadowDataX);
    
//#endif
}


inline fixed unitySampleShadow2(unityShadowCoord4 shadowCoord)
{

    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
    half4 shadowParams = GetMainLightShadowParams();
    return SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, false);


}
*/



#define SHADOW_COORDS(idx1) unityShadowCoord4 _ShadowCoord : TEXCOORD##idx1;
#define SHADOW_ATTENUATION(a) unitySampleShadow2(a._ShadowCoord)
//#define SHADOW_COORDS(idx1) float4 shadowCoord : TEXCOORD##idx1;
//#define TRANSFER_SHADOW(a) a.shadowCoord = CustomCalculateScreenPos(a.pos)
//#define SHADOW_ATTENUATION(i) tex2D(_ScreenSpaceShadowMap, i.shadowCoord.xy/i.shadowCoord.w).x
/*
inline float4 CustomCalculateScreenPos(float4 pos) {
	float4 o = pos * 0.5f;
	o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
	o.zw = pos.zw;
	return o;
}
*/