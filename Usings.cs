global using Raylib_cs;
global using jh_lib;
global using System.Text.Json;
global using System.Text;
global using TextCopy;
global using System.Security.Cryptography;

global using Rectangle = Raylib_cs.Rectangle;
global using Clipboard = TextCopy.ClipboardService;
global using Color = Raylib_cs.Color;
global using Font = Raylib_cs.Font;
global using Image = Raylib_cs.Image;
global using Button = jh_lib.Button;

class MyIcons : Icons
{
  public static vec2 penInCircle = new vec2(120,24);
  public static vec2 xInCircle = new vec2(120,0);
  public static vec2 tickInCircle = new vec2(72,24);
  public static vec2 twoSquares = new vec2(144,24);
  public static vec2 paintersPalette = new vec2(168,24);
  public static vec2 iconsInCircle = new vec2(72,48);
  public static vec2 TInCircle = new vec2(48,0);
  public static vec2 AInCircle = new vec2(48,24);
  public static vec2 paintBucketInCircle = new vec2(72,0);
  public static vec2 plusInCircle = new vec2(96,0);
  public static vec2 fntInFile = new vec2(48,48);
  public static vec2 file = new vec2(48,72);
  public static vec2 bigGear = new vec2(0,24);
  public static vec2 smallGear = new vec2(72,72);
  public static vec2 starInFile = new vec2(144,48);
  public static vec2 openFolder = new vec2(168,48);
  public static vec2 fileInOpenFolder = new vec2(192,48);
  public static vec2 floppyDisk = new vec2(216,48);
  public static vec2 floppyDiskAndStar = new vec2(240,48);

}