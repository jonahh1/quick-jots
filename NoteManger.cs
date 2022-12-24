struct Note
{
  public string title = "new note";
  public Font font = Window.font;
  public string fontPath = "Arial";
  public vec2 icon = new vec2(0,48);
  public string bannerCol = "808080";
  public string content = "";
  public int pointer = 0;

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
  public void setPointer(int p) {pointer = p;}
  //public button select = new button();
}

class NoteManager
{
  public static List<Note> notes = new List<Note>();

  public static void LoadNotes()
  {
    if (!File.Exists(SettingsManager.profile.currentCollection)) return;//MenuBarOptions.SaveAs();
    string[][] csv = File.ReadAllLines(SettingsManager.profile.currentCollection).Select(s => s.Split(",", 7)).ToArray();
    notes = new List<Note>();
    foreach (string[] note in csv)
    {
      string title = note[0];
      string fontPath = note[1];
      vec2 icon = new vec2(Convert.ToInt32(note[2]),Convert.ToInt32(note[3]));
      string banner = note[4];
      int pointer = Convert.ToInt32(note[5]);
      string content = note[note.Length-1].Replace("\\n", "\n");
      Font font = LoadFont(ref fontPath);
      NoteManager.notes.Add(new Note(title, icon, content){bannerCol = banner, font = font, fontPath = fontPath, pointer = pointer});
    }
  }

  public static void newCollection()
  {
    //string[][] csv = File.ReadAllLines(path).Select(s => s.Split(",", 7)).ToArray();
    notes = new List<Note>();
    /*foreach (string[] note in csv)
    {
      string title = note[0];
      string fontPath = note[1];
      vec2 icon = new vec2(Convert.ToInt32(note[2]),Convert.ToInt32(note[3]));
      string banner = note[4];
      int pointer = Convert.ToInt32(note[5]);
      string content = note[note.Length-1].Replace("\\n", "\n");
      Font font = LoadFont(ref fontPath);
      NoteManager.notes.Add(new Note(title, icon, content){bannerCol = banner, font = font, fontPath = fontPath, pointer = pointer});
    }*/
  }
  public static void SaveNotes()
  {
    string[] csv = new string[notes.Count];//File.ReadAllLines(path).Select(s => s.Split(",", 5)).ToArray();
    for (int i = 0; i < csv.Length; i++)
    {
      //note[4] = note[4].Replace("\\n", "\n");
      //NoteManager.notes.Add(new Note(note[0], new vec2(Convert.ToInt32(note[1]),Convert.ToInt32(note[2])), note[3]){bannerCol = note[3]});
      csv[i] =
        NoteManager.notes[i].title + "," +
        NoteManager.notes[i].fontPath + "," +
        NoteManager.notes[i].icon.x + "," +
        NoteManager.notes[i].icon.y + "," +
        NoteManager.notes[i].bannerCol + "," +
        NoteManager.notes[i].pointer + "," +
        NoteManager.notes[i].content.Replace("\n","\\n");
    }
    //if (!File.Exists(SettingsManager.profile.currentCollection)) MenuBarOptions.SaveAs();
    if (SettingsManager.profile.currentCollection != "")File.WriteAllLines(SettingsManager.profile.currentCollection, csv);
  }
  public static Font LoadFont(ref string fontPath)
  {
    if (!fontPath.Contains(".ttf")) fontPath = "C:/Windows/fonts/"+fontPath+".ttf";
    Font font = Raylib.LoadFont(fontPath); Raylib.SetTextureFilter(font.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);
    return font;
    //
  }
}