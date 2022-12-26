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

  /* fractal ? */
  /*vec2 scale = vec2(1, 1);
  vec2 translate = vec2(-1.0, -1.0);

  // Transform the texture coordinates using the scale and translation values
  vec2 z = (fragTexCoord * scale) + translate;
  for (int i = 0; i < 100; i++)
  {
    // Update the value of z according to the Mandelbrot set equation
    z = vec2(z.x*z.x - z.y*z.y, 2.0*z.x*z.y) + fragTexCoord;

    // If the magnitude of z is greater than 2, it is outside of the Mandelbrot set
    if (dot(z, z) > 4.0)
    {
      finalColor = vec4(0,0,0,1);
      break;
    }
  }

  // If the loop completed without finding a value of z outside of the Mandelbrot set, color the pixel white
  if (finalColor != vec4(0,0,0,1)) finalColor = vec4(1);*/
}