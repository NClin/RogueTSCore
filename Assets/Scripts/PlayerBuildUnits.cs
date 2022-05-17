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
    public UnitSpawnInfo spwn3;

    [SerializeField] private GameObject extractorBase;

    private MapState mapState;
    private UnitMap unitMap;
    private ResourcesMap resourcesMap;
    private PlayerRes playerRes;
    private UnitFactory unitFactory;
    private PlayerController playerController;

    /// <summary>
    /// Tmp hack
    /// </summary>
    [SerializeField]
    private GameObject projBase;
    [SerializeField]
    private Sprite unit1Sprite;
    [SerializeField]
    private Sprite unit2Sprite;
    [SerializeField]
    private Sprite unit3Sprite;

    private void Start()
    {
        EnforceSingleton();

        playerRes = GetComponent<PlayerRes>();
        playerController = FindObjectOfType<PlayerController>();
        mapState = FindObjectOfType<MapState>();
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
        spwn1.cost = 15;
        spwn1.dataCost = 0;
        spwn1.team = Team.white;
        spwn1.sprite = unit1Sprite;

        // hardcoded for testing:
        spwn2 = new UnitSpawnInfo();
        spwn2.maxHealth = 50;
        spwn2.maxShield = 5;
        spwn2.attackDamage = 5;
        spwn2.attackCooldown = 1;
        spwn2.projectileBase = projBase;
        spwn2.range = 4;
        spwn2.teamToTarget = Team.black;
        spwn2.cost = 5;
        spwn2.dataCost = 1;
        spwn2.team = Team.white;
        spwn2.sprite = unit2Sprite;

        // hardcoded for testing:
        spwn3 = new UnitSpawnInfo();
        spwn3.maxHealth = 50;
        spwn3.maxShield = 5;
        spwn3.attackDamage = 5;
        spwn3.attackCooldown = 1;
        spwn3.projectileBase = projBase;
        spwn3.range = 4;
        spwn3.teamToTarget = Team.black;
        spwn3.cost = 5;
        spwn3.dataCost = 1;
        spwn3.team = Team.white;
        spwn3.sprite = unit3Sprite;
    }

    private void Update()
    {

        if (playerController.GetDeployed()) { DoInput(); }
    }

    private void DoInput()
    {
        if (Input.GetKeyDown(k1) && CanAfford(spwn1)) // rework this expenditure system? is simple though.
        {
            Debug.Log("building ");

            if (SpawnUnit(spwn1, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                playerRes.SpendMass(spwn1.cost);
                playerRes.SpendData(spwn1.dataCost);
            }
        }

        if (Input.GetKeyDown(k2))
        {
            if (SpawnUnit(spwn2, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                playerRes.SpendMass(spwn2.cost);
                playerRes.SpendData(spwn2.dataCost);
            }
        }

        if (Input.GetKeyDown(k3))
        {

            if (SpawnUnit(spwn3, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                playerRes.SpendMass(spwn3.cost);
                playerRes.SpendData(spwn3.dataCost);
            }
        }

        if (Input.GetKeyDown(k4))
        {
            if (SpawnExtractor(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                playerRes.SpendMass(25); // extractor cost

            }

        }
    }

    private bool CanAfford(UnitSpawnInfo spwninfo)
    {
        if (playerRes.GetCurrentMass() >= spwninfo.cost
            && playerRes.GetCurrentData() >= spwninfo.dataCost)
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


    private bool SpawnUnit(UnitSpawnInfo spawnInfo, Vector3 position)
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

        // checks pathfinding that tile is walkable
        if (!AstarPath.active.GetNearest(position).node.Walkable)
        {
            return false;
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
