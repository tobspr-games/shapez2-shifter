using Game.Core.Rendering.MeshGeneration;

namespace ShapezShifter.Hijack
{
    public interface IBuildingsRewirer : IRewirer
    {
        GameBuildings ModifyGameBuildings(
            AuthoringBuildings metaBuildings,
            GameBuildings gameBuildings,
            IMeshCache meshCache,
            VisualThemeBaseResources theme);
    }
}