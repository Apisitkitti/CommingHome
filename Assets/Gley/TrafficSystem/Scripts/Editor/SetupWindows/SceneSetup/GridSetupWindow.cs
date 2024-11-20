using Gley.UrbanAssets.Editor;

namespace Gley.TrafficSystem.Editor
{
    public class GridSetupWindow : GridSetupWindowBase
    {
        public override void DrawInScene()
        {
            if (viewGrid)
            {
                DrawGrid.Draw(grid.grid);
            }
            base.DrawInScene();
        }
    }
}
