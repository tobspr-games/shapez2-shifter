namespace ShapezShifter.Hijack
{
    public interface IIslandsRewirer : IRewirer
    {
        GameIslands ModifyGameIslands(
            IslandDefinitionFactory factory,
            AuthoringIslands metaIslands,
            GameIslands gameIslands);
    }
}
