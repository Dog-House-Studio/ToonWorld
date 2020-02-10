using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// IUnitIdentifier is an interface that when
    /// implemented, is implemented by something that
    /// identifies a unit's information. When this happens,
    /// the concrete implementation must set the given
    /// information about a particular unit.
    /// </summary>
    public interface IUnitIdentifier
    {
        void SetName(string name);
        void SetClassName(string className);
        void SetClassEmblem(Sprite sprite);
        void SetUnitLevel(int level);
    }
}
