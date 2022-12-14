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

  public button select = new button();
}

class NoteListManager
{
  public static rect rec = rect.zero;

  public static List<Note> notes = new List<Note>();

  public static button newNote = new button();
  public static button deleteMode = new button(); 
  public static button editMode = new button();

  public static button editButton = new button();
  public static float deleteButtonTimer = 0;

  public static button EditButton = new button();
  public static float editButtonTimer = 0;


  public static bool inDeleteMode = false;
  public static bool lastInDeleteMode = false;

  public static bool inEditMode = false;
  public static bool lastInEditMode = false;

  public static bool openIconSelectBox = false;
  public static bool openAlterTextBox = false;

  public static void LoadNotes(string path)
  {
    string[][] csv = File.ReadAllLines(path).Select(s => s.Split(",", 5)).ToArray();
    notes = new List<Note>();
    foreach (string[] note in csv)
    {
      note[4] = note[4].Replace("\\n", "\n");
      notes.Add(new Note(note[0], new vec2(Convert.ToInt32(note[1]),Convert.ToInt32(note[2])), note[3]){bannerCol = note[3]});
      
      //notes.Last().bannerCol. = note[3];
      notes.Last().select.changeRect = true;
      notes.Last().select.max = 10;
      notes.Last().select.rectRule = (r,t) => new rect(r.x+t, r.y, r.w,r.h);
    }
    newNote.changeRect = true;
    newNote.max = 2;
    newNote.rectRule = (r,t) => new rect(r.x-t/2, r.y-t/2, r.w+t);

    deleteMode.changeRect = true;
    deleteMode.max = 2;
    deleteMode.rectRule = (r,t) => new rect(r.x-t/2, r.y-t/2, r.w+t);

    editMode.changeRect = true;
    editMode.max = 2;
    editMode.rectRule = (r,t) => new rect(r.x-t/2, r.y-t/2, r.w+t);

    selectIconBtn.changeRect = true;
    selectIconBtn.max = 20;
    selectIconBtn.rectRule = (r,t) => r;
    
    closeIconSelectBoxBtn.changeRect = true;
    closeIconSelectBoxBtn.max = 20;
    closeIconSelectBoxBtn.rectRule = (r,t) => r;

    acceptAlterTextBoxBtn.changeRect = true;
    acceptAlterTextBoxBtn.max = 20;
    acceptAlterTextBoxBtn.rectRule = (r,t) => r;
  }
 // public static 
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

      notes[i].select.rec = new rect(text.x+text.w, text.y, buffer);
      
      if (Button.AnimatedActionIcon(notes[i].select, new vec2(48,24), Anchor.middleCenter, env.theme.colors["note list bg"],env.theme.colors["note list bg"]).state)
        NoteContentManager.selectedNoteIndex = i;

