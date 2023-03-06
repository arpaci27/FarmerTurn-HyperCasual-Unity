using UnityEngine;

namespace AllIn1VfxToolkit.DemoAssets.TexturesDemo.Scripts
{
    [CreateAssetMenu(fileName = "All1VfxDemoTextureCollection", menuName = "AllIn1Vfx/DemoTextureCollection")]
    public class All1VfxDemoTextureCollection : ScriptableObject
    {
        public string collectionName;
        public Texture[] demoTextureCollection;
    }
}