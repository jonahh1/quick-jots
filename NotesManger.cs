struct Note
{
  public string title = "new note";
  public vec2 icon = new vec2(48,24);
  public string bannerCol = "568c6a";
  public string content = "";

  public Note(){}
  public Note(string title, vec2 icon)
  {
    this.title = title;
    this.icon = icon;
  }
  public Note(string title, vec2 icon, string content)
  {
    this.title = title;
    this.icon = icon;
    this.content = content;
  }

  //public button select = new button();
}

class NoteManager
{
  public static List<Note> notes = new List<Note>();

  public static void LoadNotes(string path)
  {
    string[][] csv = File.ReadAllLines(path).Select(s => s.Split(",", 5)).ToArray();
    notes = new List<Note>();
    foreach (string[] note in csv)
    {
      note[4] = note[4].Replace("\\n", "\n");
      notes.Add(new Note(note[0], new vec2(Convert.ToInt32(note[1]),Convert.ToInt32(note[2])), note[3]){bannerCol = note[3]});
      
      //notes.Last().bannerCol. = note[3];
      /*notes.Last().select.changeRect = true;
      notes.Last().select.max = 10;
      notes.Last().select.rectRule = (r,t) => new rect(r.x+t, r.y, r.w,r.h);*/
    }
  }
  public static rect rec = rect.zero;
  public static void DrawNoteList()
  {
    rec = new rect(0,0,env.window.w*.35f, env.window.h);
    Draw.Rectangle(rec, env.theme.colors["note list bg"]);
    float buffer = 24;
    vec2 pos = rec.xy+buffer;
    vec2 size = new vec2(rec.w-buffer*3.5f, buffer);
    //foreach (Note note in notes)
    for (int i = 0; i < notes.Count; i++)
    {
      rect icon = new rect(pos.x, pos.y, buffer);
      rect text = new rect(pos.x + buffer*1.5f, pos.y, size.x, size.y);
      float curvature = buffer/4;
      Draw.RectangleCurved(icon, new rect(curvature,0,curvature,0), notes[i].bannerCol);
      Draw.RectangleCurved(text, new rect(0,curvature,0,curvature), notes[i].bannerCol);
      
      Draw.Icon(notes[i].icon, icon.xy, 0, "fff");
      Draw.TextAnchored(notes[i].title, new vec2(text.x + curvature, text.y + text.h/2), Anchor.middleLeft, 16, "fff");
      rect totalRec = new rect(0, icon.y, rec.w, icon.h);
      //notes[i].select.rec = new rect(text.x+text.w, text.y, buffer);
      if (Utils.MouseOnRec(totalRec) && Input.IsRightMouse(ClickMode.pressed))
      {
        selectedNoteIndex = i;
        menuPos = env.mousePos;
        menuTimer = 0;
      }
      pos.y += buffer*1.5f;
      
      
      #region ahhhhh
      //if (Button.AnimatedActionIcon(notes[i].select, new vec2(48,24), Anchor.middleCenter, env.theme.colors["note list bg"],env.theme.colors["note list bg"]).state)
        //NoteContentManager.selectedNoteIndex = i;

      /*if (deleteButtonTimer != 0)
      {
        string col = "fff";
        editButton.rec = new rect(icon.x, icon.y, rec.w-icon.x*2 + buffer/2, icon.h);
        vec2 binPos = text.xy+new vec2(text.w,0) + new vec2(Utils.Lerp(20, 0, deleteButtonTimer),0);
        if (Utils.MouseOnRec(editButton.rec)) col = "d13";
        Draw.Icon(new vec2(120, 48), binPos, 0, Utils.LerpCol(col+"0", col, deleteButtonTimer));

        //new rect(editIconPos-2,14);
        if (Button.AnimatedAction(editButton, "0000", "0000").state) notes.RemoveAt(i);
        //Draw.Icon(new vec2(120, 48), binPos, 0, Utils.LerpCol("d130", "d13", deleteButtonTimer));

        
      }*/
      /*
      if (editButtonTimer != 0)
      {
        vec2 lerp = new vec2(Utils.Lerp(15, -5, editButtonTimer),0);
        
        // icon
        string col = "fff";
        icon.w += buffer/3;
        vec2 editIconPos = icon.xy+new vec2(buffer,0)/*text.xy-new vec2(buffer/2,0) + lerp;
        if (Utils.MouseOnRec(icon)) col = "07c";
        Draw.Icon(new vec2(96, 48), editIconPos, 0, Utils.LerpCol(col+"0", col, editButtonTimer));
        editButton.rec = icon;//new rect(editIconPos-2,14);
        if (Button.AnimatedAction(editButton, "0000", "0000").state)
        {
          openIconSelectBox = true;
          NoteIndexToChange = i;
        }

        // text
        col = "fff";
        text.w += buffer/3;
        vec2 editTextPos = text.xy+new vec2(text.w-buffer/3,0) + lerp;
        if (Utils.MouseOnRec(text)) col = "07c";
        Draw.Icon(new vec2(96, 48), editTextPos, 0, Utils.LerpCol(col+"0", col, editButtonTimer));
        editButton.rec = text;
        if (Button.AnimatedAction(editButton, "0000", "0000").state)
        {
          openAlterTextBox = true;
          NoteIndexToChange = i;
          newTitle = notes[NoteIndexToChange].title;
          pointer = newTitle.Length-1;
        }

      }*/
      #endregion
    }
    RightClickMenu();
  }
  public static int selectedNoteIndex = -1;
  public static vec2 menuPos = vec2.zero;
  public static float menuTimer = 0;
  public static button editBtn = new button() {rectRule = (r,t) => r;}
  public static void RightClickMenu()
  {
    if (selectedNoteIndex == -1)
    {
      menuTimer = Math.Max(menuTimer-Raylib.GetFrameTime() * 5, 0);
      return;
    }
    else menuTimer = Math.Min(menuTimer+Raylib.GetFrameTime() * 5, 1);

    rect rec = new rect(menuPos.x,menuPos.y, 150,100);
    Draw.RectangleShadow(rec, Utils.LerpCol("0000","0008",menuTimer));
    Draw.Rectangle(rec,Utils.LerpCol(env.theme.colors["input menus"]+"00",env.theme.colors["input menus"],menuTimer));
    Draw.RectangleOutline(rec, 1, false, env.theme.colors["accent"]);
    
    

    if (!Utils.MouseOnRec(rec) && Input.IsLeftMouse(ClickMode.pressed)) selectedNoteIndex = -1;
    
  }
}