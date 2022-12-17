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
  public static void DrawNoteContent()
  {
    float buffer = 24;
    rect rec = new rect(NoteList.rec.w, 0, env.window.w-NoteList.rec.w, env.window.h);
    
    Note note = NoteManager.notes[selectedNoteIndex];
    Raylib.DrawRectangleGradientV(
      (int)(rec.x), 
      (int)(Window.menuBarHeight+rec.y), 
      (int)(rec.w), 
      (int)(buffer*4),Utils.HexToRGB(note.bannerCol), Utils.HexToRGB(note.bannerCol+"00")
    );
    Draw.TextAnchored(note.title, rec.xy+buffer, Anchor.middleLeft, 32, env.theme.colors["background"]);

    //TODO: big, fully featured text editor, -_-
    // text editor
    
      
    // text renderer
    int textSize = 18;
    string content = note.content;
    float x=0,y=0;
    for (int i = 0; i < content.Length; i++)
    {
      if (content[i] == '\n')
      {
        x=0;
        y++;
      }
      else if (content[i] == '\t') x+=4;
      else
      {
        vec2 pos = new vec2(rec.x+buffer, rec.y+buffer*5);
        Draw.Codepoint((int)content[i], new vec2(pos.x+x*textSize/2, pos.y+y*textSize), textSize, "fff");
        x++;
      }

    }
  }
}