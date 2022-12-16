// initialize window
Window.New(new vec2(800, 450), "quick-jots", 16, 24);
NoteManager.LoadNotes("test.csv");
Raylib.SetTargetFPS(30);
// updates each frame
string I = "hello";
int p = 2;
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
    NoteContent.DrawNoteContent();
    NoteList.DrawNoteList();
    //MyInput.TextBox(env.window.wh/2, ref I, ref p, 32);
  #endregion
  Window.EndFrame();
  Utils.UpdateCursor(); // updates the mouse cursor image
  //I += (char)MyInput.keyPressedThisFrame;
}
NoteManager.SaveNotes("test.csv");
Window.Close(); // manages closing the window