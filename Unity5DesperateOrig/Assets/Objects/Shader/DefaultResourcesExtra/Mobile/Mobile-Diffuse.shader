// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/ZombiColor" {
Properties {
	_MainTex ("Main (RGB)", 2D) = "white" {}
	_Mask ("Mask (A)", 2D) = "white" {}
	_Color ("Color alpha (RGB)",Color) = (0.5,0.5,0.5,0.5)
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 	


}
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

sampler2D _MainTex;
sampler2D _Mask;
fixed4 _Color;
fixed _Cutoff;
struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 m = tex2D(_Mask, IN.uv_MainTex);	
	clip(m.rgb - _Cutoff);
	o.Albedo = c.rgb;
	o.Emission = (m.rgb-0.5)-_Cutoff;
	o.Alpha = m.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
