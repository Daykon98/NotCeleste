// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PiotrMitrega/ColorSwap"
{
    Properties
    {
        _MainTex("Sprite", 2D) = "white" {}
        _ColorToChange("Color You Want To Change 1", Color) = (0,0,1,1)
        _DesiredColor("Desired Color 1", Color) = (1,0,0,1)
		_ColorToChange2("Color You Want To Change 2", Color) = (0,0,1,1)
        _DesiredColor2("Desired Color 2", Color) = (1,0,0,1)
		_ColorToChange3("Color You Want To Change 3", Color) = (0,0,1,1)
        _DesiredColor3("Desired Color 3", Color) = (1,0,0,1)
		_ColorToChange4("Color You Want To Change 4", Color) = (0,0,1,1)
        _DesiredColor4("Desired Color 4", Color) = (1,0,0,1)
		_ColorToChange5("Color You Want To Change 5", Color) = (0,0,1,1)
        _DesiredColor5("Desired Color 5", Color) = (1,0,0,1)
		_ColorToChange6("Color You Want To Change 6", Color) = (0,0,1,1)
        _DesiredColor6("Desired Color 6", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Transparent+1"
        }

        Pass
        {
		Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile DUMMY PIXELSNAP_ON

        sampler2D _MainTex;
        float4 _ColorToChange;
        float4 _ColorToChange2;
        float4 _ColorToChange3;
        float4 _ColorToChange4;
		float4 _ColorToChange5;
		float4 _ColorToChange6;

		
        float4 _DesiredColor;
        float4 _DesiredColor2;
        float4 _DesiredColor3;
        float4 _DesiredColor4;
		float4 _DesiredColor5;
		float4 _DesiredColor6;



        struct Vertex
        {
            float4 vertex : POSITION;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2 : TEXCOORD1;
        };

        struct Fragment
        {
            float4 vertex : POSITION;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2 : TEXCOORD1;
        };

        Fragment vert(Vertex v)
        {
            Fragment o;

            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv_MainTex = v.uv_MainTex;
            o.uv2 = v.uv2;

            return o;
        }

        float4 frag(Fragment IN) : COLOR
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);

                if (c.r >= _ColorToChange.r - 0.005 && c.r <= _ColorToChange.r + 0.005
                && c.g >= _ColorToChange.g - 0.005 && c.g <= _ColorToChange.g + 0.005
                    && c.b >= _ColorToChange.b - 0.005 && c.b <= _ColorToChange.b + 0.005)
            {
                return _DesiredColor;
            }
			
			if (c.r >= _ColorToChange2.r - 0.005 && c.r <= _ColorToChange2.r + 0.005
                && c.g >= _ColorToChange2.g - 0.005 && c.g <= _ColorToChange2.g + 0.005
                    && c.b >= _ColorToChange2.b - 0.005 && c.b <= _ColorToChange2.b + 0.005)
            {
                return _DesiredColor2;
            }
			
			if (c.r >= _ColorToChange3.r - 0.005 && c.r <= _ColorToChange3.r + 0.005
                && c.g >= _ColorToChange3.g - 0.005 && c.g <= _ColorToChange3.g + 0.005
                    && c.b >= _ColorToChange3.b - 0.005 && c.b <= _ColorToChange3.b + 0.005)
            {
                return _DesiredColor3;
            }
			
			if (c.r >= _ColorToChange4.r - 0.005 && c.r <= _ColorToChange4.r + 0.005
                && c.g >= _ColorToChange4.g - 0.005 && c.g <= _ColorToChange4.g + 0.005
                    && c.b >= _ColorToChange4.b - 0.005 && c.b <= _ColorToChange4.b + 0.005)
            {
                return _DesiredColor4;
            }
			
			if (c.r >= _ColorToChange5.r - 0.005 && c.r <= _ColorToChange5.r + 0.005
                && c.g >= _ColorToChange5.g - 0.005 && c.g <= _ColorToChange5.g + 0.005
                    && c.b >= _ColorToChange5.b - 0.005 && c.b <= _ColorToChange5.b + 0.005)
            {
                return _DesiredColor5;
            }
			
			if (c.r >= _ColorToChange6.r - 0.005 && c.r <= _ColorToChange6.r + 0.005
                && c.g >= _ColorToChange6.g - 0.005 && c.g <= _ColorToChange6.g + 0.005
                    && c.b >= _ColorToChange6.b - 0.005 && c.b <= _ColorToChange6.b + 0.005)
            {
                return _DesiredColor6;
            }

            return c;
        }
            ENDCG
        }
    }
}