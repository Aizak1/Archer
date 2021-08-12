// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BurnFinal"
{
	Properties
	{
		_Smothness("Smothness", Range( 0 , 1)) = 0
		_NoizeTreshhold("NoizeTreshhold", Range( 0 , 1)) = 1
		_DisolbeGap("DisolbeGap", Range( 0 , 1)) = 0.5
		_BaseAlbedo("BaseAlbedo", 2D) = "white" {}
		[HDR]_DesolveColor("DesolveColor", Color) = (0.359336,0.8018868,0.7973659,1)
		_LowestYPos("LowestYPos", Float) = 0
		_HiestYposition("HiestYposition", Float) = 0
		_FireRelativeheight("FireRelativeheight", Range( 0 , 1)) = 0
		_FireGap("FireGap", Range( 0 , 1)) = 0
		_fireDestorsionByheight("fireDestorsionByheight", Range( 0 , 10)) = 0
		[Toggle(_BACKFASECOLOR_ON)] _BackFaseColor("BackFaseColor", Float) = 1
		[HDR]_BackFaceColor("BackFaceColor", Color) = (1,0,0,0)
		[Toggle(_ISLOCAL_ON)] _IsLocal("IsLocal", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#pragma shader_feature_local _ISLOCAL_ON
		#pragma shader_feature_local _BACKFASECOLOR_ON
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			half ASEVFace : VFACE;
		};

		uniform half _LowestYPos;
		uniform half _HiestYposition;
		uniform half _FireRelativeheight;
		uniform half _FireGap;
		uniform half _fireDestorsionByheight;
		uniform half _NoizeTreshhold;
		uniform half _DisolbeGap;
		uniform sampler2D _BaseAlbedo;
		uniform half4 _BaseAlbedo_ST;
		uniform half4 _BackFaceColor;
		uniform half4 _DesolveColor;
		uniform half _Smothness;


		//https://www.shadertoy.com/view/XdXGW8
		float2 GradientNoiseDir( float2 x )
		{
			const float2 k = float2( 0.3183099, 0.3678794 );
			x = x * k + k.yx;
			return -1.0 + 2.0 * frac( 16.0 * k * frac( x.x * x.y * ( x.x + x.y ) ) );
		}
		
		float GradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 i = floor( p );
			float2 f = frac( p );
			float2 u = f * f * ( 3.0 - 2.0 * f );
			return lerp( lerp( dot( GradientNoiseDir( i + float2( 0.0, 0.0 ) ), f - float2( 0.0, 0.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 0.0 ) ), f - float2( 1.0, 0.0 ) ), u.x ),
					lerp( dot( GradientNoiseDir( i + float2( 0.0, 1.0 ) ), f - float2( 0.0, 1.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 1.0 ) ), f - float2( 1.0, 1.0 ) ), u.x ), u.y );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			#ifdef _ISLOCAL_ON
				half staticSwitch269 = ase_vertex3Pos.y;
			#else
				half staticSwitch269 = ase_worldPos.y;
			#endif
			half YCoordValue272 = staticSwitch269;
			half Height147 = _FireRelativeheight;
			half lerpResult155 = lerp( _LowestYPos , _HiestYposition , Height147);
			half YHeightCoord156 = lerpResult155;
			half Gap148 = _FireGap;
			half temp_output_160_0 = ( Gap148 / 2.0 );
			half4 color277 = IsGammaSpace() ? half4(1,1,1,1) : half4(1,1,1,1);
			half4 White141 = color277;
			half4 color276 = IsGammaSpace() ? half4(0,0,0,1) : half4(0,0,0,1);
			half4 Black140 = color276;
			half4 FireMask162 = (( YCoordValue272 >= ( YHeightCoord156 - temp_output_160_0 ) && YCoordValue272 <= ( YHeightCoord156 + temp_output_160_0 ) ) ? White141 :  Black140 );
			half mulTime238 = _Time.y * 0.2;
			float2 uv_TexCoord189 = i.uv_texcoord + ( ( half2( 1,0 ) * mulTime238 ) + ( _SinTime.x * half2( 0,1 ) ) );
			half gradientNoise190 = GradientNoise(uv_TexCoord189,_fireDestorsionByheight);
			gradientNoise190 = gradientNoise190*0.5 + 0.5;
			half temp_output_203_0 = ( Gap148 / 2.0 );
			half temp_output_206_0 = ( YHeightCoord156 - temp_output_203_0 );
			half FireHeightGap246 = ( abs( ( ( YHeightCoord156 + temp_output_203_0 ) - temp_output_206_0 ) ) / abs( ( temp_output_206_0 - YCoordValue272 ) ) );
			half temp_output_248_0 = ( FireHeightGap246 * _NoizeTreshhold );
			half DesoleBorderMask196 = step( gradientNoise190 , ( temp_output_248_0 - _DisolbeGap ) );
			half4 temp_cast_0 = (DesoleBorderMask196).xxxx;
			half4 HeightMask175 = ( YCoordValue272 > YHeightCoord156 ? Black140 : White141 );
			half4 DesoleCompleatMask265 = ( FireMask162 == White141 ? temp_cast_0 : HeightMask175 );
			float2 uv_BaseAlbedo = i.uv_texcoord * _BaseAlbedo_ST.xy + _BaseAlbedo_ST.zw;
			half4 tex2DNode42 = tex2D( _BaseAlbedo, uv_BaseAlbedo );
			#ifdef _BACKFASECOLOR_ON
				half4 staticSwitch267 = _BackFaceColor;
			#else
				half4 staticSwitch267 = tex2DNode42;
			#endif
			half4 switchResult266 = (((i.ASEVFace>0)?(tex2DNode42):(staticSwitch267)));
			half4 Albedo51 = ( DesoleCompleatMask265 == White141 ? switchResult266 : _DesolveColor );
			o.Albedo = Albedo51.rgb;
			o.Smoothness = _Smothness;
			half OpacityMaskv2193 = step( gradientNoise190 , temp_output_248_0 );
			half4 temp_cast_2 = (OpacityMaskv2193).xxxx;
			half4 FireHeightOpacityMask187 = ( FireMask162 == White141 ? temp_cast_2 : HeightMask175 );
			o.Alpha = FireHeightOpacityMask187.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
