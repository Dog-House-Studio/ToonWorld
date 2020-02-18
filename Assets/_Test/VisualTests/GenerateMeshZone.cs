using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using DogHouse.ToonWorld.Services;

public class GenerateMeshZone : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_tiles;

    [MethodButton("GenerateZone")]
    [SerializeField]
    private bool editorFoldout;

    private ServiceReference<IMeshZoneService> m_meshZoneService 
        = new ServiceReference<IMeshZoneService>();

    public void GenerateZone()
    {
        List<Vector3> zone = new List<Vector3>();
        foreach(GameObject obj in m_tiles)
        {
            zone.Add(obj.transform.position);
        }

        m_meshZoneService.Reference.GenerateZone(zone.ToArray());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) GenerateZone();
    }
}
