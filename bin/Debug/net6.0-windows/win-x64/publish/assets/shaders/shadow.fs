#version 330

// Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;
in vec4 fragColor;
// Input uniform values
uniform sampler2D texture0;

uniform vec2 size = vec2(0.5);
uniform vec4 col = vec4(1);
// x = rec.w
// y = rec.h
// z = size
// w = max(rec.w, rec.h)

out vec4 finalColor;

float getShadowColorValue(vec2 p, vec4 r)
{
  vec2 tl = r.xy;
  vec2 br = r.xy + r.zw;
  vec2 d = max(tl-p, p-br);
  return length(max(vec2(0.0), d)) + min(0.0, max(d.x, d.y));
}

void main()
{
  float width = size.x;
  float height = size.y;
  float d = getShadowColorValue(fragTexCoord, vec4(0.5-width/2, 0.5-height/2, width, height));
  float rgb = 1-d;
  
  finalColor = vec4(rgb*col.r,rgb*col.g,rgb*col.b, 1-(d*10)-(1-col.a));
}