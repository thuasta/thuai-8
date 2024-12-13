// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DLNK Shaders/ASE/Nature/WaterRefractDepth"
{
	Properties
	{
		_UVScale("UVScale (x) Border (y)", Vector) = (0.02,0.05,0,0)
		_ColorA("Color A", Color) = (0.2971698,0.6247243,1,0)
		_ColorB("Color B", Color) = (0.09838911,0.1034623,0.3113208,0)
		_ColorC("Color C (Refract)", Color) = (0.2971698,0.6247243,1,0)
		_NormalA("Normal A", 2D) = "bump" {}
		_NormalB("Normal B", 2D) = "bump" {}
		_NormalScale("NormalScale", Float) = 1
		[Toggle]_UseBorderNormal("UseBorderNormal", Float) = 1
		_BorderNormalPower("BorderNormalPower", Float) = 1
		_NormalBorder("Normal Border", 2D) = "bump" {}
		_NormalBorderScale("NormalBorderScale", Float) = 1
		_SpecXYSnsZW("Metal (xy) Gloss (zw)", Vector) = (0.1,0,0.5,0.2)
		_Depth("Depth", Float) = 0.9
		_Falloff("Falloff", Float) = -3
		_Distorsion("Distorsion", Float) = 0.1
		_DepthOpacity("DepthOpacity (xy)", Vector) = (0,1,0,0)
		__VelocityXYFoamZ("Waves Speed (xy) Border (z)", Vector) = (0.01,-0.02,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform float _UseBorderNormal;
		uniform sampler2D _NormalA;
		uniform float3 __VelocityXYFoamZ;
		uniform float2 _UVScale;
		uniform float _NormalScale;
		uniform sampler2D _NormalB;
		uniform sampler2D _NormalBorder;
		uniform float _NormalBorderScale;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth;
		uniform float _Falloff;
		uniform float _BorderNormalPower;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float4 _ColorC;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Distorsion;
		uniform float2 _DepthOpacity;
		uniform float4 _SpecXYSnsZW;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (__VelocityXYFoamZ.x).xx;
			float3 ase_worldPos = i.worldPos;
			float2 appendResult6 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_9_0 = ( appendResult6 * _UVScale.x );
			float2 panner11 = ( 1.0 * _Time.y * temp_cast_0 + temp_output_9_0);
			float2 temp_cast_1 = (__VelocityXYFoamZ.y).xx;
			float2 panner12 = ( 1.0 * _Time.y * temp_cast_1 + temp_output_9_0);
			float3 temp_output_28_0 = BlendNormals( UnpackScaleNormal( tex2D( _NormalA, panner11 ), _NormalScale ) , UnpackScaleNormal( tex2D( _NormalB, panner12 ), _NormalScale ) );
			float2 temp_cast_2 = (__VelocityXYFoamZ.z).xx;
			float2 panner59 = ( 1.0 * _Time.y * temp_cast_2 + ( appendResult6 * _UVScale.y ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth4 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float temp_output_36_0 = saturate( pow( ( abs( ( eyeDepth4 - ase_screenPos.w ) ) + _Depth ) , _Falloff ) );
			float temp_output_47_0 = ( 1.0 - temp_output_36_0 );
			float3 lerpResult56 = lerp( UnpackScaleNormal( tex2D( _NormalBorder, panner59 ), _NormalBorderScale ) , temp_output_28_0 , abs( pow( temp_output_47_0 , _BorderNormalPower ) ));
			o.Normal = (( _UseBorderNormal )?( lerpResult56 ):( temp_output_28_0 ));
			float4 lerpResult39 = lerp( _ColorA , _ColorB , temp_output_36_0);
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( float3( (ase_grabScreenPosNorm).xy ,  0.0 ) + ( temp_output_28_0 * _Distorsion ) ).xy);
			float4 lerpResult45 = lerp( lerpResult39 , ( _ColorC * screenColor41 ) , ( temp_output_47_0 * saturate( (_DepthOpacity.x + (temp_output_36_0 - 0.0) * (_DepthOpacity.y - _DepthOpacity.x) / (1.0 - 0.0)) ) ));
			o.Albedo = lerpResult45.rgb;
			float lerpResult44 = lerp( _SpecXYSnsZW.x , _SpecXYSnsZW.y , temp_output_36_0);
			o.Metallic = lerpResult44;
			float lerpResult46 = lerp( _SpecXYSnsZW.z , _SpecXYSnsZW.w , temp_output_36_0);
			o.Smoothness = saturate( lerpResult46 );
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1567.846,551.9696;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;11;-863.8392,280.9692;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;23;-694.5421,222.9906;Inherit;True;Property;_NormalA;Normal A;4;0;Create;True;0;0;0;False;0;False;-1;None;56c1ddb3fe77bb94c8009ccb7909c80f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;24;-830.8378,-678.6123;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-725.9032,-500.3498;Inherit;False;Property;_Distorsion;Distorsion;14;0;Create;True;0;0;0;False;0;False;0.1;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-537.5268,-574.942;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;33;-614.6476,-658.384;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;35;-877.5121,-222.8869;Inherit;False;Property;_ColorB;Color B;2;0;Create;True;0;0;0;False;0;False;0.09838911,0.1034623,0.3113208,0;0.6100035,0.7282173,0.8679245,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;39;-579.0333,-348.1736;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-369.3787,-638.1554;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;1;-1573.461,-56.59119;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;4;-1370.461,-33.59121;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-1133.461,10.4088;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;3;-1355.461,58.40874;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;41;-235.4858,-579.556;Inherit;False;Global;_GrabScreen1;Grab Screen 1;9;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;25;-810.0329,-0.3376091;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;15;-1113.917,139.8079;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;45;-16.84483,-377.6606;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-944.4442,9.979769;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;36;-659.7034,-52.84229;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-789.2885,112.1452;Inherit;False;Property;_Falloff;Falloff;13;0;Create;True;0;0;0;False;0;False;-3;-1.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-959.9817,138.4991;Inherit;False;Property;_Depth;Depth;12;0;Create;True;0;0;0;False;0;False;0.9;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-1387.261,572.8128;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector3Node;57;-1461.964,329.5376;Inherit;False;Property;__VelocityXYFoamZ;Waves Speed (xy) Border (z);16;0;Create;False;0;0;0;False;0;False;0.01,-0.02,0;0.01,-0.02,0.005;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;47;-515.1294,-187.1206;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;49;-421.0437,-107.3438;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;50;-641.7381,33.84782;Inherit;False;Property;_DepthOpacity;DepthOpacity (xy);15;0;Create;False;0;0;0;False;0;False;0,1;0.14,5.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;46;318.4671,554.2876;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;12;-869.142,434.6906;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;59;-902.1704,653.2926;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1219.217,571.7388;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1115.479,672.4099;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;60;-1433.428,710.6443;Inherit;False;Property;_UVScale;UVScale (x) Border (y);0;0;Create;False;0;0;0;False;0;False;0.02,0.05;0.02,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.BlendNormalsNode;28;-403.9405,352.918;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;376.1718,-291.4005;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;DLNK Shaders/ASE/Nature/WaterRefractDepth;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.LerpOp;56;108.9932,98.25719;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;64;-15.31204,181.5107;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;63;-46.32092,82.53845;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;51;-225.5141,-147.8567;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-890.6477,565.1962;Inherit;False;Property;_NormalScale;NormalScale;6;0;Create;True;0;0;0;False;0;False;1;-0.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-692.1421,412.6906;Inherit;True;Property;_NormalB;Normal B;5;0;Create;True;0;0;0;False;0;False;-1;None;e2f18ab7277462641b30501d47d3d715;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;65;-265.4193,88.22593;Inherit;False;Property;_BorderNormalPower;BorderNormalPower;8;0;Create;True;0;0;0;False;0;False;1;2.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;62;113.7144,-50.90194;Inherit;False;Property;_UseBorderNormal;UseBorderNormal;7;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;43;0.8739123,421.8105;Inherit;False;Property;_SpecXYSnsZW;Metal (xy) Gloss (zw);11;0;Create;False;0;0;0;False;0;False;0.1,0,0.5,0.2;0,0,1.27,-0.74;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;44;243.3828,390.4159;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;436.479,399.2836;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-917.4,784.8253;Inherit;False;Property;_NormalBorderScale;NormalBorderScale;10;0;Create;True;0;0;0;False;0;False;1;-0.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;-692.335,604.8432;Inherit;True;Property;_NormalBorder;Normal Border;9;0;Create;True;0;0;0;False;0;False;-1;None;8f925442ab1daa748808b04b489fe0e7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-880.2606,-400.1871;Inherit;False;Property;_ColorA;Color A;1;0;Create;True;0;0;0;False;0;False;0.2971698,0.6247243,1,0;0.232289,0.2966892,0.3396226,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-206.1223,-258.8704;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-53.7828,-558.1483;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;68;-259.0874,-780.3448;Inherit;False;Property;_ColorC;Color C (Refract);3;0;Create;False;0;0;0;False;0;False;0.2971698,0.6247243,1,0;0.4016999,0.4939475,0.5566037,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;11;0;9;0
WireConnection;11;2;57;1
WireConnection;23;1;11;0
WireConnection;23;5;13;0
WireConnection;32;0;28;0
WireConnection;32;1;27;0
WireConnection;33;0;24;0
WireConnection;39;0;30;0
WireConnection;39;1;35;0
WireConnection;39;2;36;0
WireConnection;40;0;33;0
WireConnection;40;1;32;0
WireConnection;4;0;1;0
WireConnection;7;0;4;0
WireConnection;7;1;3;4
WireConnection;41;0;40;0
WireConnection;25;0;19;0
WireConnection;25;1;20;0
WireConnection;15;0;7;0
WireConnection;45;0;39;0
WireConnection;45;1;69;0
WireConnection;45;2;53;0
WireConnection;19;0;15;0
WireConnection;19;1;14;0
WireConnection;36;0;25;0
WireConnection;6;0;2;1
WireConnection;6;1;2;3
WireConnection;47;0;36;0
WireConnection;49;0;36;0
WireConnection;49;3;50;1
WireConnection;49;4;50;2
WireConnection;46;0;43;3
WireConnection;46;1;43;4
WireConnection;46;2;36;0
WireConnection;12;0;9;0
WireConnection;12;2;57;2
WireConnection;59;0;61;0
WireConnection;59;2;57;3
WireConnection;9;0;6;0
WireConnection;9;1;60;1
WireConnection;61;0;6;0
WireConnection;61;1;60;2
WireConnection;28;0;23;0
WireConnection;28;1;22;0
WireConnection;0;0;45;0
WireConnection;0;1;62;0
WireConnection;0;3;44;0
WireConnection;0;4;67;0
WireConnection;56;0;55;0
WireConnection;56;1;28;0
WireConnection;56;2;64;0
WireConnection;64;0;63;0
WireConnection;63;0;47;0
WireConnection;63;1;65;0
WireConnection;51;0;49;0
WireConnection;22;1;12;0
WireConnection;22;5;13;0
WireConnection;62;0;28;0
WireConnection;62;1;56;0
WireConnection;44;0;43;1
WireConnection;44;1;43;2
WireConnection;44;2;36;0
WireConnection;67;0;46;0
WireConnection;55;1;59;0
WireConnection;55;5;66;0
WireConnection;53;0;47;0
WireConnection;53;1;51;0
WireConnection;69;0;68;0
WireConnection;69;1;41;0
ASEEND*/
//CHKSM=E5F590C368F3FD50B4536854888321331E25E384