struct Note
{
  public string title = "new note";
  public vec2 icon = new vec2(0,48);
  public string bannerCol = "808080";
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
      NoteManager.notes.Add(new Note(note[0], new vec2(Convert.ToInt32(note[1]),Convert.ToInt32(note[2])), note[4]){bannerCol = note[3]});
    }
  }
  public static void SaveNotes(string path)
  {
    string[] csv = new string[notes.Count];//File.ReadAllLines(path).Select(s => s.Split(",", 5)).ToArray();
    for (int i = 0; i < csv.Length; i++)
    {
      //note[4] = note[4].Replace("\\n", "\n");
      //NoteManager.notes.Add(new Note(note[0], new vec2(Convert.ToInt32(note[1]),Convert.ToInt32(note[2])), note[3]){bannerCol = note[3]});
      csv[i] =
        NoteManager.notes[i].title + "," +
        NoteManager.notes[i].icon.x + "," +
        NoteManager.notes[i].icon.y + "," +
        NoteManager.notes[i].bannerCol + "," +
        NoteManager.notes[i].content.Replace("\n","\\n");
    }
    File.WriteAllLines(path, csv);
  }
}