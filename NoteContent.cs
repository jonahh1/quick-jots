class NoteContent
{
  /*public static int selectedNoteIndex = -1;
  public static void DrawSelectedNote()
  {
    float buffer = 24;
    rect rec = new rect(NoteListManager.rec.w, 0, env.window.w-NoteListManager.rec.w, env.window.h);
    if (selectedNoteIndex == -1)
    {
      Draw.TextAnchored("press the arrow beside a note to select it", rec.center, Anchor.middleCenter, 24, "2a2a2a");
      return;
    }
    Note note = NoteListManager.notes[selectedNoteIndex];
    Draw.Rectangle(rec.x,rec.y,rec.w,buffer*2, note.bannerCol);
    Draw.TextAnchored(note.title, rec.xy+buffer, Anchor.middleLeft, 32, "fff");
    //Draw.Text(note.content, rec.xy, 18, "fff");

  }*/
  public static int selectedNoteIndex = 0;
  //public static vec2 pointer = new vec2();
  //static int rawPointer = 0 ;
  public static void DrawNoteContent()
  {
    while (NoteManager.notes.Count <= selectedNoteIndex) selectedNoteIndex--;
    float buffer = 24;
    rect rec = new rect(NoteList.rec.w, 0, env.window.w-NoteList.rec.w, env.window.h);
    
    Note note = NoteManager.notes[selectedNoteIndex];
    Raylib.DrawRectangleGradientV(
      (int)(rec.x), 
      (int)(Window.menuBarHeight+rec.y), 
      (int)(rec.w), 
      (int)(buffer*4),Utils.HexToRGB(note.bannerCol), Utils.HexToRGB(note.bannerCol+"00")
    );
    Draw.TextAnchored(note.title, rec.xy+buffer, Anchor.middleLeft, 32, env.theme.colors["text"]);

    //TODO: big, fully featured text editor, -_-
    int fontSize = 18;
    string content = note.content;
    int rawPointer = note.pointer;
    if (NoteList.menuOpen || MenuBarOptions.menuOpen) goto skipEditStage;
    // text editor
    
    int key = Raylib.GetCharPressed();
    while (key > 0)
    {
      // NOTE: Only allow keys in range [32..125]
      if ((key >= 32) && (key <= 125))
      {
        content = content.Insert(rawPointer, ""+(char)key);
        rawPointer++;
      }

      key = Raylib.GetCharPressed();
    }
    key = MyInput.keyPressedThisFrame;
    bool ctrl = Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL);
    bool shift = Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT);
    if (key == (int)KeyboardKey.KEY_BACKSPACE && content.Length > 0 && rawPointer > 0)
    {
      content = content.Remove(rawPointer-1,1);
      rawPointer--;
    }
    if (key == (int)KeyboardKey.KEY_ENTER && rawPointer >= 0)
    {
      content = content.Insert(rawPointer, "\n");
      rawPointer++;
    }
    if (key == (int)KeyboardKey.KEY_TAB)
    {
      content = content.Insert(rawPointer, "\t");
      rawPointer++;
    }
    if (ctrl && Raylib.IsKeyPressed(KeyboardKey.KEY_V))
    {
      string pastedText = Clipboard.GetText().Replace("\r","");
      //if (pastedText == null) pastedText = "";
      content = content.Insert(rawPointer, pastedText);
      rawPointer+=pastedText.Length;
    }

    note.content = content;
    NoteManager.notes[selectedNoteIndex] = note;
    content += "\0";

    if (key == (int)KeyboardKey.KEY_LEFT && rawPointer > 0) rawPointer--;
    if (key == (int)KeyboardKey.KEY_RIGHT && rawPointer < content.Length-1) rawPointer++;
    
    if (key == (int)KeyboardKey.KEY_DOWN) // nightmare code
    {
      int trackX=0,trackY=0;
      int pointerX=0,pointerY=0;
      for (int i = 0; i < content.Length; i++)
      {
        trackX++;

        if (rawPointer == i)
        {
          pointerX = trackX;
          pointerY = trackY;
        }
        if (i > rawPointer && trackY == pointerY+1)
        {
          if (trackX == pointerX)
          {
            rawPointer = i;
            break;
          }
          else
          {
            for (int n = i; n > 0; n--) if (content[n] == '\n' || content[n] == '\0')
            {
              rawPointer = n;
              break;
            }
          }
        }
        if (content[i] == '\n')
        {
          trackX=0;
          trackY++;
        }
      }
    }
    if (key == (int)KeyboardKey.KEY_UP) // nightmare code
    {
      int tx=0,ty=0; // temp x & y
      for (int i = 0; i < content.Length; i++)
      {
        tx++;
        if (i == rawPointer)
        {
          int nx=0,ny=0;
          for (int n = 0; n < content.Length; n++)
          {
            nx++;
            if (ny == ty-1)
            {
              if (nx == tx)
              {
                rawPointer = n;
                break;
              }
              else 
              {
                for (int m = n; m > 0; m--) if (content[m] == '\n')
                {
                  rawPointer = m;
                  break;
                }
              }
            }
            if (content[n] == '\n')
            {
              nx=0;
              ny++;
            }
          }
          break;
        }
        if (content[i] == '\n')
        {
          tx=0;
          ty++;
        }
      }
    }
    
    if (key == (int)KeyboardKey.KEY_END)
    {
      for (int i = rawPointer; i < content.Length; i++)
      {
        if (content[i] == '\n' || content[i] == '\0')
        {
          rawPointer = i;
          break;
        }
      }
    }
    if (key == (int)KeyboardKey.KEY_HOME)
    {
      for (int i = rawPointer; i >= 0; i--)
      {
        if (i == 0 || content[i-1] == '\n')
        {
          rawPointer = i;
          break;
        }
      }
    }
    // text renderer
    skipEditStage:
    Font font = note.font;
    int spacing = SettingsManager.profile.fontSpacing;
    int iy=0; // index x & y
    float x=0,y=0; // real x & y
    vec2 startPos = new vec2(rec.x+buffer, rec.y+buffer*5);
    float charW = 2;
    for (int i = 0; i < content.Length; i++)
    {
      y = startPos.y+iy*fontSize;
      vec2 pos = new vec2( startPos.x + x, y);
      if (content[i] == '\n')
      {
        iy++;
        x=0;
        charW = 2;
      }
      else if (content[i] == '\t')
      {
        unsafe
        {
          int index = Raylib.GetGlyphIndex(font, content[i]);
          GlyphInfo glyph = font.glyphs[index];

          x += (glyph.advanceX*((float)fontSize/font.baseSize) + spacing)*SettingsManager.profile.tabSize;
        }
      }
      else if (content[i] != '\0')
      {
        string col = env.theme.colors["text"];
        if (SettingsManager.profile.textMatchesBannerColor) col = note.bannerCol;
        Draw.Codepoint((int)content[i], font, pos, fontSize, col);
        
        unsafe
        {
          int index = Raylib.GetGlyphIndex(font, content[i]);
          GlyphInfo glyph = font.glyphs[index];

          x += glyph.advanceX*((float)fontSize/font.baseSize) + spacing;
        }
      }
      if (i == rawPointer)
      {
        float xPos = pos.x;
        if (xPos != startPos.x) xPos -= spacing;
        Draw.Rectangle(new rect(new vec2(xPos,pos.y),1,fontSize), env.theme.colors["accent"]);
      }
    }
    note = NoteManager.notes[selectedNoteIndex];
    note.pointer = rawPointer;
    NoteManager.notes[selectedNoteIndex] = note;
  }
}
/*
fgfg
fgf
*/