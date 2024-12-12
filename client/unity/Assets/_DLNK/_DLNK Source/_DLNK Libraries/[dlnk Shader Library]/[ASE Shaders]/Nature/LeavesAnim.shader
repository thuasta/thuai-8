// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DLNK Shaders/ASE/Nature/LeavesAnim"
{
	Properties
	{
		[HDR]_ColorA("Color A", Color) = (0.5943396,0.5943396,0.5943396,0)
		[HDR]_ColorB("Color B", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_Smoothness("Smoothness", Float) = 0
		_BumpMap("Normal", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_NormalScale("NormalScale", Float) = 0
		_Noisespeed("Noise speed", Float) = 1
		_NoiseScale("Noise Scale", Float) = 1
		_LeavesGradientxy("Leaves Gradient (xy)", Vector) = (0,1,0,0)
		_AOPower("AO Power", Float) = 0.5
		_MotionValue("MotionValue (xy)", Vector) = (0.1,0.1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TreeTransparentCutout"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#define ASE_USING_SAMPLING_MACROS 1
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred dithercrossfade vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _Noisespeed;
		uniform float _NoiseScale;
		uniform float2 _MotionValue;
		uniform float2 _LeavesGradientxy;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_BumpMap);
		uniform float4 _BumpMap_ST;
		SamplerState sampler_BumpMap;
		uniform float _NormalScale;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MainTex);
		uniform float4 _MainTex_ST;
		SamplerState sampler_MainTex;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float _Smoothness;
		uniform float _AOPower;
		uniform float _Cutoff = 0.5;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = (_Noisespeed).xx;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 panner56 = ( 1.0 * _Time.y * temp_cast_0 + ase_worldPos.xy);
			float simplePerlin3D57 = snoise( float3( panner56 ,  0.0 )*_NoiseScale );
			simplePerlin3D57 = simplePerlin3D57*0.5 + 0.5;
			float2 temp_cast_3 = (_Noisespeed).xx;
			float2 panner93 = ( 6.6 * _Time.y * temp_cast_3 + ase_worldPos.xy);
			float simplePerlin3D88 = snoise( float3( panner93 ,  0.0 )*( 1.0 - _NoiseScale ) );
			simplePerlin3D88 = simplePerlin3D88*0.5 + 0.5;
			float blendOpSrc97 = simplePerlin3D57;
			float blendOpDest97 = simplePerlin3D88;
			float temp_output_97_0 = ( saturate( (( blendOpDest97 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest97 ) * ( 1.0 - blendOpSrc97 ) ) : ( 2.0 * blendOpDest97 * blendOpSrc97 ) ) ));
			float lerpResult83 = lerp( _MotionValue.x , -_MotionValue.x , _CosTime.z);
			float lerpResult85 = lerp( _MotionValue.y , -_MotionValue.y , _CosTime.y);
			float4 appendResult64 = (float4(( temp_output_97_0 * lerpResult83 ) , 0.0 , ( temp_output_97_0 * lerpResult85 ) , 0.0));
			float temp_output_99_0 = saturate( (_LeavesGradientxy.x + (v.texcoord.xy.y - 0.0) * (_LeavesGradientxy.y - _LeavesGradientxy.x) / (1.0 - 0.0)) );
			float4 lerpResult92 = lerp( float4( 0,0,0,0 ) , appendResult64 , temp_output_99_0);
			v.vertex.xyz += lerpResult92.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			o.Normal = UnpackScaleNormal( SAMPLE_TEXTURE2D( _BumpMap, sampler_BumpMap, uv_BumpMap ), _NormalScale );
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode7 = SAMPLE_TEXTURE2D( _MainTex, sampler_MainTex, uv_MainTex );
			float temp_output_99_0 = saturate( (_LeavesGradientxy.x + (i.uv_texcoord.y - 0.0) * (_LeavesGradientxy.y - _LeavesGradientxy.x) / (1.0 - 0.0)) );
			float4 lerpResult49 = lerp( _ColorA , _ColorB , temp_output_99_0);
			o.Albedo = ( tex2DNode7 * lerpResult49 ).rgb;
			o.Smoothness = _Smoothness;
			o.Occlusion = (( 1.0 - _AOPower ) + ((lerpResult49).a - 0.0) * (1.0 - ( 1.0 - _AOPower )) / (1.0 - 0.0));
			o.Alpha = 1;
			clip( tex2DNode7.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "DLNKShaders/Nature/LeavesAnim"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.RangedFloatNode;55;-106.7086,118.1799;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-486,49.5;Inherit;True;Property;_BumpMap;Normal;4;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;49;-118.9743,-461.2481;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-688.0908,131.8801;Inherit;False;Property;_NormalScale;NormalScale;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-252.9211,298.8245;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;-117.9211,316.8246;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-249.9211,396.8245;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-669.2058,-282.7589;Inherit;True;Property;_MainTex;Albedo;2;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;68;-1004.772,-115.7249;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;74;114.9911,-484.1711;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;78;158.5382,-367.3395;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;73;-1007.23,17.29645;Inherit;False;Property;_LeavesGradientxy;Leaves Gradient (xy);9;0;Create;True;0;0;0;False;0;False;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;59;-854.0193,757.1432;Inherit;False;Property;_Noisespeed;Noise speed;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;61;-862.8398,607.3307;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;83;-612.9706,243.171;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;85;-612.9706,387.171;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;86;-988.9707,427.1711;Inherit;False;Property;_MotionValue;MotionValue (xy);11;0;Create;False;0;0;0;False;0;False;0.1,0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CosTime;87;-1012.971,243.171;Inherit;True;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;84;-756.9707,388.1711;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;82;-754.9707,263.1711;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-345.1744,-602.0415;Inherit;False;Property;_ColorA;Color A;0;1;[HDR];Create;True;0;0;0;False;0;False;0.5943396,0.5943396,0.5943396,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;51;-335.1071,-424.8551;Inherit;False;Property;_ColorB;Color B;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;141.9001,-270.1754;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;79;348.3381,-446.7395;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;77.02814,-559.2891;Inherit;False;Property;_AOPower;AO Power;10;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;417.2,-170.1;Float;False;True;-1;2;DLNKShaders/Nature/LeavesAnim;0;0;Standard;DLNK Shaders/ASE/Nature/LeavesAnim;False;False;False;False;False;False;False;False;False;False;False;False;True;False;True;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;TreeTransparentCutout;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;5;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;57;-472.3275,573.4816;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;92;139.0153,134.4308;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;88;-456.1055,700.9401;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;56;-675.9786,636.9161;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;93;-652.0948,775.3301;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;6.6;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-892.5904,863.028;Inherit;False;Property;_NoiseScale;Noise Scale;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;94;-627.7749,932.6938;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;97;-240.0881,586.4937;Inherit;False;Overlay;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;98;-727.9154,-67.28079;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;99;-298.7416,-77.29484;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
WireConnection;11;5;16;0
WireConnection;49;0;50;0
WireConnection;49;1;51;0
WireConnection;49;2;99;0
WireConnection;63;0;97;0
WireConnection;63;1;83;0
WireConnection;64;0;63;0
WireConnection;64;2;65;0
WireConnection;65;0;97;0
WireConnection;65;1;85;0
WireConnection;74;0;49;0
WireConnection;78;0;76;0
WireConnection;83;0;86;1
WireConnection;83;1;82;0
WireConnection;83;2;87;3
WireConnection;85;0;86;2
WireConnection;85;1;84;0
WireConnection;85;2;87;2
WireConnection;84;0;86;2
WireConnection;82;0;86;1
WireConnection;48;0;7;0
WireConnection;48;1;49;0
WireConnection;79;0;74;0
WireConnection;79;3;78;0
WireConnection;0;0;48;0
WireConnection;0;1;11;0
WireConnection;0;4;55;0
WireConnection;0;5;79;0
WireConnection;0;10;7;4
WireConnection;0;11;92;0
WireConnection;57;0;56;0
WireConnection;57;1;60;0
WireConnection;92;1;64;0
WireConnection;92;2;99;0
WireConnection;88;0;93;0
WireConnection;88;1;94;0
WireConnection;56;0;61;0
WireConnection;56;2;59;0
WireConnection;93;0;61;0
WireConnection;93;2;59;0
WireConnection;94;0;60;0
WireConnection;97;0;57;0
WireConnection;97;1;88;0
WireConnection;98;0;68;2
WireConnection;98;3;73;1
WireConnection;98;4;73;2
WireConnection;99;0;98;0
ASEEND*/
//CHKSM=DAF2898B45754C18E4B0816CEFAA1981182FD43E