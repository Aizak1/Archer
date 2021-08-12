// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Disolve"
{
	Properties
	{
		_Smothness("Smothness", Range( 0 , 1)) = 0
		_MaskStep("MaskStep", Range( 0 , 1)) = 0.5
		_DesoleGap("DesoleGap", Range( 0 , 1)) = 0.5
		_BaseAlbedo("BaseAlbedo", 2D) = "white" {}
		[HDR]_DesolveColor("DesolveColor", Color) = (0.359336,0.8018868,0.7973659,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _MaskStep;
		uniform float _DesoleGap;
		uniform sampler2D _BaseAlbedo;
		uniform float4 _BaseAlbedo_ST;
		uniform float4 _DesolveColor;
		uniform float _Smothness;


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
			float2 uv_TexCoord12 = i.uv_texcoord * float2( 1,1 );
			float gradientNoise1 = GradientNoise(uv_TexCoord12,10.0);
			gradientNoise1 = gradientNoise1*0.5 + 0.5;
			float temp_output_69_0 = ( 1.0 - _MaskStep );
			float OpacityMask16 = step( gradientNoise1 , temp_output_69_0 );
			float DesoleMask32 = step( gradientNoise1 , ( temp_output_69_0 - _DesoleGap ) );
			float4 color26 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
			float4 color40 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 DesolGap41 = ( OpacityMask16 != DesoleMask32 ? color26 : color40 );
			float4 color44 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float2 uv_BaseAlbedo = i.uv_texcoord * _BaseAlbedo_ST.xy + _BaseAlbedo_ST.zw;
			float4 Albedo51 = ( DesolGap41 == color44 ? tex2D( _BaseAlbedo, uv_BaseAlbedo ) : _DesolveColor );
			o.Albedo = Albedo51.rgb;
			o.Smoothness = _Smothness;
			o.Alpha = 1;
			clip( OpacityMask16 - _Smothness );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
1920;0;1920;1019;4230.667;3278.719;3.923408;True;True
Node;AmplifyShaderEditor.CommentaryNode;15;-2134.166,-540.5251;Inherit;False;1268.941;465.1832;Comment;11;16;32;31;21;38;1;29;12;11;22;69;Opacity Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;11;-2055.901,-400.8169;Inherit;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;22;-1844.398,-490.6249;Inherit;False;Property;_MaskStep;MaskStep;1;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1626.361,-156.2021;Inherit;False;Property;_DesoleGap;DesoleGap;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1898.902,-395.8169;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;69;-1539.295,-496.9;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;-1348.487,-185.2149;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-1620.901,-393.0168;Inherit;True;Gradient;True;False;2;0;FLOAT2;0.5,0.5;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;21;-1315.019,-390.0511;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;31;-1193.112,-288.972;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-1064.535,-266.6107;Inherit;False;DesoleMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-1135.339,-385.2545;Inherit;False;OpacityMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;39;-2133.963,-1258.856;Inherit;False;758.2993;622.1345;Comment;6;41;34;20;40;26;33;DesolGap;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-2098.154,-1128.818;Inherit;False;32;DesoleMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-2108.694,-1027.096;Inherit;False;Constant;_Color1;Color 1;3;0;Create;True;0;0;0;False;0;False;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;40;-2113.559,-821.8561;Inherit;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;33;-2098.802,-1208.856;Inherit;False;16;OpacityMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;34;-1815.148,-1040.194;Inherit;False;1;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-1624.743,-1018.349;Inherit;False;DesolGap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2139.003,-2071.029;Inherit;False;764.0938;744.2446;Albedo;6;51;49;44;43;42;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-2087.571,-2021.029;Inherit;False;41;DesolGap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;42;-2112.594,-1754.55;Inherit;True;Property;_BaseAlbedo;BaseAlbedo;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;47;-2113.891,-1556.452;Inherit;False;Property;_DesolveColor;DesolveColor;4;1;[HDR];Create;True;0;0;0;False;0;False;0.359336,0.8018868,0.7973659,1;0.359336,0.8018868,0.7973659,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;-2102.218,-1932.07;Inherit;False;Constant;_Color2;Color 2;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;49;-1777.814,-1801.255;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;64;-2146.162,-2784.373;Inherit;False;768.413;675.8063;Comment;6;62;61;58;55;63;59;Emision;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-1591.885,-1784.329;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-2084.544,-2734.373;Inherit;False;41;DesolGap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;63;-2093.469,-2484.254;Inherit;False;Property;_AlbedoEmisionColor;AlbedoEmisionColor;6;0;Create;True;0;0;0;False;0;False;0,0,0,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;61;-1809.689,-2555.312;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;55;-2089.864,-2304.795;Inherit;False;Property;_DesolveEmitionColor;DesolveEmitionColor;5;1;[HDR];Create;True;0;0;0;False;0;False;0.359336,0.8018868,0.7973659,1;0.359336,0.8018868,0.7973659,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;17;-488.1769,-1608.192;Inherit;False;16;OpacityMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-532.8861,-1746.097;Inherit;False;Property;_Smothness;Smothness;0;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-447.2833,-2021.185;Inherit;False;51;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-1660.963,-2554.916;Inherit;False;Emision;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;-510.9633,-1870.551;Inherit;False;62;Emision;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;59;-2094.191,-2658.414;Inherit;False;Constant;_Color3;Color 3;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-213.0468,-1910.199;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Disolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Overlay;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;True;6;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;0;11;0
WireConnection;69;1;22;0
WireConnection;38;0;69;0
WireConnection;38;1;29;0
WireConnection;1;0;12;0
WireConnection;21;0;1;0
WireConnection;21;1;69;0
WireConnection;31;0;1;0
WireConnection;31;1;38;0
WireConnection;32;0;31;0
WireConnection;16;0;21;0
WireConnection;34;0;33;0
WireConnection;34;1;20;0
WireConnection;34;2;26;0
WireConnection;34;3;40;0
WireConnection;41;0;34;0
WireConnection;49;0;43;0
WireConnection;49;1;44;0
WireConnection;49;2;42;0
WireConnection;49;3;47;0
WireConnection;51;0;49;0
WireConnection;61;0;58;0
WireConnection;61;1;59;0
WireConnection;61;2;63;0
WireConnection;61;3;55;0
WireConnection;62;0;61;0
WireConnection;0;0;54;0
WireConnection;0;4;6;0
WireConnection;0;10;17;0
ASEEND*/
//CHKSM=25CF5418BE74455E767D34310227ABCE939306F4