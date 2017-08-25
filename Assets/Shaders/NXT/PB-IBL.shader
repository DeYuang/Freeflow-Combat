Shader "NXT/Physically Based (IBL)" {
Properties {
	_Color ("Main Color", Color) = (0, 0, 0, 0)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_Shininess ("Shininess", Range (0, 2)) = 0.5
	_Specular ("Specular (R), Gloss (G), Roughness(B)", 2D) = "black" {}
	_BumpMap ("Normal Map (RGB)", 2D) = "bump" {}
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_Cube ("Reflection Cubemap", Cube) = "" { TexGen CubeReflect }
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 400
	Zwrite On
	Cull back
CGPROGRAM
#pragma surface surf BlinnPhong approxview halfasview
#pragma exclude_renderers flash xbox360 ps3
#pragma target 3.0
#pragma glsl
//input limit (8) exceeded, shader uses 9
//#pragma exclude_renderers d3d11_9x

sampler2D _MainTex;
sampler2D _Specular;
sampler2D _BumpMap;
samplerCUBE _Cube;

float4 _Color;
float4 _ReflectColor;
float _Shininess;

struct Input {
	float2 uv_MainTex;
	float2 uv_Specular;
	float2 uv_BumpMap;
	float3 worldRefl;
	INTERNAL_DATA
};

void surf (Input IN, inout SurfaceOutput o) {
	float4 tex = tex2D(_MainTex, IN.uv_MainTex);
	float4 specTex = tex2D(_Specular, IN.uv_Specular);
	float4 c = tex * _Color;
	o.Albedo = c.rgb;
	
	o.Specular = specTex.r;
	o.Gloss = specTex.g * _Shininess;
	
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	
	float3 worldRefl = WorldReflectionVector (IN, o.Normal);
	float4 reflcol = texCUBE (_Cube, worldRefl);
	reflcol *= tex.a;
	o.Emission = reflcol.rgb * _ReflectColor.rgb * specTex.b;
	//o.Alpha = reflcol.a * _ReflectColor.a;
}
ENDCG
}

FallBack "Reflective/Bumped Diffuse"
}
