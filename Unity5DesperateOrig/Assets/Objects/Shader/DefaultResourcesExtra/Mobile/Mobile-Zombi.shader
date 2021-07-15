Shader "Custom/Burn"
{
         Properties
         {
                 _MainTex ("Base (RGB)", 2D) = "white" {}
				 _Mask ("Mask (A)",2D) = "white" {}
                 _Cutoff("Cutoff", Range(0, 1.1)) = 0
                 _BurnShift("BurnShift", Range(0, 0.1)) = 0.01
                 _BurnColor("Cutoff", Color) = (1, 1, 0, 1)
				 _BloodColor("Blood", Color) = (1, 0, 0, 1)
         }
         SubShader
         {
                Tags { "RenderType"="Opaque" }
                 LOD 200
                
                 CGPROGRAM
                 #pragma surface surf Lambert

                 sampler2D _MainTex;
				 sampler2D _Mask;
                 fixed _Cutoff;
                 fixed _BurnShift;
                 fixed4 _BurnColor;
				 fixed _BloodShift;
				 fixed4 _BloodColor;

                 struct Input
                 {
                         float2 uv_MainTex;
                 };

                 void surf (Input IN, inout SurfaceOutput o)
                 {
                         half4 c = tex2D (_MainTex, IN.uv_MainTex);
						 half4 m = tex2D (_Mask, IN.uv_MainTex);
						 fixed a = m.rgb-_Cutoff;
                         fixed bPos = a -_BurnShift;
						 fixed rPos = a -_Cutoff;
                         fixed isBurn = 1-step(-bPos, 0);
						 fixed isBlood = 1-step(-rPos,0);
						 
						 
						 fixed3 color = lerp(c.rgb, _BloodColor.rgb, isBlood);
                         o.Albedo = lerp(color.rgb, _BurnColor.rgb, isBurn);

                         o.Emission = lerp(fixed3(0,0,0), _BurnColor.rgb, isBurn);

                         //fixed a = m.rgb-_Cutoff;
                         o.Alpha = a;
                         clip(a);
                 }
                 ENDCG
         }
         FallBack "Diffuse"
}
