Shader "Custom/WaveShader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_HeightMap ("Height map", 2D) = "white" {}
		
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard nolightmap vertex:vert alpha
		#pragma target 4.6
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _HeightMap;

		struct Input {
			float2 uv_MainTex;
		};

		//half _Glossiness;
		//half _Metallic;

		void vert (inout appdata_full v) {
			//#if !defined(SHADER_API_OPENGL)

			const float border = 0.1f;
			float magn = 1;
			float minLimit = min(v.texcoord.x, v.texcoord.y);
			float maxLimit = max(v.texcoord.x, v.texcoord.y);
			if (minLimit < border) magn = min(magn, minLimit / border);
			if (maxLimit > 1 - border) magn = min(magn, 1 - (maxLimit - (1 - border)) / border);

			float4 tex = tex2Dlod (_HeightMap, float4(v.texcoord.xy,0,0));
			v.vertex.y += length( tex.rgb ) * 2.0 * magn;
			//#endif
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = tex2D(_HeightMap, IN.uv_MainTex);
			// Metallic and smoothness come from slider variables
			o.Metallic = 0;
			o.Smoothness = 0;
			o.Occlusion = 0;
			o.Alpha = 0;

			o.Emission = tex2D(_HeightMap, IN.uv_MainTex).rgb * 3;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
