using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "SpriteData", menuName = "ScriptableObjects/Data/SpriteData", order = 1)]
    public class SpriteData : ScriptableObject
    {
        [FormerlySerializedAs("Textures")] [SerializedDictionary("Element Type", "Texture")]
        public SerializedDictionary<SelectedElement, Sprite> Sprites;

        public Sprite GetSprite(SelectedElement selectedElement)
        {
            return Sprites[selectedElement];
        }
        
        public Texture GetTexture(SelectedElement selectedElement)
        {
            return Sprites[selectedElement].texture;
        }
    }
}
