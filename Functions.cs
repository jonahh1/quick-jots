class Functions
{
  public static Color HexToRGB(string hex, string alpha = "ff")
  // it just works
  {
    hex = hex.Replace("#", "");
    byte r = 255;
    byte g = 255;
    byte b = 255;
    byte a = 255;
    System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.HexNumber;
    if (hex.Length >= 6)
    {
      byte.TryParse(hex.Substring(0, 2), numberStyle, null, out r);
      byte.TryParse(hex.Substring(2, 2), numberStyle, null, out g);
      byte.TryParse(hex.Substring(4, 2), numberStyle, null, out b);
      if (hex.Length == 8) byte.TryParse(hex.Substring(6, 2), numberStyle, null, out a);
    }
    else if (hex.Length >= 3)
    {
      byte.TryParse(String.Concat(Enumerable.Repeat(hex.Substring(0, 1),2)), numberStyle, null, out r);
      byte.TryParse(String.Concat(Enumerable.Repeat(hex.Substring(1, 1),2)), numberStyle, null, out g);
      byte.TryParse(String.Concat(Enumerable.Repeat(hex.Substring(2, 1),2)), numberStyle, null, out b);
      if (hex.Length == 4) byte.TryParse(String.Concat(Enumerable.Repeat(hex.Substring(3, 1),2)), numberStyle, null, out a);
    }
    else return new Color(255,0,255,255);

    if (alpha.Length == 1) alpha += alpha;

    if (a == 255) byte.TryParse(alpha, System.Globalization.NumberStyles.HexNumber, null, out a);
    return new Color(r, g, b, a);
  }
}
