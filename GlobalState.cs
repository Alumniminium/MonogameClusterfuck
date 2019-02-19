using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using System.Collections.Generic;

namespace MonoGameClusterFuck
{
    public static class GlobalState
    {
        public static GraphicsSettings GraphicsSettings = GraphicsSettings.Instance;
        public static Game Game{get;set;}
        public static bool DrawTileSet { get; set; }
        public static bool DisplayHelp { get; set; } = true;
        public static int Frames { get; internal set; }
        public static Dictionary<LayerType, Layer> Layers = new Dictionary<LayerType, Layer>
        {
            [LayerType.Cursor] = new Layer(LayerType.Cursor),
            [LayerType.UI] = new Layer(LayerType.UI),
            [LayerType.L3] = new Layer(LayerType.L3),
            [LayerType.Entity] = new Layer(LayerType.Entity),
            [LayerType.GroundDecoration] = new Layer(LayerType.GroundDecoration),
            [LayerType.Ground] = new Layer(LayerType.Ground),
        };
    }
}