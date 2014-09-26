Shader "Custom/OldFilmShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf OldFilmPhong

		uniform sampler2D _MainTex;

		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 original = tex2D(_MainTex, IN.uv_MainTex);
			fixed Y = dot (fixed3(0.299, 0.587, 0.114), original.rgb);
			fixed4 sepiaConvert = float4 (0.191, -0.054, -0.221, 0.0);
			fixed4 output = sepiaConvert + Y;
			o.Albedo = output.rgb;
			o.Alpha = 0.1f;
		}
		
		inline float4 LightingOldFilmPhong (SurfaceOutput s, fixed3
		lightDir, fixed atten) {
			float difLight = max(0, dot (s.Normal, lightDir));
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2);
			col.a = s.Alpha;
			return col;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
