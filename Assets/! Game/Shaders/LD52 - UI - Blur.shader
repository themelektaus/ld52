// Base shader found at https://stackoverflow.com/questions/29030321/unity3d-blur-the-background-of-a-ui-canvas

Shader "LD52/UI/Blur"
{
	Properties
	{
		[HideInInspector] _MainTex("Masking Texture", 2D) = "white" {}
		[HideInInspector] _Scale("Scale", Float) = 1

		_Weight("Weight", Range(0, 1)) = 1
		_Size("Blur", Range(0, 6)) = 3
		_MultiplyColor("Multiply Tint color", Color) = (1, 1, 1, 1)
		_AdditiveColor("Additive Tint color", Color) = (0, 0, 0, 0)
		_Saturation("Saturation", Range(0, 1)) = 1
	}

	Category
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Opaque"
		}

		SubShader
		{
			GrabPass
			{
				"_HBlur"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;
					float2 uvmain : TEXCOORD1;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif

					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;

					o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}

				sampler2D _HBlur;
				float4 _HBlur_TexelSize;

				float _Scale;
				float _Weight;
				float _Size;

				float4 grabPixel(float4 uvgrab, float weight, float a)
				{
					float4 coords = float4(uvgrab.x + _HBlur_TexelSize.x * a * _Scale * _Weight * _Size, uvgrab.y, uvgrab.z, uvgrab.w);
					return tex2Dproj(_HBlur, UNITY_PROJ_COORD(coords)) * weight;
				}

				half4 frag(v2f i) : COLOR
				{
					half3 c = 0;

					for (float weight = -1; weight < 1; weight += .1)
						c += grabPixel(i.uvgrab, lerp(0, 0.1, 1 - abs(weight)), weight * 4);

					return half4(c, tex2D(_MainTex, i.uvmain).a);
				}
				ENDCG
			}
			
			GrabPass
			{
				"_VBlur"
			}

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
				};

				struct v2f {
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;
					float2 uvmain : TEXCOORD1;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif

					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;

					o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);

					return o;
				}

				sampler2D _VBlur;
				float4 _VBlur_TexelSize;

				float _Scale;
				float _Weight;
				float _Size;

				float4 _MultiplyColor;
				float4 _AdditiveColor;
				half _Saturation;

				float3 grabPixel(float4 uvgrab, float weight, float a)
				{
					float4 coords = float4(uvgrab.x, uvgrab.y + _VBlur_TexelSize.y * a * _Scale * _Weight * _Size, uvgrab.z, uvgrab.w);
					return tex2Dproj(_VBlur, UNITY_PROJ_COORD(coords)) * weight;
				}

				half4 frag(v2f i) : COLOR
				{
					half3 c = 0;
					
					for (float weight = -1; weight < 1; weight += .1)
						c += grabPixel(i.uvgrab, lerp(0, 0.1, 1 - abs(weight)), weight * 4);

					c = lerp(c, c * _MultiplyColor.rgb, _MultiplyColor.a * _Weight);
					c = lerp(c, c + _AdditiveColor.rgb, _AdditiveColor.a * _Weight);
					c = lerp(dot(c, float3(0.299, 0.587, 0.114)), c, lerp(1, _Saturation, _Weight));
					
					return half4(c, tex2D(_MainTex, i.uvmain).a);
				}
				ENDCG
			}
		}
	}
}