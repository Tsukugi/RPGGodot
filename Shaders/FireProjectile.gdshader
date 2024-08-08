shader_type spatial;

uniform float heightInterval: hint_range(0.0, 50.0, 0.1) = 2.0;
uniform float heightLimit: hint_range(0.0, 10.0, 0.1) = 2.0;
uniform float heightDeformation: hint_range(0.0, 10.0, 0.1) = 2.0;
uniform float colorInterval: hint_range(0.0, 30.0, 0.1) = 2.0;
uniform float longitude: hint_range(0.0, 10.0, 0.1) = 2.0;

uniform float redMagnitude: hint_range(0.0, 1.0, 0.1) = 1.0;
uniform float greenMagnitude: hint_range(0.0, 1.0, 0.1) = 1.0;
uniform float blueMagnitude: hint_range(0.0, 1.0, 0.1) = 1.0;
uniform float alphaMagnitude: hint_range(0.0, 100.0, 0.1) = 1.0;

uniform float darkEffect: hint_range(0.0, 1.0, 0.1) = 1.0;

void vertex() {
	VERTEX.x -= 0.2;
	VERTEX.y += cos(TIME * heightInterval + VERTEX.x * heightDeformation) * heightLimit;
	VERTEX.x += VERTEX.x * longitude;
}

void fragment() {
	ROUGHNESS = 0.3;
	METALLIC = 1.0;
	RIM = 1.0;
	vec3 color = ALBEDO;
	color.r += sin(TIME * colorInterval) + redMagnitude;
	color.g += sin(TIME * colorInterval) + greenMagnitude;
	color.b += sin(TIME * colorInterval) + blueMagnitude;
	ALBEDO = color;
	ALPHA = darkEffect / sqrt(pow(VERTEX.x, 2) + pow(VERTEX.y, 2)) - 10.0 + (sin(TIME * alphaMagnitude) + 1.0) * 0.1;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
