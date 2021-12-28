Shader "MyShader/Diffuse With LightProbes" {
	Properties{ [NoScaleOffset] _MainTex("Texture", 2D) = "white" {} }
		SubShader{
			Pass {
				Tags {
					"LightMode" = "ForwardBase"
				}
				CGPROGRAM
				#pragma vertex v
				#pragma fragment f
				#include "UnityCG.cginc"
				#include "UnityStandardUtils.cginc" //For access to ShadeSHPerPixel if I need to use it.
				sampler2D _MainTex;

				struct appdata_t {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float3 worldPos : TEXCOORD2;
					float3 worldNormal : TEXCOORD3;
				};

				v2f v(appdata_t vertex_data) {
					v2f o;
					o.vertex = UnityObjectToClipPos(vertex_data.vertex);
					o.uv = vertex_data.texcoord;
					UNITY_TRANSFER_FOG(o, o.vertex);
					o.worldNormal = UnityObjectToWorldNormal(vertex_data.normal);
					o.worldPos = mul(unity_ObjectToWorld, vertex_data.vertex).xyz;
					return o;
				}
				fixed4 f(v2f input_fragment) : SV_Target {
					fixed4 col = tex2D(_MainTex, input_fragment.uv);
					half3 ambient = ShadeSH9(half4(input_fragment.worldNormal, 1.0));
					col.xyz *= ambient;
					// apply fog
					UNITY_APPLY_FOG(input_fragment.fogCoord, col);
					return col;
				}
				ENDCG
			}
		}
}