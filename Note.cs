struct Note
{
  public string title = "new note";
  public vec2 icon = new vec2(48,24);

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
}

class NoteManager
{
  public static List<Note> notes = new List<Note>();

  public static button newNote = new button();
  public static button deleteMode = new button(); 
  public static button editMode = new button();

  public static button binButton = new button();
  public static float deleteModeTimerB = 0;

  public static bool inDeleteMode = false;



  public static void LoadNotes(string path)
  {
    string[][] csv = File.ReadAllLines(path).Select(s => s.Split(",", 4)).ToArray();
    notes = new List<Note>();
    foreach (string[] note in csv)
    {
      note[3] = note[3].Replace("\\n", "\n");
      notes.Add(new Note(note[0], new vec2(Convert.ToInt32(note[1]),Convert.ToInt32(note[2])), note[3]));
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
  }

  public static void DrawNoteList()
  {
    rect rec = new rect(0,0,env.window.w*.35f, env.window.h);
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
      Draw.RectangleCurved(icon, new rect(curvature,0,curvature,0), "333");
      Draw.RectangleCurved(text, new rect(0,curvature,0,curvature), "333");
      
      Draw.Icon(notes[i].icon, icon.xy, 0, "fff");
      Draw.TextAnchored(notes[i].title, new vec2(text.x + curvature, text.y + text.h/2), Anchor.middleLeft, 16, "fff");
      
      if (inDeleteMode)
      {
        vec2 binPos = text.xy+new vec2(text.w,0) + new vec2(Utils.Lerp(20, 0, deleteModeTimerB),0);
        Draw.Icon(new vec2(120, 48), binPos, 0, Utils.LerpCol("d130", "d13", deleteModeTimerB));
        binButton.rec = new rect(binPos-2,14);

        if (Button.AnimatedAction(binButton, "fff0", "fffa").state) notes.RemoveAt(i);
      }
      pos.y += buffer*1.5f;
    }
    newNote.rec = new rect((rec.w-buffer)/2 - buffer*1.5f, pos.y, buffer);
    deleteMode.rec = new rect((rec.w-buffer)/2, pos.y, buffer);
    editMode.rec = new rect((rec.w-buffer)/2 + buffer*1.5f, pos.y, buffer);
    
    bool createNewNote = iconButton(newNote, new vec2(96, 0), false);
    inDeleteMode = iconButton(deleteMode, new vec2(120, 0));
    bool enterEditMode = iconButton(editMode, new vec2(120, 24));
    if (createNewNote) notes.Add(new Note("hello", new vec2(Convert.ToInt32(0),Convert.ToInt32(24)), "gurk"));
    // if (deleteMode.timer == 1 && deleteMode.currentState.state) TimerBIsAlways1 = true;
    // else if (!deleteMode.currentState.state) TimerBIsAlways1 = false;
    //deleteModeTimerB = TimerBIsAlways1?1:deleteMode.timer;
    if (deleteMode.currentState.state) deleteModeTimerB = Math.Min(deleteModeTimerB+Raylib.GetFrameTime() * deleteMode.speed, 1);
    else deleteModeTimerB = Math.Max(deleteModeTimerB-Raylib.GetFrameTime() * deleteMode.speed, 0);
    Console.WriteLine(deleteModeTimerB);
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