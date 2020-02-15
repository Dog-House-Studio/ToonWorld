using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using DogHouse.ToonWorld.Services;

public class GenerateMeshZone : MonoBehaviour
{
    [MethodButton("GenerateZone")]
    [SerializeField]
    private bool editorFoldout;

    private ServiceReference<IMeshZoneService> m_meshZoneService 
        = new ServiceReference<IMeshZoneService>();

    public void GenerateZone()
    {
        List<Vector3> zone = new List<Vector3>();
        zone.Add(Vector3.zero);

        m_meshZoneService.Reference.GenerateZone(zone.ToArray());
    }
}
