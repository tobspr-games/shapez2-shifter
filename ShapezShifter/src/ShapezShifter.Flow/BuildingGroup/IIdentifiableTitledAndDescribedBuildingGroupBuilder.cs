using UnityEngine;

namespace ShapezShifter.Flow
{
    public interface IIdentifiableTitledAndDescribedBuildingGroupBuilder
    {
        IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Sprite icon);
    }
}
