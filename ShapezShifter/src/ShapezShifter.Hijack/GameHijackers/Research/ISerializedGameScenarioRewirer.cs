namespace ShapezShifter.Hijack
{
    public interface ISerializedGameScenarioRewirer : IRewirer
    {
        SerializedGameScenario ModifySerializedGameScenario(SerializedGameScenario scenario);
    }
}
