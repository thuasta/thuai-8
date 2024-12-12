// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DLNK Shaders/ASE/TopBottomSimple"
{
	Properties
	{
		_Color("Color", Color) = (0.8207547,0.8207547,0.8207547,0)
		_MainTex("Albedo", 2D) = "white" {}
		_BumpMap("BumpMap", 2D) = "bump" {}
		_BumpScale("BumpScale", Float) = 1
		_MetallicGlossMap("MetallicGlossMap", 2D) = "white" {}
		_Metallic("Metallic", Float) = 0
		_Glossiness("Glossiness", Float) = 0.5
		_OcclusionMap("Occlusion Map", 2D) = "white" {}
		_OcclusionStrength("Occlusion Strength", Float) = 0
		_DetailMask("DetailMask", 2D) = "white" {}
		_DetailAlbedoMap("DetailAlbedoMap", 2D) = "white" {}
		_DetailNormalMap("DetailNormalMap", 2D) = "bump" {}
		_DetailNormalMapScale("DetailNormalMapScale", Float) = 1
		_ColorTop("ColorTop", Color) = (0.8490566,0.8450516,0.8450516,0)
		_TopTiling("TopTiling", Float) = 1
		[Toggle]_UVWorld("UVWorld", Float) = 0
		_Ammount("Ammount", Float) = 0.5
		_Smooth("Smooth", Float) = 0.5
		_Clamp("Clamp", Vector) = (0,1,0,0)
		_AlbedoTop("AlbedoTop", 2D) = "white" {}
		_BumpMapTop("BumpMapTop", 2D) = "bump" {}
		_BumpMix("BumpMix", Float) = 1
		_BumpScaleTop("BumpScaleTop", Float) = 1
		_MetalnessTop("MetalnessTop", 2D) = "white" {}
		_GlossinessTop("GlossinessTop", Float) = 0.5
		_MetallicTop("MetallicTop", Float) = 0
		_OcclusionTop("Occlusion Top", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _BumpScale;
		uniform sampler2D _DetailNormalMap;
		uniform float4 _DetailNormalMap_ST;
		uniform float _DetailNormalMapScale;
		uniform sampler2D _DetailMask;
		uniform float4 _DetailMask_ST;
		uniform sampler2D _BumpMapTop;
		uniform float _UVWorld;
		uniform float _TopTiling;
		uniform float _BumpScaleTop;
		uniform float _BumpMix;
		uniform float _Ammount;
		uniform float2 _Clamp;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform float4 _ColorTop;
		uniform sampler2D _AlbedoTop;
		uniform float _Smooth;
		uniform sampler2D _MetallicGlossMap;
		uniform float4 _MetallicGlossMap_ST;
		uniform float _Metallic;
		uniform sampler2D _MetalnessTop;
		uniform float _MetallicTop;
		uniform float _Glossiness;
		uniform float _GlossinessTop;
		uniform sampler2D _OcclusionMap;
		uniform float4 _OcclusionMap_ST;
		uniform float _OcclusionTop;
		uniform float _OcclusionStrength;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float3 tex2DNode4 = UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ), _BumpScale );
			float2 uv_DetailNormalMap = i.uv_texcoord * _DetailNormalMap_ST.xy + _DetailNormalMap_ST.zw;
			float2 uv_DetailMask = i.uv_texcoord * _DetailMask_ST.xy + _DetailMask_ST.zw;
			float4 tex2DNode43 = tex2D( _DetailMask, uv_DetailMask );
			float3 lerpResult46 = lerp( tex2DNode4 , BlendNormals( tex2DNode4 , UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailNormalMap ), _DetailNormalMapScale ) ) , tex2DNode43.a);
			float2 temp_cast_0 = (_TopTiling).xx;
			float2 uv_TexCoord110 = i.uv_texcoord * temp_cast_0;
			float3 ase_worldPos = i.worldPos;
			float4 appendResult132 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float3 tex2DNode5 = UnpackScaleNormal( tex2D( _BumpMapTop, (( _UVWorld )?( ( appendResult132 * _TopTiling * 0.1 ) ):( float4( uv_TexCoord110, 0.0 , 0.0 ) )).xy ), _BumpScaleTop );
			float3 lerpResult120 = lerp( tex2DNode5 , BlendNormals( tex2DNode5 , tex2DNode4 ) , _BumpMix);
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 lerpResult11 = lerp( lerpResult46 , lerpResult120 , saturate( (0.0 + (( ase_normWorldNormal.y * _Ammount ) - _Clamp.x) * (1.0 - 0.0) / (_Clamp.y - _Clamp.x)) ));
			o.Normal = lerpResult11;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode2 = tex2D( _MainTex, uv_MainTex );
			float2 uv_DetailAlbedoMap = i.uv_texcoord * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			float4 lerpResult44 = lerp( ( _Color * tex2DNode2 ) , ( tex2DNode2 * tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap ) * _Color ) , tex2DNode43.a);
			float smoothstepResult118 = smoothstep( 0.0 , _Smooth , normalize( (WorldNormalVector( i , lerpResult11 )) ).y);
			float temp_output_16_0 = saturate( (0.0 + (( smoothstepResult118 * _Ammount ) - _Clamp.x) * (1.0 - 0.0) / (_Clamp.y - _Clamp.x)) );
			float4 lerpResult18 = lerp( lerpResult44 , ( _ColorTop * tex2D( _AlbedoTop, (( _UVWorld )?( ( appendResult132 * _TopTiling * 0.1 ) ):( float4( uv_TexCoord110, 0.0 , 0.0 ) )).xy ) ) , temp_output_16_0);
			o.Albedo = lerpResult18.rgb;
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
			float4 tex2DNode6 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap );
			float4 tex2DNode7 = tex2D( _MetalnessTop, (( _UVWorld )?( ( appendResult132 * _TopTiling * 0.1 ) ):( float4( uv_TexCoord110, 0.0 , 0.0 ) )).xy );
			float4 lerpResult14 = lerp( ( tex2DNode6 * _Metallic ) , ( tex2DNode7 * _MetallicTop ) , temp_output_16_0);
			o.Metallic = lerpResult14.r;
			float lerpResult19 = lerp( ( tex2DNode6.a * _Glossiness ) , ( tex2DNode7.a * _GlossinessTop ) , temp_output_16_0);
			o.Smoothness = lerpResult19;
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST.xy + _OcclusionMap_ST.zw;
			float4 tex2DNode48 = tex2D( _OcclusionMap, uv_OcclusionMap );
			float lerpResult51 = lerp( tex2DNode48.r , ( tex2DNode48.r + ( 1.0 - _OcclusionTop ) ) , temp_output_16_0);
			o.Occlusion = saturate( ( lerpResult51 + ( 1.0 - _OcclusionStrength ) ) );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows dithercrossfade 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.RangedFloatNode;111;-1494.636,698.1601;Inherit;False;Property;_TopTiling;TopTiling;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1175.863,90.2252;Inherit;False;Property;_Ammount;Ammount;16;0;Create;True;0;0;0;False;0;False;0.5;0.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-1295.381,-128.4;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;12;-1180.168,304.4252;Inherit;False;Property;_BumpScale;BumpScale;3;0;Create;True;0;0;0;False;0;False;1;0.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1486.523,121.6645;Inherit;False;Property;_DetailNormalMapScale;DetailNormalMapScale;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1211.368,451.3252;Inherit;False;Property;_BumpScaleTop;BumpScaleTop;22;0;Create;True;0;0;0;False;0;False;1;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-958.6176,430.6252;Inherit;True;Property;_BumpMapTop;BumpMapTop;20;0;Create;True;0;0;0;False;0;False;-1;None;2a2da984a6af369408b716da8bdd6296;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-1500.186,207.7091;Inherit;True;Property;_DetailNormalMap;DetailNormalMap;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1084.26,-66.76968;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;117;-1003.142,-260.8409;Inherit;False;Property;_Clamp;Clamp;18;0;Create;True;0;0;0;False;0;False;0,1;0,0.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;4;-958.4177,244.5252;Inherit;True;Property;_BumpMap;BumpMap;2;0;Create;True;0;0;0;False;0;False;-1;None;93b05dc19e074394b92362a5e6ee5ddc;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;121;-473.3607,472.8547;Inherit;False;Property;_BumpMix;BumpMix;21;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;47;-603.8795,351.5309;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;108;-934.0028,-12.66623;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;43;-1514.53,440.2991;Inherit;True;Property;_DetailMask;DetailMask;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;39;-598.3251,196.3061;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;10;-771.4171,146.74;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;46;-295.6988,239.7465;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;120;-292.3607,374.8547;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;11;-79.0016,296.8399;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;15;-288.3126,-131.5585;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;119;-101.9447,-14.76172;Inherit;False;Property;_Smooth;Smooth;17;0;Create;True;0;0;0;False;0;False;0.5;0.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;118;-621.4454,19.14252;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-449.608,86.1126;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-43.20768,1138.505;Inherit;False;Property;_OcclusionTop;Occlusion Top;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;-161.4265,896.813;Inherit;True;Property;_OcclusionMap;Occlusion Map;7;0;Create;True;0;0;0;False;0;False;-1;None;47db18e2f63886448bab79ebd25d39d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;57;61.87585,1259.351;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;105;-278.3647,53.16402;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-729.7998,-565.7997;Inherit;True;Property;_MainTex;Albedo;1;0;Create;False;0;0;0;False;0;False;-1;None;222db5a971f30374d8aa07285b03b6af;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-658.8688,-745.9743;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;0.8207547,0.8207547,0.8207547,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;56.62176,790.4156;Inherit;False;Property;_OcclusionStrength;Occlusion Strength;8;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-1027.37,-566.9212;Inherit;True;Property;_DetailAlbedoMap;DetailAlbedoMap;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;16;21.64855,115.5967;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;253.6533,1125.369;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-712.8995,-182.3;Inherit;True;Property;_AlbedoTop;AlbedoTop;19;0;Create;True;0;0;0;False;0;False;-1;None;384f9a1ad45e901438e288b77eb4a76d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-849.9683,923.2254;Inherit;False;Property;_GlossinessTop;GlossinessTop;24;0;Create;True;0;0;0;False;0;False;0.5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-847.3686,842.6254;Inherit;False;Property;_MetallicTop;MetallicTop;25;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-662.7689,-372.8746;Inherit;False;Property;_ColorTop;ColorTop;13;0;Create;True;0;0;0;False;0;False;0.8490566,0.8450516,0.8450516,0;0.4672482,0.5413404,0.5660378,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-352.0436,-647.963;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-341.6853,-499.8063;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-851.2681,746.4254;Inherit;False;Property;_Glossiness;Glossiness;6;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-664.6179,544.8253;Inherit;True;Property;_MetallicGlossMap;MetallicGlossMap;4;0;Create;True;0;0;0;False;0;False;-1;None;d46622e22b87d6e48ba58f52cd0f3dcc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;51;198.4846,911.2618;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-666.1178,732.2252;Inherit;True;Property;_MetalnessTop;MetalnessTop;23;0;Create;True;0;0;0;False;0;False;-1;None;b449395b5e3ec5e4fa31c87a752509ed;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-848.6683,665.8253;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;59;109.1635,699.7813;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-314.3682,543.6255;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-381.6689,-249.9746;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;44;-101.0026,-561.2209;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-315.6683,641.775;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;269.4158,718.1709;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-318.2681,853.6758;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-315.668,756.8263;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;-94.6681,554.0255;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;19;-118.0685,698.3256;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;54;294.3732,609.1467;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;12.61198,-167.9955;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-423.9769,-107.4029;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;130;411.2387,75.7908;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;DLNK Shaders/ASE/TopBottomSimple;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.WorldPosInputsNode;131;-1431.287,797.9874;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;132;-1227.287,810.9874;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-1081.287,696.9874;Inherit;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-1242.287,650.9874;Inherit;False;Constant;_Float0;Float 0;26;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;110;-1578.787,965.5519;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;136;-1076.331,844.2047;Inherit;False;Property;_UVWorld;UVWorld;15;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
