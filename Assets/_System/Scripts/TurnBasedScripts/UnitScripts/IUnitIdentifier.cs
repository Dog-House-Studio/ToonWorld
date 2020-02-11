using DogHouse.ToonWorld.Animation;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// IUnitIdentifier is an interface that when
    /// implemented, is implemented by something that
    /// identifies a unit's information. When this happens,
    /// the concrete implementation must set the given
    /// information about a particular unit.
    /// </summary>
    public interface IUnitIdentifier : IFade
    {
        void SetDataDisplay(GameUnitDefinition definition);
    }
}
