using System.Collections.Generic;
using MonoGameClusterFuck.Layers;

namespace MonoGameClusterFuck.Primitives
{
    public class GameMap
    {
        public static Dictionary<LayerType, Layer> Layers = new Dictionary<LayerType, Layer>
        {
            [LayerType.Ground] = new Layer(LayerType.Ground),
            [LayerType.GroundDecoration] = new Layer(LayerType.GroundDecoration),
            [LayerType.Entity] = new Layer(LayerType.Entity),
            [LayerType.L3] = new Layer(LayerType.L3),
            [LayerType.UI] = new Layer(LayerType.UI),
            [LayerType.Cursor] = new Layer(LayerType.Cursor),
        };

        public void Load()
        {
            
        }

        public void Draw()
        {
            
        }
    }
}