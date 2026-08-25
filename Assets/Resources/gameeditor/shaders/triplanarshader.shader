Shader "TriplanarShader" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _MainTex1 ("Base (RGB)", 2D) = "white" {}
 _SizeX ("SizeX", Float) = 20
 _SizeY ("SizeY", Float) = 20
 _NX ("NX", Range(0,1)) = 1
 _NY ("NY", Range(0,1)) = 1
 _NZ ("NZ", Range(0,1)) = 1
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}