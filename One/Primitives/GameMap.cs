using System.Collections.Generic;
using System.IO;
using MonoGameClusterFuck.Layers;
using Newtonsoft.Json;

namespace MonogameClusterfuck_master.Primitives
{
    public class GameMap
    {
        public static Dictionary<LayerType, Layer> Layers = new Dictionary<LayerType, Layer>
        {
            [LayerType.Cursor] = new Layer(LayerType.Cursor),
            [LayerType.UI] = new Layer(LayerType.UI),
            [LayerType.L3] = new Layer(LayerType.L3),
            [LayerType.Entity] = new Layer(LayerType.Entity),
            [LayerType.GroundDecoration] = new Layer(LayerType.GroundDecoration),
            [LayerType.Ground] = new Layer(LayerType.Ground),
        };
        public void Load()
        {
            
        }

        public void Draw()
        {
            
        }
    }
}