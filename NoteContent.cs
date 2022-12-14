class NoteContentManager
{
  public static int selectedNoteIndex = -1;
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

  }
}