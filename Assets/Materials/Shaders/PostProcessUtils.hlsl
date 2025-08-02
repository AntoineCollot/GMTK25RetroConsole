
half random(half2 p){return frac(cos(dot(p,half2(23.14069263277926,2.665144142690225)))*12345.6789);}

float2 fracUVs(float2 uv)
{
	uv.x*=10;
	uv.y*=9;
	uv = floor(uv);
	uv.x*=0.1;
	uv.y/=9;
	return uv;
}

void glitchUV_float(in float2 UV,in float2 WPos, in half GlitchAmount, out float2 GlitchedUV)
{
	float2 fracUV = fracUVs(UV+float2(WPos.x*0.1+0.5, WPos.y/9 + 0.5));
	half rand = random(fracUV) * 5 + 0.001;
	half rand2 = random(fracUV+half2(0.43543,0.3545)) * 5+ 0.001;
	
	GlitchedUV = step(GlitchAmount,rand) * UV + step(rand,GlitchAmount) * (1-UV);
	GlitchedUV = step(GlitchAmount,rand2) * GlitchedUV + step(rand2,GlitchAmount) * (random(UV));
}

void glitchColor_float(in half4 Col, in float2 UV,in float2 WPos,in half GlitchAmount, out half4 GlitchedCol)
{
	float2 fracUV = fracUVs(UV+float2(WPos.x*0.1+0.5, WPos.y/9 + 0.5))+ 0.001;
	half rand = random(fracUV+half2(0.46486,0.7946)) * 3+ 0.001;
	
	GlitchedCol = step(GlitchAmount,rand) * Col + step(rand,GlitchAmount) * (1-Col);
}