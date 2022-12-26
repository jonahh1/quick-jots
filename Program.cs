static class Program{[STAThread]static void Main(){

// initialize window
Window.New(new vec2(800, 450), "quick-jots", 16, 24);

//Window.font = Raylib.LoadFont("C:/Windows/fonts/Arial.ttf");
//Raylib.SetTextureFilter(Window.font.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);
SettingsManager.LoadSettings();
NoteManager.LoadNotes();
Raylib.SetTargetFPS(30);
// updates each frame
while (!Raylib.WindowShouldClose() && !Window.shouldClose)
{
  Window.UpdateVariables();
  #region  update
  MyInput.keyPressedThisFrame = MyInput.GetKeyPressed();
  #endregion
  Window.BeginFrame(); // draws the menu bar & manages resizing as well
  #region draw
    // draw some text in the center of the screen
    //Draw.TextAnchored("Hello World!", new vec2(env.window.w/2,env.window.h/2), Anchor.middleCenter, 32, env.theme.colors["text"]);
    //NoteContentManager.DrawSelectedNote();
    //NoteContentManager.DrawSelectedNote();
    if (NoteManager.notes.Count > 0) NoteContent.DrawNoteContent();
    NoteList.DrawNoteList();
    WordIntellisense.DrawMenu();
    string title = Path.GetFileName(SettingsManager.profile.currentCollection);
    if (title == "") title = "*";
    title += " - quick-jots";
    Window.windowTitle = title;

    //Draw.RectangleShadow(new rect(env.window.wh/2 - 50, 100), "fff");
    //MyInput.TextBox(env.window.wh/2, ref I, ref p, 32);
    
  #endregion
  Window.EndFrame();
  MenuBarOptions.DrawMenuBarOptions();
  Raylib.EndDrawing();
  Utils.UpdateCursor(); // updates the mouse cursor image
  //I += (char)MyInput.keyPressedThisFrame;
}
//NoteManager.SaveNotes();
MenuBarOptions.Save();
SettingsManager.SaveSettings();
Window.Close(); // manages closing the window
}}