Shader "NXT/Transparent" {

	////////////////////////////
	/// Anne's Special Shader///
	////////////////////////////

	Properties {

		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0, 1)) = 0.5
		_Specular ("Specular (R), Gloss (G))", 2D) = "black" {}
		_BumpMap ("Normal Map (RGB)", 2D) = "bump" {}
	}
	SubShader {

		Tags {

			"Queue"="Transparent"
			"RenderType"="Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha
	
		CGPROGRAM

		#pragma surface surf BlinnPhong
		#pragma exclude_renderers flash xbox360 ps3

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Specular;
		fixed4 _Color;
		half _Shininess;

		struct Input {

			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_Specular;
		};

		void surf (Input IN, inout SurfaceOutput o) {

			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 specTex = tex2D(_Specular, IN.uv_Specular);
			o.Albedo = tex.rgb * _Color.rgb;
			o.Alpha = tex.a * _Color.a;
			o.Specular = specTex.r;
			o.Gloss = specTex.g;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)); 
		}
		ENDCG
	}
	FallBack "Transparent/Bumped Specular"
}
