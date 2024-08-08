shader_type spatial;

uniform sampler2D img;
uniform float x_scale = 10.0;
uniform float y_scale = 10.0;
uniform float time = 0.5;
uniform float RTint: hint_range(0.0, 1.0, 0.1) = 0.5;
uniform float GTint: hint_range(0.0, 1.0, 0.1) = 0.5;
uniform float BTint: hint_range(0.0, 1.0, 0.1) = 0.5;
uniform bool Emision = false;

void fragment(){
	float x = mod(UV.x, 1.0/x_scale);
	float y = mod(UV.y, 1.0/y_scale);
	if( Emision){
		EMISSION = vec3(RTint, GTint, BTint);
	}
	ALBEDO = vec3(RTint, GTint, BTint);
	
	float direction;
	if(mod(UV.x, 2.0/x_scale)-mod(x, 1.0/x_scale) == 0.0){
		direction = -TIME/5.0;
	}else{
		direction = TIME/5.0;
	}
	
	vec3 tex = texture(img, vec2(x*x_scale, direction+TIME+y*y_scale)).rgb;
	float brightness = mod(UV.y - (TIME*time) + y + UV.x-x, 0.7);
	ALPHA = 0.2 + pow(tex.x * brightness * 0.2, 0.75);
	if(ALPHA > 0.5){EMISSION = vec3(0.5, 1.0, 0.5);}
}