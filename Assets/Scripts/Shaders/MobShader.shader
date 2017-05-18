Shader "Custom/MobShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		[MaterialToggle] _highlight("Selected", Float) = 0
		[MaterialToggle] _inactive("Inactive", Float) = 0
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float _inactive;
			float _highlight;

			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				fixed4 temp = c;

				float gray = (c.r * .2989f + c.g * .587f + c.b * .114f);
				if (_highlight == 1) {
					temp.r = abs(sin(_Time[1] * 2)) * _Color.r;
					temp.g = abs(sin(_Time[1] * 2)) * _Color.g;
				}
				else if (_inactive == 1) {
					temp = (.389, .1465, .4645, .2) * _Color;
				}


				o.Albedo = temp.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