1920;1;1920;1018;7110.506;2145.126;6.792964;True;True
Node;AmplifyShaderEditor.CommentaryNode;281;-3206.849,1263.584;Inherit;False;710.761;1345.171;Comment;19;136;147;151;150;149;137;271;270;155;148;156;269;272;276;277;140;141;153;154;Props;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-3107.486,2065.53;Inherit;False;Property;_FireRelativeheight;FireRelativeheight;10;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;147;-2819.208,2066.29;Inherit;False;Height;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;151;-3141.028,2492.755;Inherit;False;147;Height;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-3156.849,2276.063;Inherit;False;Property;_LowestYPos;LowestYPos;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;137;-3106.521,2158.998;Inherit;False;Property;_FireGap;FireGap;11;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-3152.849,2360.063;Inherit;False;Property;_HiestYposition;HiestYposition;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;271;-3114.158,1313.584;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;155;-2943.263,2448.116;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;148;-2820.018,2156.877;Inherit;False;Gap;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;270;-3109.886,1479.961;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;282;-2197.128,4024.294;Inherit;False;1241.854;425.3369;Comment;12;202;204;203;275;206;205;208;210;211;209;212;246;Y Miday relation poin;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;269;-2918.179,1401.727;Inherit;False;Property;_IsLocal;IsLocal;15;0;Create;True;0;0;0;False;0;False;0;1;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;156;-2758.662,2455.917;Inherit;False;YHeightCoord;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;202;-2147.128,4323.849;Inherit;False;148;Gap;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;272;-2720.088,1400.09;Inherit;False;YCoordValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;203;-1938.691,4314.631;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;204;-1984.527,4240.949;Inherit;False;156;YHeightCoord;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;206;-1719.69,4199.631;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;205;-1710.727,4309.249;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;275;-1988.388,4094.363;Inherit;False;272;YCoordValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;210;-1598.704,4074.294;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;208;-1554.704,4242.294;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;211;-1465.704,4076.294;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;209;-1426.704,4237.294;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;283;-2173.73,2995.973;Inherit;False;1503.322;891.9609;Comment;20;252;236;235;238;240;237;241;191;247;248;197;213;189;190;195;194;196;192;193;251;Masks mexed dizolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinTimeNode;252;-2123.73,3500.93;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;236;-2108.171,3723.934;Inherit;False;Constant;_Vector6;Vector 6;20;0;Create;True;0;0;0;False;0;False;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;238;-2116.972,3646.634;Inherit;False;1;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;212;-1316.704,4126.294;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;235;-2122.27,3369.433;Inherit;False;Constant;_Vector5;Vector 5;20;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;246;-1179.273,4129.872;Inherit;False;FireHeightGap;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;237;-1899.172,3483.934;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;-1879.172,3683.934;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;163;-2143.686,474.1425;Inherit;False;1200.457;673.5883;white - fire;10;159;157;160;158;161;144;143;145;162;273;FireMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;241;-1754.172,3571.934;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;276;-3150.152,1657.097;Inherit;False;Constant;_Black1;Black;3;0;Create;True;0;0;0;False;0;False;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;159;-2093.686,828.3658;Inherit;False;148;Gap;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;247;-1884.397,3045.973;Inherit;False;246;FireHeightGap;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-1939.615,3136.733;Inherit;False;Property;_NoizeTreshhold;NoizeTreshhold;2;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;277;-3144.826,1862.053;Inherit;False;Constant;_white1;white;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;213;-2088.318,3260.282;Inherit;False;Property;_fireDestorsionByheight;fireDestorsionByheight;12;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-1478.234,3545.982;Inherit;False;Property;_DisolbeGap;DisolbeGap;4;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;-1439.395,3135.973;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;160;-1885.249,819.1477;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;140;-2870.455,1703.381;Inherit;False;Black;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-1931.086,745.4664;Inherit;False;156;YHeightCoord;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;141;-2891.455,1873.382;Inherit;False;White;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;189;-1742.377,3207.267;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;176;-2144.698,1219.349;Inherit;False;989.8214;578.5881;Black Transperant;5;170;169;165;174;175;Hight mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;273;-1914.491,550.6452;Inherit;False;272;YCoordValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;165;-2079.545,1461.672;Inherit;False;156;YHeightCoord;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;190;-1508.076,3305.467;Inherit;True;Gradient;True;False;2;0;FLOAT2;0.5,0.5;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;144;-1909.238,1031.731;Inherit;False;140;Black;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;274;-1988.185,1316.256;Inherit;False;272;YCoordValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;-1657.286,813.7656;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-1910.238,951.7308;Inherit;False;141;White;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;-2094.698,1601.937;Inherit;False;140;Black;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-2084.698,1681.937;Inherit;False;141;White;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;161;-1666.249,704.1477;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;195;-1200.36,3516.969;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;174;-1604.691,1437.375;Inherit;False;2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;194;-1044.985,3413.212;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;145;-1402.934,725.6868;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;175;-1378.874,1443.708;Inherit;False;HeightMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;162;-1167.227,717.301;Inherit;False;FireMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;257;-2189.553,2431.138;Inherit;False;1026.831;435.6214;Comment;7;264;263;261;260;259;258;265;fireHeightOpasityMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;196;-916.4086,3434.273;Inherit;False;DesoleBorderMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;258;-2133.064,2573.22;Inherit;False;141;White;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;259;-2134.02,2481.138;Inherit;False;162;FireMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;260;-2149.892,2783.516;Inherit;False;196;DesoleBorderMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;264;-1803.155,2775.335;Inherit;False;175;HeightMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2133.421,-1145.752;Inherit;False;1126.927;867.4808;Albedo;9;49;47;42;43;51;266;267;268;280;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;42;-2103.232,-831.7797;Inherit;True;Property;_BaseAlbedo;BaseAlbedo;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;192;-1166.892,3312.133;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;261;-1628.346,2486.599;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;268;-2108.721,-514.5606;Inherit;False;Property;_BackFaceColor;BackFaceColor;14;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;193;-918.4496,3235.568;Inherit;False;OpacityMaskv2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;265;-1402.271,2490.057;Inherit;False;DesoleCompleatMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;267;-2108.721,-634.5607;Inherit;False;Property;_BackFaseColor;BackFaseColor;13;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;186;-2148.159,1873.419;Inherit;False;1026.831;435.6214;Comment;7;181;187;180;179;185;178;177;fireHeightOpasityMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;179;-2091.67,2015.501;Inherit;False;141;White;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;177;-1761.761,2217.616;Inherit;False;175;HeightMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;185;-2108.498,2225.797;Inherit;False;193;OpacityMaskv2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;178;-2092.626,1923.419;Inherit;False;162;FireMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SwitchByFaceNode;266;-1719.542,-824.7647;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;280;-2031.995,-982.3715;Inherit;False;141;White;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-2051.491,-1091.471;Inherit;False;265;DesoleCompleatMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;47;-1879.709,-527.2537;Inherit;False;Property;_DesolveColor;DesolveColor;6;1;[HDR];Create;True;0;0;0;False;0;False;0.359336,0.8018868,0.7973659,1;0.359336,0.8018868,0.7973659,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;49;-1437.38,-899.8187;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Compare;181;-1586.952,1928.88;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;187;-1397.474,1953.199;Inherit;False;FireHeightOpacityMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-1230.495,-893.2946;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;15;-2145.208,-81.23833;Inherit;False;1243.491;463.1858;Comment;10;22;32;31;29;16;21;1;38;11;12;Opacity Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;-1481.19,241.1979;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;21;-1447.722,36.36205;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-424.5563,2218.728;Inherit;False;187;FireHeightOpacityMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-1745.905,34.69614;Inherit;True;Gradient;True;False;2;0;FLOAT2;0.5,0.5;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1763.442,-47.03811;Inherit;False;Property;_MaskStep;MaskStep;1;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-1197.238,158.5021;Inherit;False;DesoleMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1960.205,40.49633;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-1199.279,-40.20275;Inherit;False;OpacityMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-438.2572,1962.99;Inherit;False;51;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1759.064,270.2107;Inherit;False;Property;_DesoleGap;DesoleGap;3;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-2918.889,2344.965;Inherit;False;HierYcoord;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;11;-2108.102,40.69613;Inherit;False;Property;_Vector0;Vector 0;7;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;180;-2098.159,2100.774;Inherit;False;140;Black;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;263;-2139.553,2658.493;Inherit;False;140;Black;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-447.9572,2088.233;Inherit;False;Property;_Smothness;Smothness;0;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Exp2OpNode;251;-1607.589,3052.488;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;153;-2923.889,2255.965;Inherit;False;LowerYCoord;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;31;-1325.815,137.4408;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-89.48416,2023.508;Half;False;True;-1;3;ASEMaterialInspector;0;0;Standard;BurnFinal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Overlay;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;True;0;0;False;-1;0;0;True;6;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;147;0;136;0
WireConnection;155;0;149;0
WireConnection;155;1;150;0
WireConnection;155;2;151;0
WireConnection;148;0;137;0
WireConnection;269;1;271;2
WireConnection;269;0;270;2
WireConnection;156;0;155;0
WireConnection;272;0;269;0
WireConnection;203;0;202;0
WireConnection;206;0;204;0
WireConnection;206;1;203;0
WireConnection;205;0;204;0
WireConnection;205;1;203;0
WireConnection;210;0;206;0
WireConnection;210;1;275;0
WireConnection;208;0;205;0
WireConnection;208;1;206;0
WireConnection;211;0;210;0
WireConnection;209;0;208;0
WireConnection;212;0;209;0
WireConnection;212;1;211;0
WireConnection;246;0;212;0
WireConnection;237;0;235;0
WireConnection;237;1;238;0
WireConnection;240;0;252;1
WireConnection;240;1;236;0
WireConnection;241;0;237;0
WireConnection;241;1;240;0
WireConnection;248;0;247;0
WireConnection;248;1;191;0
WireConnection;160;0;159;0
WireConnection;140;0;276;0
WireConnection;141;0;277;0
WireConnection;189;1;241;0
WireConnection;190;0;189;0
WireConnection;190;1;213;0
WireConnection;158;0;157;0
WireConnection;158;1;160;0
WireConnection;161;0;157;0
WireConnection;161;1;160;0
WireConnection;195;0;248;0
WireConnection;195;1;197;0
WireConnection;174;0;274;0
WireConnection;174;1;165;0
WireConnection;174;2;169;0
WireConnection;174;3;170;0
WireConnection;194;0;190;0
WireConnection;194;1;195;0
WireConnection;145;0;273;0
WireConnection;145;1;161;0
WireConnection;145;2;158;0
WireConnection;145;3;143;0
WireConnection;145;4;144;0
WireConnection;175;0;174;0
WireConnection;162;0;145;0
WireConnection;196;0;194;0
WireConnection;192;0;190;0
WireConnection;192;1;248;0
WireConnection;261;0;259;0
WireConnection;261;1;258;0
WireConnection;261;2;260;0
WireConnection;261;3;264;0
WireConnection;193;0;192;0
WireConnection;265;0;261;0
WireConnection;267;1;42;0
WireConnection;267;0;268;0
WireConnection;266;0;42;0
WireConnection;266;1;267;0
WireConnection;49;0;43;0
WireConnection;49;1;280;0
WireConnection;49;2;266;0
WireConnection;49;3;47;0
WireConnection;181;0;178;0
WireConnection;181;1;179;0
WireConnection;181;2;185;0
WireConnection;181;3;177;0
WireConnection;187;0;181;0
WireConnection;51;0;49;0
WireConnection;38;0;22;0
WireConnection;38;1;29;0
WireConnection;21;0;1;0
WireConnection;21;1;22;0
WireConnection;1;0;12;0
WireConnection;32;0;31;0
WireConnection;12;0;11;0
WireConnection;16;0;21;0
WireConnection;154;0;150;0
WireConnection;251;0;247;0
WireConnection;153;0;149;0
WireConnection;31;0;1;0
WireConnection;31;1;38;0
WireConnection;0;0;54;0
WireConnection;0;4;6;0
WireConnection;0;9;17;0
ASEEND*/
//CHKSM=9BE9EA3DF99F1291EAF656CFF9060A6B5FE6667C