WireConnection;5;1;136;0
WireConnection;5;5;13;0
WireConnection;35;5;38;0
WireConnection;9;0;1;2
WireConnection;9;1;8;0
WireConnection;4;5;12;0
WireConnection;47;0;5;0
WireConnection;47;1;4;0
WireConnection;108;0;9;0
WireConnection;108;1;117;1
WireConnection;108;2;117;2
WireConnection;39;0;4;0
WireConnection;39;1;35;0
WireConnection;10;0;108;0
WireConnection;46;0;4;0
WireConnection;46;1;39;0
WireConnection;46;2;43;4
WireConnection;120;0;5;0
WireConnection;120;1;47;0
WireConnection;120;2;121;0
WireConnection;11;0;46;0
WireConnection;11;1;120;0
WireConnection;11;2;10;0
WireConnection;15;0;11;0
WireConnection;118;0;15;2
WireConnection;118;2;119;0
WireConnection;17;0;118;0
WireConnection;17;1;8;0
WireConnection;57;0;52;0
WireConnection;105;0;17;0
WireConnection;105;1;117;1
WireConnection;105;2;117;2
WireConnection;16;0;105;0
WireConnection;56;0;48;1
WireConnection;56;1;57;0
WireConnection;3;1;136;0
WireConnection;28;0;30;0
WireConnection;28;1;2;0
WireConnection;37;0;2;0
WireConnection;37;1;34;0
WireConnection;37;2;30;0
WireConnection;51;0;48;1
WireConnection;51;1;56;0
WireConnection;51;2;16;0
WireConnection;7;1;136;0
WireConnection;59;0;53;0
WireConnection;20;0;6;0
WireConnection;20;1;22;0
WireConnection;29;0;31;0
WireConnection;29;1;3;0
WireConnection;44;0;28;0
WireConnection;44;1;37;0
WireConnection;44;2;43;4
WireConnection;26;0;7;0
WireConnection;26;1;25;0
WireConnection;58;0;51;0
WireConnection;58;1;59;0
WireConnection;27;0;7;4
WireConnection;27;1;24;0
WireConnection;21;0;6;4
WireConnection;21;1;23;0
WireConnection;14;0;20;0
WireConnection;14;1;26;0
WireConnection;14;2;16;0
WireConnection;19;0;21;0
WireConnection;19;1;27;0
WireConnection;19;2;16;0
WireConnection;54;0;58;0
WireConnection;18;0;44;0
WireConnection;18;1;29;0
WireConnection;18;2;16;0
WireConnection;98;0;11;0
WireConnection;98;1;8;0
WireConnection;130;0;18;0
WireConnection;130;1;11;0
WireConnection;130;3;14;0
WireConnection;130;4;19;0
WireConnection;130;5;54;0
WireConnection;132;0;131;1
WireConnection;132;1;131;3
WireConnection;133;0;132;0
WireConnection;133;1;111;0
WireConnection;133;2;134;0
WireConnection;110;0;111;0
WireConnection;136;0;110;0
WireConnection;136;1;133;0
ASEEND*/
//CHKSM=FFDD2F766D50167CBB20A677BA3BE3FFC990F0A4