using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildUnits : MonoBehaviour
{

    private KeyCode k1 = KeyCode.Alpha1;
    private KeyCode k2 = KeyCode.Alpha2;
    private KeyCode k3 = KeyCode.Alpha3;
    private KeyCode k4 = KeyCode.Alpha4;
    private KeyCode k5 = KeyCode.Alpha5;

    public UnitSpawnInfo spwn1;
    public UnitSpawnInfo spwn2;
    //[SerializeField]
    //private GameObject spwn4;
    //[SerializeField]
    //private GameObject spwn5;
    [SerializeField] private GameObject extractorBase;

    private MapState mapState;
    private UnitMap unitMap;
    private ResourcesMap resourcesMap;
    private PlayerRes playerRes;
    private UnitFactory unitFactory;

    /// <summary>
    /// Tmp hack
    /// </summary>
    [SerializeField]
    private GameObject projBase;

    private void Start()
    {
        EnforceSingleton();

        playerRes = GetComponent<PlayerRes>();
        if (playerRes == null)
        {
            Debug.LogError("PlayerRes not found");
        }

        mapState = FindObjectOfType<MapState>();
        Debug.Log("Mapstate is " + mapState);
        unitMap = mapState.unitMap;
        unitFactory = FindObjectOfType<UnitFactory>();
        resourcesMap = mapState.resourcesMap;

        // hardcoded for testing:
        spwn1 = new UnitSpawnInfo();
        spwn1.maxHealth = 50;
        spwn1.maxShield = 5;
        spwn1.attackDamage = 5;
        spwn1.attackCooldown = 1;
        spwn1.projectileBase = projBase;
        spwn1.range = 4;
        spwn1.teamToTarget = Team.black;
        spwn1.cost = 5;
        spwn1.team = Team.white;

        // hardcoded for testing:
        spwn2 = new UnitSpawnInfo();
        spwn2.maxHealth = 50;
        spwn2.maxShield = 5;
        spwn2.attackDamage = 5;
        spwn2.attackCooldown = 1;
        spwn2.projectileBase = projBase;
        spwn2.range = 4;
        spwn2.teamToTarget = Team.white;
        spwn2.cost = 5;
        spwn2.team = Team.black;
    }

    private void Update()
    {
        if (Input.GetKeyDown(k1) && CanAfford(spwn1)) // rework this expenditure system? is simple though.
        {
            if(spawnUnit(spwn1, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                playerRes.SpendMoney(spwn1.cost);
            }
        }

        if (Input.GetKeyDown(k2))
        {
            if (spawnUnit(spwn2, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
            }
        }

        if (Input.GetKeyDown(k3))
        {
            if (SpawnExtractor(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                playerRes.SpendMoney(25); // extractor cost

            }
        }

        if (Input.GetKeyDown(k4))
        {


            Vector2Int clickTile = VectorTools.GetClosestTileCoordinatesV2Int(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //if (resourcesMap.AddNode(clickTile, 500, ResourceType.Circle)) 
            //{

            //}
        }
    }

    private bool CanAfford(UnitSpawnInfo spwninfo)
    {
        if (playerRes.GetCurrentMoney() > spwninfo.cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool SpawnExtractor(Vector3 position)
    {

        Vector2Int spawnTarget = VectorTools.GetClosestTileCoordinatesV2Int(position);
        Debug.Log("spawning at " + spawnTarget);


        if (resourcesMap == null)
        {
            resourcesMap = mapState.resourcesMap;
        }
        if (resourcesMap == null)
        {
            Debug.LogError("resourcesMap not found");
        }

        Debug.Log("Node Detected: " + resourcesMap.IsNodeAt(spawnTarget));

        var toSpawn = Instantiate(extractorBase, position, Quaternion.identity);
        toSpawn.GetComponent<Unit>().team = Team.white;
        Debug.Log("spawned");
        return true;
    }


    private bool spawnUnit(UnitSpawnInfo spawnInfo, Vector3 position)
    {

        Vector2Int spawnTarget = VectorTools.GetClosestTileCoordinatesV2Int(position);
        Debug.Log("spawning at " + spawnTarget);

        if (unitMap == null)
        {
            unitMap = mapState.unitMap;
        }
        if (unitMap == null)
        {
            Debug.LogError("Could not find mapOccupiedInfo");
        }


        if (unitMap.IsUnitAt(spawnTarget))
        {
            Debug.Log("occupied");
            return false;
        }

        unitFactory.SpawnUnit(spawnInfo, true, position);

        return true;
    }

    private void EnforceSingleton()
    {
        var check = FindObjectOfType<PlayerBuildUnits>();
        if (check != this)
        {
            Debug.LogError("PlayerBuildUnits already exists");
            Destroy(this);
        }
    }

}
