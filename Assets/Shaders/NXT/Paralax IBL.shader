Shader "NXT/Parallax IBL" {
Properties {
	_Color ("Main Color", Color) = (0, 0, 0, 0)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_Shininess ("Shininess", Range (0, 2)) = 1
	_Specular ("Specular (R), Gloss (G), Reflection Strength (B)", 2D) = "black" {}
	_Rough ("Roughness", Range (0.1, 2)) = 1
	_BumpMap ("Normal Map (RGB)", 2D) = "bump" {}
	
	_Parallax ("Height", Range (0.005, 0.08)) = 0.02
	_ParallaxMap ("Heightmap (A)", 2D) = "black" {}
	
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_Cube ("IBL Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 600
	
CGPROGRAM
#pragma surface surf BlinnPhong approxview halfasview noforwardadd
#pragma exclude_renderers flash xbox360 ps3
#pragma target 3.0
#pragma glsl

sampler2D _MainTex;
sampler2D _Specular;
sampler2D _BumpMap;
samplerCUBE _Cube;
sampler2D _ParallaxMap;

fixed4 _Color;
fixed4 _ReflectColor;
half _Shininess;
float _Parallax;
float _Rough;

struct Input {
	float2 uv_MainTex;
	float2 uv_Specular;
	float2 uv_BumpMap;
	float3 worldRefl;
	float3 viewDir;
	INTERNAL_DATA
};

void surf (Input IN, inout SurfaceOutput o) {
	// Paralax
	half h = tex2D (_ParallaxMap, IN.uv_BumpMap).w;
	float2 offset = ParallaxOffset (h, _Parallax, IN.viewDir);
	IN.uv_MainTex += offset;
	IN.uv_BumpMap += offset;
	// Albedo
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 specTex = tex2D(_Specular, IN.uv_Specular);
	o.Albedo = tex.rgb * _Color.rgb;
	// Specular
	o.Specular = specTex.r * _Shininess;
	o.Gloss = specTex.g * _Shininess;
	// Normal Map
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) * _Rough;
	// IBL
	float3 worldRefl = WorldReflectionVector (IN, o.Normal);
	fixed4 reflcol = texCUBE (_Cube, worldRefl);
	o.Emission = reflcol.rgb * _ReflectColor.rgb * specTex.b;
}
ENDCG
}

FallBack "Reflective/Bumped Specular"
}
