using UnityEngine;
using DogHouse.ToonWorld.CombatControllers;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_unitRootPrefab;

    [SerializeField]
    private GameUnitDefinition m_unitDefinition;

    [MethodButton("SpawnUnit", "ShowUI", "HideUI", "ShowHealth", "HideHealth", 
        "ShowXP", "HideXP", "AddXP", "RemoveHealth", "AddHealth")]
    [SerializeField]
    private bool editorFoldout;

    void SpawnUnit()
    {
        GameObject unit = Instantiate(m_unitRootPrefab);
        UnitRootController controller = unit.GetComponent<UnitRootController>();

        GameUnitDefinition clone = Instantiate(m_unitDefinition);
        controller.CreateUnit(clone);
    }

    void ShowUI()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.DisplayUnitIdentifiers(true);
    }

    void HideUI()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.DisplayUnitIdentifiers(false);
    }

    void ShowHealth()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.DisplayHealthBar(true);
    }

    void HideHealth()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.DisplayHealthBar(false);
    }

    void ShowXP()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.DisplayExperienceBar(true);
    }

    void HideXP()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.DisplayExperienceBar(false);
    }

    void AddXP()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.AddExperience(1);
    }

    void RemoveHealth()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.ApplyHealthChange(-1);
    }

    void AddHealth()
    {
        UnitRootController controller = FindObjectOfType<UnitRootController>();
        controller.ApplyHealthChange(1);
    }
}
