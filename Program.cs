// initialize window
Window.New(new vec2(800, 450), "quick-jots", 16, 24);
NoteManager.LoadNotes("test.csv");
// updates each frame
while (!Raylib.WindowShouldClose() && !Window.shouldClose)
{
  Window.UpdateVariables();
  #region  update

  #endregion
  Window.BeginFrame(); // draws the menu bar & manages resizing as well
  #region draw
    // draw some text in the center of the screen
    Draw.TextAnchored("Hello World!", new vec2(env.window.w/2,env.window.h/2), Anchor.middleCenter, 32, env.theme.colors["text"]);
    NoteManager.DrawNoteList();
  #endregion
  Window.EndFrame();
  Utils.UpdateCursor(); // updates the mouse cursor image
}
Window.Close(); // manages closing the window

//Note: day one: ~2 hours