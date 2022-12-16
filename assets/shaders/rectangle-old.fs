#version 330

// Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;
in vec4 fragColor;
// Input uniform values
uniform sampler2D texture0;

uniform vec3 size = vec3(0.5);
uniform vec4 curvature = vec4(0);
uniform vec4 col = vec4(1);

uniform vec4 shadowCol = vec4(1);
uniform vec2 borderInfo = vec2(1,0);
uniform vec4 borderCol = vec4(1);
// x = rec.w
// y = rec.h
// z = size
// w = max(rec.w, rec.h)

out vec4 finalColor;

float smoothDistance(vec2 p, vec4 r)
{
  vec2 tl = r.xy;
  vec2 br = r.xy + r.zw;
  vec2 d = max(tl-p, p-br);
  return length(max(vec2(0.0), d)) + min(0.0, max(d.x, d.y));
}
bool curvatureForPixel(float dA, float dB, float dC, float dD, vec2 p)
{
  // quadrants of rectangle
  //  A | B
  // ---|---
  //  C | D

  if (p.x < .5) // A or C
  {
    if (p.y < .5) return dA<curvature.x; // A
    if (p.y > .5) return dC<curvature.z; // C
  }/*
  else // B or D
  {
    if (p.y < .5) return dB<curvature.y; // B
    if (p.y > .5) return dD<curvature.w; // D
  }*/
  else return 1==0;
}
void main()
{
  float width = size.x;
  float height = size.y;
  float d = smoothDistance(fragTexCoord, vec4(0.5-width/2, 0.5-height/2, width, height));
  float maxC = max(max(curvature.x,curvature.y),max(curvature.z,curvature.w));
  // distances for each amount of curvature
  width = size.x+(maxC-curvature.x);
  height = size.y+(maxC-curvature.x);
  float dA = smoothDistance(fragTexCoord, vec4(0.5-width/2, 0.5-height/2, width/2, height/2));

  width = size.x+(maxC-curvature.y);
  height = size.y+(maxC-curvature.y);
  float dB = smoothDistance(fragTexCoord, vec4(width/2, 0.5-height/2, width/2, height/2));

  width = size.x+(maxC-curvature.z);
  height = size.y+(maxC-curvature.z);
  float dC = smoothDistance(fragTexCoord, vec4(0.5-width/2, 0, width/2, height/2));

  width = size.x+(maxC-curvature.w);
  height = size.y+(maxC-curvature.w);
  float dD = smoothDistance(fragTexCoord, vec4(0,0, width/2, height/2));

  float rgb = 1-d;
  if (d < curvature.x) finalColor = col;

  /*if (d > 0) finalColor = vec4(rgb*shadowCol.r,rgb*shadowCol.g,rgb*shadowCol.b, 1-(d*10)-(1-shadowCol.a));

  float combinedAlpha = col.a + borderCol.a;
  if (d > -borderInfo.x && d <0) finalColor = vec4(mix(col, borderCol, borderCol.a).rgb, combinedAlpha);*/
  //finalColor = col;
  finalColor = vec4(fragTexCoord, 0,1);
  if (curvatureForPixel(dA,dB,dC,dD,fragTexCoord)) finalColor = col;
  //if (curvatureForPixel(d,d,d,d,fragTexCoord)) finalColor = col;
  if (d < 0) finalColor = vec4(fragTexCoord, 0,1);
}