      if (deleteButtonTimer != 0)
      {
        string col = "fff";
        editButton.rec = new rect(icon.x, icon.y, rec.w-icon.x*2 + buffer/2, icon.h);
        vec2 binPos = text.xy+new vec2(text.w,0) + new vec2(Utils.Lerp(20, 0, deleteButtonTimer),0);
        if (Utils.MouseOnRec(editButton.rec)) col = "d13";
        Draw.Icon(new vec2(120, 48), binPos, 0, Utils.LerpCol(col+"0", col, deleteButtonTimer));

        //new rect(editIconPos-2,14);
        if (Button.AnimatedAction(editButton, "0000", "0000").state) notes.RemoveAt(i);
        //Draw.Icon(new vec2(120, 48), binPos, 0, Utils.LerpCol("d130", "d13", deleteButtonTimer));

        
      }
      if (editButtonTimer != 0)
      {
        vec2 lerp = new vec2(Utils.Lerp(15, -5, editButtonTimer),0);
        
        // icon
        string col = "fff";
        icon.w += buffer/3;
        vec2 editIconPos = icon.xy+new vec2(buffer,0)/*text.xy-new vec2(buffer/2,0)*/ + lerp;
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

      }
      pos.y += buffer*1.5f;
    }
    newNote.rec = new rect((rec.w-buffer)/2 - buffer*1.5f, pos.y, buffer);
    deleteMode.rec = new rect((rec.w-buffer)/2, pos.y, buffer);
    editMode.rec = new rect((rec.w-buffer)/2 + buffer*1.5f, pos.y, buffer);
    
    bool createNewNote = iconButton(newNote, new vec2(96, 0), false);
    inDeleteMode = iconButton(deleteMode, new vec2(120, 0));
    inEditMode = iconButton(editMode, new vec2(120, 24));
    if (createNewNote) notes.Add(new Note("hello", new vec2(Convert.ToInt32(0),Convert.ToInt32(24)), "gurk"));

    if (inDeleteMode && !lastInDeleteMode) editMode.currentState.state = false;
    else if (inEditMode && !lastInEditMode) deleteMode.currentState.state = false;

    if (deleteMode.currentState.state) deleteButtonTimer = Math.Min(deleteButtonTimer+Raylib.GetFrameTime() * deleteMode.speed, 1);
    else deleteButtonTimer = Math.Max(deleteButtonTimer-Raylib.GetFrameTime() * deleteMode.speed, 0);

    if (editMode.currentState.state) editButtonTimer = Math.Min(editButtonTimer+Raylib.GetFrameTime() * editMode.speed, 1);
    else editButtonTimer = Math.Max(editButtonTimer-Raylib.GetFrameTime() * editMode.speed, 0);
    
    lastInDeleteMode = inDeleteMode;
    lastInEditMode = inEditMode;

    if (openIconSelectBox) iconSelectBox();
    if (openAlterTextBox) alterTextBox();
  }

  public static button closeIconSelectBoxBtn = new button();
  public static button selectIconBtn = new button();
  public static int NoteIndexToChange = 0;
  public static void iconSelectBox()
  {
    float infoHeight = 18;
    vec2 newIcon = notes[NoteIndexToChange].icon;
    vec2 size = new vec2(Window.iconSheet.width, Window.iconSheet.height+infoHeight);
    rect rec = new rect(env.window.wh/2 - size/2, size.x, size.y);
    closeIconSelectBoxBtn.rec = new rect(rec.x+rec.w-infoHeight,rec.y,infoHeight);
    Draw.Rectangle(rec, "333");
    openIconSelectBox = !Button.AnimatedActionIcon(closeIconSelectBoxBtn, Icons.close, Anchor.middleCenter, "d130","d134").state;
    Draw.RectangleOutline(rec, 1, false, "07c");
    Draw.Line(new vec2(rec.x,rec.y+infoHeight), new vec2(rec.x+rec.w,rec.y+infoHeight), 1, "07c");
    Draw.Texture(Window.iconSheet, new rect(0,0,size.x,size.y-infoHeight), new rect(rec.x, rec.y+infoHeight, rec.w, rec.h-infoHeight), 0, "fff");
    Draw.TextAnchored("select a new icon", rec.xy+infoHeight/2, Anchor.middleLeft, 16, "fff");
    
    for (int y = 0; y < Window.iconSheet.height; y+=24)
    {
      for (int x = 0; x < Window.iconSheet.width; x+=24)
      {
        selectIconBtn.rec = new rect(new vec2(rec.x+x,rec.y+y+infoHeight), 24,24);
        if (Button.AnimatedAction(selectIconBtn, "fff0","fffa").state)
        {
          Note newNote = notes[NoteIndexToChange];
          newNote.icon = new vec2(x,y);
          notes[NoteIndexToChange] = newNote;
          openIconSelectBox = false;
        }
      }
    }
  }
  public static int pointer = -2;
  public static button cancelAlterTextBoxBtn = new button();
  public static button acceptAlterTextBoxBtn = new button();
  public static string newTitle = "";
  public static void alterTextBox()
  {
    //float infoHeight = 18;
    vec2 size = new vec2(232, 32);
    rect rec = new rect(env.window.wh/2 - size/2, size.x, size.y);
    Draw.Rectangle(rec, "333");
    
    int key = Raylib.GetCharPressed();
    // Check if more characters have been pressed on the same frame
    while (key > 0)
    {
      // NOTE: Only allow keys in range [32..125]
      if ((key >= 32) && (key <= 125) && (newTitle.Length < 26))
      {
        //newTitle += (char)key;
        newTitle = newTitle.Insert(pointer+1, ""+(char)key);
        pointer++;
      }

      key = Raylib.GetCharPressed();  // Check next character in the queue
    }

    if (MyInput.keyPressedThisFrame == (int)KeyboardKey.KEY_BACKSPACE && newTitle.Length > 0)
    {
      newTitle = newTitle.Remove(pointer,1);
      pointer--;
    }
    if (MyInput.keyPressedThisFrame == (int)KeyboardKey.KEY_LEFT && pointer > -1) pointer--;
    if (MyInput.keyPressedThisFrame == (int)KeyboardKey.KEY_RIGHT && pointer < newTitle.Length-1) pointer++;
    if (MyInput.keyPressedThisFrame == (int)KeyboardKey.KEY_UP) pointer=newTitle.Length-1;
    if (MyInput.keyPressedThisFrame == (int)KeyboardKey.KEY_DOWN) pointer=-1;

    Draw.TextAnchored(newTitle, new vec2(rec.x+5, rec.y+rec.h/2), Anchor.middleLeft, 16, "fff");
    vec2 pointerPos = Utils.TextSize(Window.font, new string(newTitle.AsSpan(0, pointer+1)), 16);

    Draw.Line(new vec2(rec.x +5+ pointerPos.x, rec.y + (rec.h-18)/2), new vec2(rec.x +5+ pointerPos.x, rec.y + 25), 1, "07c");

    acceptAlterTextBoxBtn.max = 20;
    cancelAlterTextBoxBtn.max = 20;

    acceptAlterTextBoxBtn.rec = new rect(rec.x+rec.w-64,rec.y,32);
    bool accept = Button.AnimatedActionIcon(acceptAlterTextBoxBtn, new vec2(72,24), Anchor.middleCenter, "fff0", "fff3").state;
    cancelAlterTextBoxBtn.rec = new rect(rec.x+rec.w-32,rec.y,32);
    bool cancel = Button.AnimatedActionIcon(cancelAlterTextBoxBtn, Icons.close, Anchor.middleCenter, "d130", "d134").state;
    Draw.RectangleOutline(rec, 1, false, "07c");

    if (accept)
    {
      Note newNote = notes[NoteIndexToChange];
      newNote.title = newTitle;
      notes[NoteIndexToChange] = newNote;
    }
    if (cancel || accept) openAlterTextBox = false;
    /*btn.rec = new rect(rec.x+rec.w-64,rec.y,32);
    bool accept = Button.AnimatedActionIcon(btn, new vec2(72, 24), Anchor.middleCenter, "fff0", "fff8").state;*/

    /*
    if (MyInput.keyPressedThisFrame != 0)
    {
      Note newNote = notes[NoteIndexToChange];
      newNote.title += (char)(MyInput.keyPressedThisFrame+32);
      notes[NoteIndexToChange] = newNote;


    Note newNote = notes[NoteIndexToChange];
    newNote.title = newTitle;
    notes[NoteIndexToChange] = newNote;

    }*/

    /*vec2 newIcon = notes[NoteIndexToChangeIcon].icon;
    vec2 size = new vec2(Window.iconSheet.width, Window.iconSheet.height+infoHeight);
    rect rec = new rect(env.window.wh/2 - size/2, size.x, size.y);
    closeIconSelectBoxBtn.rec = new rect(rec.x+rec.w-infoHeight,rec.y,infoHeight);
    Draw.Rectangle(rec, "333");
    openIconSelectBox = !Button.AnimatedActionIcon(closeIconSelectBoxBtn, Icons.close, Anchor.middleCenter, "d130","d134").state;
    Draw.RectangleOutline(rec, 1, false, "07c");
    Draw.Line(new vec2(rec.x,rec.y+infoHeight), new vec2(rec.x+rec.w,rec.y+infoHeight), 1, "07c");
    Draw.Texture(Window.iconSheet, new rect(0,0,size.x,size.y-infoHeight), new rect(rec.x, rec.y+infoHeight, rec.w, rec.h-infoHeight), 0, "fff");
    Draw.TextAnchored("select a new icon", rec.xy+infoHeight/2, Anchor.middleLeft, 16, "fff");
    
    for (int y = 0; y < Window.iconSheet.height; y+=24)
    {
      for (int x = 0; x < Window.iconSheet.width; x+=24)
      {
        selectIconBtn.rec = new rect(new vec2(rec.x+x,rec.y+y+infoHeight), 24,24);
        if (Button.AnimatedAction(selectIconBtn, "fff0","fffa").state)
        {
          Note newNote = notes[NoteIndexToChangeIcon];
          newNote.icon = new vec2(x,y);
          notes[NoteIndexToChangeIcon] = newNote;
          openIconSelectBox = false;
        }
      }
    }*/
  }

  static bool iconButton(button b, vec2 icon, bool toggle = true)
  {
    //editMode.rec = new rect((rec.w-buffer)/2 + buffer*1.5f, pos.y, buffer);

    bool state = false;
    if (toggle) state = Button.AnimatedToggle(b, "fff0", "fff0", "fff0").state;
    else state = Button.AnimatedAction(b, "fff0", "fff0").state;

    string iconCol = Utils.LerpCol("fff", "07c", b.timer);
    if (state) iconCol = "07c";
    Draw.Icon(icon, b.rec.xy, 0, iconCol);
    return state;
  }
}
