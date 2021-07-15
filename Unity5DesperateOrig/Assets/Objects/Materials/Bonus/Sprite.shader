Shader "Cg  shader for billboards" {
   Properties {
      _MainTex ("Texture Image", 2D) = "white" {}
	  _OtherTex("Texture Glow",2D)="white"{}
	  _MaskTex("Texture Mask",2D)="white"{}
	  _Size( "Size",Float) =1.5

	  _Emision("Emision",Float)=1.0
   }
   SubShader {
      Pass {  
		ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         // User-specified uniforms            
         uniform sampler2D _MainTex;
		 uniform sampler2D _OtherTex; 
		 uniform sampler2D _MaskTex; 
		 uniform float _Size;
		 uniform float _Emision;        
 
         struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.pos = mul(UNITY_MATRIX_P, 
              mul(UNITY_MATRIX_MV, float4(0.0, 1.0, 0.0, 1.0))
              - float4(input.vertex.x, input.vertex.z, 0.0, 0.0)*_Size);
 
            output.tex = input.tex;
 
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
			float4 texColor = tex2D(_MainTex,float2(input.tex.xy));
			
			float4 texGlow = tex2D(_OtherTex , float2(input.tex.xy));
			float4 texMask = tex2D(_MaskTex , float2(input.tex.xy));
			float4 glow = texGlow.rgba*_Emision+texMask.rgba*texMask.a;
			float4 rast = glow.rgba*glow.a+texColor.rgba*texColor.a;
            return  rast;
         }


         ENDCG
      }
   }
}


