#version 330

// Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;
in vec4 fragColor;
// Input uniform values
uniform sampler2D texture0;

uniform vec3 size = vec3(0.5);
uniform vec4 curvature = vec4(0);
uniform int curveInwards = 1;
uniform vec4 col = vec4(1);

uniform vec4 shadowCol = vec4(1);
uniform vec2 borderInfo = vec2(1,0);
uniform vec4 borderCol = vec4(1);

out vec4 finalColor;

float smoothDistance(vec2 p, vec4 r)
{
  vec2 tl = r.xy;
  vec2 br = r.xy + r.zw;
  vec2 d = max(tl-p, p-br);
  return length(max(vec2(0.0), d)) + min(0.0, max(d.x, d.y));
}
bool pointInsideRect(float dA, float dB, float dC, float dD, vec2 p)
{
  // quadrants of rectangle
  //  A | B
  // ---|---
  //  C | D

  if (p.x < .5) // A or C
  {
    if (p.y < .5) return dA<curvature.x; // A
    else return dC<curvature.z; // C
  }
  else // B or D
  {
    if (p.y < .5) return dB<curvature.y; // B
    else return dD<curvature.w; // D
  }
}

vec4 blend(vec4 background, vec4 foreground)
{
  float combinedAlpha = background.a + foreground.a;
  return vec4(mix(background, foreground, foreground.a).rgb, combinedAlpha);
}

void main()
{
  float w = size.x;
  float h = size.y;
  if (curveInwards == 1)
  {
    w -= 2*size.z;
    h -= 2*size.z;
  }
  float d = smoothDistance(fragTexCoord, vec4(0.5-size.x/2, 0.5-size.y/2, size.x, size.y));
  
  float width = w + 2*(size.z-curvature.x);
  float height = h + 2*(size.z-curvature.x);
  float dA = smoothDistance(fragTexCoord, vec4(0.5-width/2, 0.5-height/2, width/2, height/2));

  width = w + 2*(size.z-curvature.y);
  height = h + 2*(size.z-curvature.y);
  float dB = smoothDistance(fragTexCoord, vec4(0.5, 0.5-height/2, width/2, height/2));

  width = w + 2*(size.z-curvature.z);
  height = h + 2*(size.z-curvature.z);
  float dC = smoothDistance(fragTexCoord, vec4(0.5-width/2, 0.5, width/2, height/2));

  width = w + 2*(size.z-curvature.w);
  height = h + 2*(size.z-curvature.w);
  float dD = smoothDistance(fragTexCoord, vec4(0.5, 0.5, width/2, height/2));
  
  bool fragInRect = pointInsideRect(dA,dB,dC,dD, fragTexCoord);
  //vec4 recAndShadowCol = vec4(0);

  if (fragInRect) finalColor = col;
  else
  {
    float shadowD = d;
    if (fragTexCoord.x < .5) // A or C
    {
      if (fragTexCoord.y < .5) shadowD = dA-curvature.x;
      else shadowD = dC-curvature.z;
    }
    else // B or D
    {
      if (fragTexCoord.y < .5) shadowD = dB-curvature.y;
      else shadowD = dD-curvature.w;
    }
    float rgb = 1-shadowD;
    finalColor = vec4(rgb*shadowCol.r,rgb*shadowCol.g,rgb*shadowCol.b, 1-(shadowD*10)-(1-shadowCol.a));
  }

}
//TODO: borders 