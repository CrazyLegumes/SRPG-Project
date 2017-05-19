Shader "Debug/UV 1" {
	Properties{
		Texture("Texture", 2D) = "white"{}
		_Color("Color", Color) = (1,1,1,1)

		[PerRendererData][MaterialToggle]CanFade("Can Fade", Float) = 0
	}
	SubShader{
		Tags{
			"Queue" = "Transparent"
	}
		Pass{

		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		// vertex input: position, UV
	struct appdata {
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata v) {
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.uv;

		return o;
	}
	sampler2D Texture;
	float4 _Color;
	float CanFade;
	float4 frag(v2f i) : SV_Target{

		float4 color = tex2D(Texture, i.uv);
		
		color.rgba *= _Color;
		if (CanFade == 1)
			color.a = abs(sin(_Time[1] * 2));
		else
			color.a = 1;
		
		return color;

	
	}
		ENDCG
	}
	}
}