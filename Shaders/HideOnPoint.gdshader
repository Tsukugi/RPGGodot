shader_type spatial;
render_mode specular_toon;

uniform float velocityMultiplier: hint_range(0.0, 10.0, 0.1) = 1.0;
uniform float widthMultiplier: hint_range(0.0, 10.0, 0.1) = 1;
uniform vec3 color1 = vec3(1,1,0);
uniform vec3 color2 = vec3(0,0,0);

void vertex() {
	VERTEX.x += sin(((TIME * velocityMultiplier) + (VERTEX.z * widthMultiplier)));
	VERTEX.y += cos(((TIME * velocityMultiplier) + (VERTEX.z * widthMultiplier))) * 0.5;
}

void fragment() {
	ROUGHNESS = 1.0;
	RIM = 0.0;
	if(UV.x > 0.5){
		if(UV.y > 0.5) {
			ALBEDO = color1;
		} else {
			ALBEDO = color2;
		}
	} else {
		if(UV.y > 0.5) {
			ALBEDO = color2;
		} else {
			ALBEDO = color1;
		}
	}
	
}

void light() {
}