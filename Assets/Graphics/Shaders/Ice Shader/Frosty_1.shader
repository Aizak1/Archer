// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Frosty 1"
{
	Properties
	{
		_BaseAlbedo("BaseAlbedo", 2D) = "white" {}
		_BaseIce("BaseIce", 2D) = "white" {}
		_BaseNormals("BaseNormals", 2D) = "white" {}
		_IceNormals("IceNormals", 2D) = "white" {}
		_IceNoizeFirst("IceNoizeFirst", 2D) = "white" {}
		_IceNoizeSecond("IceNoizeSecond", 2D) = "white" {}
		_FrensnelColor("FrensnelColor", Color) = (1,1,1,1)
		_IceLengthSlider("Ice Length Slider", Range( 0 , 1)) = 0.2878102
		_Smooth("Smooth", Range( 0 , 1)) = 0.557936
		_IceSlider("Ice Slider", Range( 0 , 1)) = 0.3139131
		_IceMaskTile("Ice Mask Tile", Range( 0 , 1)) = 0.2875603
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 15
		_TessMin( "Tess Min Distance", Float ) = 10
		_TessMax( "Tess Max Distance", Float ) = 25
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
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
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform float _IceLengthSlider;
		uniform sampler2D _IceNoizeFirst;
		uniform float _IceMaskTile;
		uniform sampler2D _IceNoizeSecond;
		uniform float _IceSlider;
		uniform sampler2D _BaseNormals;
		uniform float4 _BaseNormals_ST;
		uniform sampler2D _IceNormals;
		uniform float4 _IceNormals_ST;
		uniform sampler2D _BaseAlbedo;
		uniform float4 _BaseAlbedo_ST;
		uniform sampler2D _BaseIce;
		uniform float4 _BaseIce_ST;
		uniform float4 _FrensnelColor;
		uniform float _Smooth;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float IceLenghtSlider7 = _IceLengthSlider;
			float IceMaskTile36 = _IceMaskTile;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float2 appendResult34 = (float2(ase_worldNormal.x , ase_worldNormal.z));
			float2 IceMaskOffset40 = (( IceMaskTile36 * appendResult34 )*1.0 + 0.5);
			float4 IceNoizeMap30 = ( tex2Dlod( _IceNoizeFirst, float4( IceMaskOffset40, 0, 0.0) ) * tex2Dlod( _IceNoizeSecond, float4( IceMaskOffset40, 0, 0.0) ) );
			float IceSlider6 = _IceSlider;
			float YMask19 = saturate( ( IceSlider6 * ( ase_worldNormal.y * -0.3 ) ) );
			float YTopMask63 = saturate( ( ase_worldNormal.y * 3.0 ) );
			float4 Vertexoffset24 = ( float4( ase_vertexNormal , 0.0 ) * ( ( IceLenghtSlider7 * ( IceNoizeMap30 * YMask19 ) ) + ( (0.0 + (IceSlider6 - 0.0) * (0.02 - 0.0) / (1.0 - 0.0)) * YTopMask63 ) ) );
			v.vertex.xyz += Vertexoffset24.rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BaseNormals = i.uv_texcoord * _BaseNormals_ST.xy + _BaseNormals_ST.zw;
			float2 uv_IceNormals = i.uv_texcoord * _IceNormals_ST.xy + _IceNormals_ST.zw;
			float IceSlider6 = _IceSlider;
			float3 lerpResult57 = lerp( UnpackNormal( tex2D( _BaseNormals, uv_BaseNormals ) ) , UnpackNormal( tex2D( _IceNormals, uv_IceNormals ) ) , IceSlider6);
			float3 Normals58 = lerpResult57;
			o.Normal = Normals58;
			float2 uv_BaseAlbedo = i.uv_texcoord * _BaseAlbedo_ST.xy + _BaseAlbedo_ST.zw;
			float2 uv_BaseIce = i.uv_texcoord * _BaseIce_ST.xy + _BaseIce_ST.zw;
			float4 IceTexture70 = tex2D( _BaseIce, uv_BaseIce );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float YMask19 = saturate( ( IceSlider6 * ( ase_worldNormal.y * -0.3 ) ) );
			float4 lerpResult5 = lerp( tex2D( _BaseAlbedo, uv_BaseAlbedo ) , IceTexture70 , saturate( ( YMask19 * 8.0 ) ));
			float4 lerpResult47 = lerp( lerpResult5 , IceTexture70 , IceSlider6);
			float4 Albedo9 = lerpResult47;
			o.Albedo = Albedo9.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV69 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode69 = ( 0.0 + 3.0 * pow( 1.0 - fresnelNdotV69, 2.5 ) );
			float4 Emision79 = ( ( ( IceTexture70 * fresnelNode69 ) * IceSlider6 ) * _FrensnelColor );
			o.Emission = Emision79.rgb;
			float Smooth51 = _Smooth;
			o.Smoothness = Smooth51;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
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
Version=18912
1920;0;1920;1019;1286.402;178.8992;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;35;-3270.82,-1070.498;Inherit;False;Property;_IceMaskTile;Ice Mask Tile;10;0;Create;True;0;0;0;False;0;False;0.2875603;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-2980.105,-1075.51;Inherit;False;IceMaskTile;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;12;-2668.449,512.0961;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;41;-2286.281,62.55694;Inherit;False;905.2451;443.4986;Comment;5;37;34;38;39;40;Ice mask Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-2236.281,263.2211;Inherit;False;36;IceMaskTile;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-2170.614,371.0556;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1981.905,304.5728;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;39;-1845.221,119.4588;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-3277.06,-1260.876;Inherit;False;Property;_IceSlider;Ice Slider;9;0;Create;True;0;0;0;False;0;False;0.3139131;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-1605.035,112.5569;Inherit;False;IceMaskOffset;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;31;-2242.184,-1145.299;Inherit;False;1236.933;496.1772;Comment;5;30;29;28;27;42;Ice Noize Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;20;-2302.067,590.7653;Inherit;False;1079.393;547.3613;Comment;8;19;17;13;16;15;61;62;63;Y mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;6;-2977.529,-1257.325;Inherit;False;IceSlider;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;13;-2253.719,646.6608;Inherit;False;6;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2233.718,744.6609;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-2217.474,-938.5999;Inherit;False;40;IceMaskOffset;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;27;-1822.184,-1083.299;Inherit;True;Property;_IceNoizeFirst;IceNoizeFirst;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-1822.184,-889.7656;Inherit;True;Property;_IceNoizeSecond;IceNoizeSecond;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2057.718,671.6608;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;-1890.718,668.6608;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-2219.872,924.8789;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1365.446,-966.998;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-3273.841,-1173.593;Inherit;False;Property;_IceLengthSlider;Ice Length Slider;7;0;Create;True;0;0;0;False;0;False;0.2878102;0.2878102;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;25;-2303.502,1161.539;Inherit;False;1272.569;785.7444;Vertex offset;12;24;23;33;21;32;22;64;65;66;67;68;83;m;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-1213.638,-970.7926;Inherit;False;IceNoizeMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-3288.726,-855.8868;Inherit;True;Property;_BaseIce;BaseIce;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;-1742.718,661.6608;Inherit;False;YMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;62;-2044.487,927.8265;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;10;-2264.919,-617.9324;Inherit;False;1454.575;638.4948;Comment;10;44;46;8;45;9;5;1;47;48;72;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-2255.251,1418.01;Inherit;False;19;YMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-2279.293,1601.525;Inherit;False;6;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;7;-2985.972,-1173.944;Inherit;False;IceLenghtSlider;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;8;-2204.924,-173.351;Inherit;False;19;YMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;-2951.246,-833.5659;Inherit;False;IceTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;63;-1874.997,924.8792;Inherit;False;YTopMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-2271.857,1241.26;Inherit;False;30;IceNoizeMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;80;-2332.497,2657.16;Inherit;False;1252;460;Comment;8;73;69;76;74;75;78;77;79;Emision;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2204.575,-90.0077;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;69;-2280.352,2844.525;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3;False;3;FLOAT;2.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;73;-2282.497,2707.16;Inherit;False;70;IceTexture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-2230.189,1794.437;Inherit;False;63;YTopMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-2053.936,1198.774;Inherit;False;7;IceLenghtSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2020.575,1348.635;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;68;-2077.895,1588.753;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2015.09,-165.8022;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-2217.227,-335.6056;Inherit;False;70;IceTexture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-1981.496,2750.16;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1856.275,1705.37;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;59;-2305.106,2033.333;Inherit;False;858.4731;545.6066;Comment;5;57;54;55;56;58;Normals Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;76;-2019.496,2912.16;Inherit;False;6;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-1837.169,1335.393;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;45;-1845.605,-159.4859;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-2217.881,-562.9703;Inherit;True;Property;_BaseAlbedo;BaseAlbedo;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-1742.496,2751.16;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-2134.598,2462.939;Inherit;False;6;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-1735.589,-450.8155;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;54;-2253.901,2083.333;Inherit;True;Property;_BaseNormals;BaseNormals;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;21;-1659.55,1210.065;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;55;-2255.106,2273.737;Inherit;True;Property;_IceNormals;IceNormals;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;77;-1801.496,2905.16;Inherit;False;Property;_FrensnelColor;FrensnelColor;6;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;48;-1673.329,-268.1337;Inherit;False;6;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-1793.936,1523.252;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1442.578,1369.455;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-3270.802,-963.3901;Inherit;False;Property;_Smooth;Smooth;8;0;Create;True;0;0;0;False;0;False;0.557936;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;57;-1847.783,2247.225;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-1505.496,2821.16;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;47;-1459.329,-393.1337;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-1162.021,-476.493;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;-1670.633,2244.815;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-1273.499,1370.403;Inherit;False;Vertexoffset;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;-1304.496,2872.16;Inherit;False;Emision;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-2991.968,-963.3901;Inherit;False;Smooth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;-293.1992,81.34491;Inherit;False;79;Emision;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;60;-290.2978,7.363159;Inherit;False;58;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-296.9702,-79.28992;Inherit;False;9;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-249.4974,324.8541;Inherit;False;24;Vertexoffset;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-294.2598,176.9966;Inherit;False;51;Smooth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Frosty 1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;11;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;0;35;0
WireConnection;34;0;12;1
WireConnection;34;1;12;3
WireConnection;37;0;38;0
WireConnection;37;1;34;0
WireConnection;39;0;37;0
WireConnection;40;0;39;0
WireConnection;6;0;3;0
WireConnection;15;0;12;2
WireConnection;27;1;42;0
WireConnection;28;1;42;0
WireConnection;16;0;13;0
WireConnection;16;1;15;0
WireConnection;17;0;16;0
WireConnection;61;0;12;2
WireConnection;29;0;27;0
WireConnection;29;1;28;0
WireConnection;30;0;29;0
WireConnection;19;0;17;0
WireConnection;62;0;61;0
WireConnection;7;0;4;0
WireConnection;70;0;2;0
WireConnection;63;0;62;0
WireConnection;33;0;32;0
WireConnection;33;1;22;0
WireConnection;68;0;66;0
WireConnection;44;0;8;0
WireConnection;44;1;46;0
WireConnection;74;0;73;0
WireConnection;74;1;69;0
WireConnection;67;0;68;0
WireConnection;67;1;64;0
WireConnection;82;0;83;0
WireConnection;82;1;33;0
WireConnection;45;0;44;0
WireConnection;75;0;74;0
WireConnection;75;1;76;0
WireConnection;5;0;1;0
WireConnection;5;1;72;0
WireConnection;5;2;45;0
WireConnection;65;0;82;0
WireConnection;65;1;67;0
WireConnection;23;0;21;0
WireConnection;23;1;65;0
WireConnection;57;0;54;0
WireConnection;57;1;55;0
WireConnection;57;2;56;0
WireConnection;78;0;75;0
WireConnection;78;1;77;0
WireConnection;47;0;5;0
WireConnection;47;1;72;0
WireConnection;47;2;48;0
WireConnection;9;0;47;0
WireConnection;58;0;57;0
WireConnection;24;0;23;0
WireConnection;79;0;78;0
WireConnection;51;0;49;0
WireConnection;0;0;11;0
WireConnection;0;1;60;0
WireConnection;0;2;81;0
WireConnection;0;4;52;0
WireConnection;0;11;26;0
ASEEND*/
//CHKSM=8B6DDCE9BD005C4EDF8AE723589C00F71AF475F7