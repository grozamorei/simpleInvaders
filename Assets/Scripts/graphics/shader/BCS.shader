Shader "Custom/BCS" {
	Properties 
	{
		_MainTex ("", 2D) = "white" {}
		_Brightness ("", Range(0.0, 2.0)) = 1.0
		_Saturation ("", Range(0.0, 2.0)) = 1.0
		_Contrast ("", Range(0.0, 1.0)) = 1.0
	}

	SubShader 
	{
		Pass 
		{
			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"


			uniform sampler2D _MainTex;
			uniform fixed _Brightness;
			uniform fixed _Saturation;
			uniform fixed _Contrast;

			float3 bcs(float3 color, float brightness, float saturation, float contrast) {
				float3 luminanceCoeff = float3(0.2125, 0.7154, 0.0721);

				float3 brightnessColor = color * brightness;

				float intensityF = dot(brightnessColor, luminanceCoeff);
				float3 intensity = float3(intensityF, intensityF, intensityF);
				float3 saturationColor = lerp(intensity, brightnessColor, saturation);
				
				float3 contrastColor = lerp(float3(0.5, 0.5, 0.5), saturationColor, contrast);

				return contrastColor;
			}

			fixed4 frag(v2f_img i) : COLOR {
				fixed4 renderTex = tex2D(_MainTex, i.uv);

				renderTex.rgb = bcs(renderTex.rgb, _Brightness, _Saturation, _Contrast);
				return renderTex;
			}

			ENDCG
		}
	} 
	FallBack Off
}
