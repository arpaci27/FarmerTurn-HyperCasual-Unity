half4 SampleTextureWithScroll(sampler2D tex, float2 uv, half scrollXSpeed, half scrollYSpeed, float time)
{
	uv.x += (time * scrollXSpeed) % 1;
	uv.y += (time * scrollYSpeed) % 1;
	return tex2D(tex, uv);
}

float2 RotateUvs(float2 uv, half rotation, half4 scaleAndTranslate)
{
	half2 center = half2(0.5 * scaleAndTranslate.x + scaleAndTranslate.z, 0.5 * scaleAndTranslate.y + scaleAndTranslate.w);
	half cosAngle = cos(rotation);
	half sinAngle = sin(rotation);
	uv -= center;
	uv = mul(half2x2(cosAngle, -sinAngle, sinAngle, cosAngle), uv);
	uv += center;
	return uv;
}

half Rand(half2 seed, half offset) {
	return (frac(sin(dot(seed, half2(12.9898, 78.233))) * 43758.5453) + offset) % 1.0;
}

half Rand2(half2 seed, half offset) {
	return (frac(sin(dot(seed * floor(50 + (_Time.x % 1.0) * 12.), half2(127.1, 311.7))) * 43758.5453123) + offset) % 1.0;
}

half RemapFloat(half inValue, half inMin, half inMax, half outMin, half outMax){
	return outMin + (inValue - inMin) * (outMax - outMin) / (inMax - inMin);
}

half EaseOutQuint(half x) {
	return 1 - pow(1 - x, 5);
}