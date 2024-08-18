using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "TextureData", menuName = "ScriptableObjects/Data/TextureData", order = 1)]
    public class TextureData : ScriptableObject
    {
        [SerializedDictionary("Element Type", "Texture")]
        public SerializedDictionary<SelectedElement, Texture> Textures;

        public Texture GetTexture(SelectedElement selectedTexture)
        {
            return Textures[selectedTexture];
        }
    }
}
