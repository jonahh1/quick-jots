class MyInput
{
  /* key lifetime

  x: half a second 

  1,0*x,(1,0)*rest of time
  */
  public static int[] characterKeySet = {
    // this mess:  ` 1234567890-=+\~!@#£€¦`¬$%^&*()_+|;:'\",.<>/?qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM[]{}\n
    96,32,49,50,51,52,53,54,55,56,57,48,45,61,43,92,126,33,64,35,163,8364,166,96,172,36,37,94,38,42,40,41,95,43,124,59,58,
    39,34,44,46,60,62,47,63,113,119,101,114,116,121,117,105,111,112,97,115,100,102,103,104,106,107,108,122,120,99,118,98,
    110,109,81,87,69,82,84,89,85,73,79,80,65,83,68,70,71,72,74,75,76,90,88,67,86,66,78,77,91,93,123,125,10,

    // other duplicate chars on keyboard
    331,332,333,334,
    
  };
  public static int[] controlKeySet = {
    263,262,265,264,259,261,257,258,268,269
  };
  public static int[] numLockKeySet = {
    327, 328, 329,
    324, 325, 326,
    321, 322, 323,
    320, 330, 335,
  };

  public static int currentKey = 0;

  static int prevKey = 0;
  static bool startTimer = false;
  static float timer = 0; // half second timer
  
  public static int totalFrames = 0;
  public static bool ctrl = false;
  public static bool shift = false;

  public static int keyPressedThisFrame = 0;

  public static int GetKeyPressed()
  {
    ctrl = Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL);
    shift = Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT);
    int[] fullKeySet = Concatenate(characterKeySet, controlKeySet);
    int rawKey = 0;
    for (int i = 0; i < fullKeySet.Length; i++)
    {
      if (!ctrl && Raylib.IsKeyDown((KeyboardKey)fullKeySet[i]))
      {
        rawKey = fullKeySet[i];
      }
    }
    //if (rawKey == 0) return 0;
    //currKey = rawKey;
    int ret = 0;
    if (prevKey == 0 && rawKey > 0)
    {
      ret = rawKey;
      startTimer = true;
      prevKey = rawKey;
      return rawKey;
    }
    if (prevKey > 0 && rawKey > 0 && prevKey != rawKey)
    {
      timer = 0;
      startTimer = false;
    }
    if (startTimer && rawKey>0) timer += Raylib.GetFrameTime();
    else if (rawKey == 0) timer = 0;
    if (timer >= .5f) ret = rawKey;

    prevKey = rawKey;

    return ret;
  }
  public static T[] Concatenate<T>(T[] first, T[] second)
  {
    if (first == null) return second;
    if (second == null) return first;
    return first.Concat(second).ToArray();
  }
}