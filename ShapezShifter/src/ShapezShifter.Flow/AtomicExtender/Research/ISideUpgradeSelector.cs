using System.Collections.Generic;

namespace ShapezShifter.Flow.Atomic
{
    public interface ISideUpgradeSelector
    {
        public ResearchSideUpgrade Select(string scenarioId, IReadOnlyList<ResearchSideUpgrade> milestones);
    }
}