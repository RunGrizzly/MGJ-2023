// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FoliUIBasic"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_ShineAngle("ShineAngle", Float) = 0
		[NoScaleOffset]_ShineShape("ShineShape", 2D) = "white" {}
		_Tile("Tile", Float) = 0
		_Offset("Offset", Float) = 0
		_PauseLength("Pause Length", Range( 0 , 10)) = 1
		_ShineSpeed("ShineSpeed", Float) = 0
		_Float0("Float 0", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float _Float0;
			uniform float _PauseLength;
			uniform float4 _MainTex_ST;
			uniform sampler2D _ShineShape;
			uniform float _Tile;
			uniform float _Offset;
			uniform float _ShineSpeed;
			uniform float _ShineAngle;
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				float2 texCoord81 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_28_0 = ( 1.0 / _PauseLength );
				float temp_output_20_0 = sin( ( ( 0.0 + _Time.y ) * temp_output_28_0 ) );
				
				
				OUT.worldPosition.xyz += float3( ( ( texCoord81 - float2( 0.5,0.5 ) ) * ( _Float0 * temp_output_20_0 ) ) ,  0.0 );
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2D( _MainTex, uv_MainTex );
				float4 appendResult63 = (float4(tex2DNode4.r , tex2DNode4.g , tex2DNode4.b , 0.0));
				float3 hsvTorgb9 = RGBToHSV( appendResult63.xyz );
				float mulTime32 = _Time.y * _ShineSpeed;
				float temp_output_28_0 = ( 1.0 / _PauseLength );
				float temp_output_20_0 = sin( ( ( 0.0 + _Time.y ) * temp_output_28_0 ) );
				float clampResult41 = clamp( ( ( mulTime32 % ( ( ( 2.0 * UNITY_PI ) / temp_output_28_0 ) / 2.0 ) ) * ( step( sign( ( -1.0 + ( 1.0 * temp_output_20_0 ) ) ) , 0.0 ) * 15.0 ) ) , -20.0 , 20.0 );
				float cos46 = cos( _ShineAngle );
				float sin46 = sin( _ShineAngle );
				float2 rotator46 = mul( (IN.texcoord.xy*_Tile + ( _Offset + clampResult41 )) - float2( 0.5,0.5 ) , float2x2( cos46 , -sin46 , sin46 , cos46 )) + float2( 0.5,0.5 );
				float3 hsvTorgb61 = HSVToRGB( float3(hsvTorgb9.x,hsvTorgb9.y,( hsvTorgb9.z + tex2D( _ShineShape, rotator46 ).a )) );
				float4 appendResult62 = (float4(hsvTorgb61.x , hsvTorgb61.y , hsvTorgb61.z , tex2DNode4.a));
				
				half4 color = ( IN.color * appendResult62 );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
2560;0;1920;1059;1845.295;2685.396;2.263353;True;False
Node;AmplifyShaderEditor.RangedFloatNode;37;-3679.426,-358.4543;Inherit;False;Constant;_LeftShift;LeftShift;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-4142.827,67.84425;Inherit;False;Property;_PauseLength;Pause Length;4;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;44;-3757.126,-228.4556;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-3464.426,-251.4556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;28;-3539.915,-94.94071;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-3296.426,-122.4557;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;20;-3113.426,-227.4556;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-3116.218,-389.6964;Inherit;False;Constant;_Strength;Strength;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-2910.218,-463.6964;Inherit;False;Constant;_VerticalShift;VerticalShift;6;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2914.833,-328.8225;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;31;-3707.518,130.6022;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-2735.219,-390.6964;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-3423.468,-845.2324;Inherit;False;Property;_ShineSpeed;ShineSpeed;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;27;-3303.923,131.4504;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SignOpNode;35;-2433.028,-501.4553;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;34;-2286.427,-503.6543;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;32;-3097.806,-759.3623;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;26;-3067.218,132.3035;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleRemainderNode;23;-2541.244,-760.7063;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-2094.146,-502.4133;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1916.803,-761.9473;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;41;-1677.744,-582.8032;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-20;False;2;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1819.817,-1107.603;Inherit;False;Property;_Offset;Offset;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1434.825,-1099.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;74;-428.8052,-2310.1;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;16;-1551.026,-1494.324;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;-1524.215,-1315.734;Inherit;False;Property;_Tile;Tile;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-122.4879,-2055.385;Inherit;True;Property;_TextureSample;Texture Sample;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0,0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;17;-1167.027,-1332.198;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-938.7225,-1148.888;Inherit;False;Property;_ShineAngle;ShineAngle;0;0;Create;True;0;0;0;False;0;False;0;38.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;45;-950.6431,-1569.174;Inherit;True;Property;_ShineShape;ShineShape;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RotatorNode;46;-652.0178,-1332.712;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;63;329.3631,-2093.905;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;42;-420.6861,-1563.729;Inherit;True;Property;_TextureSample1;Texture Sample 1;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RGBToHSVNode;9;599.4436,-2169.671;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;43;907.758,-2094.676;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;61;1070.976,-2141.292;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;80;1696.014,-1814.219;Inherit;False;Property;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;1786.447,-2257.241;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;62;1323.46,-2016.082;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;1828.905,-1812.435;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;82;2139.447,-2147.241;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateFragmentDataNode;69;1332.433,-2347.475;Inherit;False;0;0;color;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;78;1818.447,-2381.241;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StickyNoteNode;12;-4680.569,-557.9323;Inherit;False;516.3999;302.9;Shine Wave;;1,1,1,1;Follows the basic formula of y = ASin(B(x+C))+D$$A is the gamut of the wave - its min max$B is the frequency of the wave$C is the phase shift of the wave$D is the vertical shift of the wave$$We plug the period of the wave into a sawtooth to get a linearly scrolling U component$$We plug the sign of the wave into the sawtooth to define when we want it to move and when we don't - this gives us a pause function between waves;0;0
Node;AmplifyShaderEditor.DistanceOpNode;75;2135.447,-2316.241;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateVertexDataNode;64;1067.041,-2568.812;Inherit;False;0;0;color;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;73;-895.7128,-2240.091;Inherit;True;Property;_Texture0;Texture 0;6;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;2510.447,-2251.241;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;20;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StickyNoteNode;14;-2010.147,-393.4124;Inherit;False;150;100;Gamut;;1,1,1,1;This extends our bounds to the UV limit we desire;0;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;6;-1085.127,-2037.3;Inherit;False;0;0;_TextureSampleAdd;Pass;True;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StickyNoteNode;10;-3066.835,233.2435;Inherit;False;286.4994;159.6469;Divide by 2;;1,1,1,1;We only want the positve section of the sin wave, so we divide by 2;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-3771.114,-106.6407;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;1641.672,-2489.555;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StickyNoteNode;13;-3392.27,-517.6323;Inherit;False;363;104;Pause Strength (A);;1,1,1,1;The strength of the multiplier that is output. It's fine being 1;0;0
Node;AmplifyShaderEditor.StickyNoteNode;11;-4266.915,-63.94072;Inherit;False;363;104;Pause Length/Frequency (B);;1,1,1,1;Pause length must be a minimum of 1 to allow the period to finish. We divide by 1 to make the number more intuitive;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;66;2885.083,-2490.824;Float;False;True;-1;2;ASEMaterialInspector;0;6;FoliUIBasic;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-8;False;False;False;False;False;False;False;True;True;0;True;-4;255;True;-7;255;True;-6;0;True;-3;0;True;-5;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-10;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;19;0;37;0
WireConnection;19;1;44;0
WireConnection;28;1;30;0
WireConnection;18;0;19;0
WireConnection;18;1;28;0
WireConnection;20;0;18;0
WireConnection;21;0;39;0
WireConnection;21;1;20;0
WireConnection;22;0;38;0
WireConnection;22;1;21;0
WireConnection;27;0;31;0
WireConnection;27;1;28;0
WireConnection;35;0;22;0
WireConnection;34;0;35;0
WireConnection;32;0;33;0
WireConnection;26;0;27;0
WireConnection;23;0;32;0
WireConnection;23;1;26;0
WireConnection;36;0;34;0
WireConnection;40;0;23;0
WireConnection;40;1;36;0
WireConnection;41;0;40;0
WireConnection;25;0;24;0
WireConnection;25;1;41;0
WireConnection;4;0;74;0
WireConnection;17;0;16;0
WireConnection;17;1;47;0
WireConnection;17;2;25;0
WireConnection;46;0;17;0
WireConnection;46;2;48;0
WireConnection;63;0;4;1
WireConnection;63;1;4;2
WireConnection;63;2;4;3
WireConnection;42;0;45;0
WireConnection;42;1;46;0
WireConnection;9;0;63;0
WireConnection;43;0;9;3
WireConnection;43;1;42;4
WireConnection;61;0;9;1
WireConnection;61;1;9;2
WireConnection;61;2;43;0
WireConnection;62;0;61;1
WireConnection;62;1;61;2
WireConnection;62;2;61;3
WireConnection;62;3;4;4
WireConnection;83;0;80;0
WireConnection;83;1;20;0
WireConnection;82;0;81;0
WireConnection;75;0;81;0
WireConnection;79;0;82;0
WireConnection;79;1;83;0
WireConnection;29;1;30;0
WireConnection;67;0;69;0
WireConnection;67;1;62;0
WireConnection;66;0;67;0
WireConnection;66;1;79;0
ASEEND*/
//CHKSM=D6FCBD841801F503A1CC04D30F4527CD618D6C7A