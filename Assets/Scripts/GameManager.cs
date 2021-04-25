using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject SubmarineGO;
    public Camera camera;
    public Light mainLight;


    [Header("Skybox Depths")]
    public Material depth1SkyBoxMaterial;
    public Material depth2SkyBoxMaterial;
    public Material depth3SkyBoxMaterial;
    public Material depth4SkyBoxMaterial;
    public Material surfaceSkyBoxMaterial;

    public List<GameObject> SeaCreatures;

    public List<Transform> depth1SpawnPoints;
    public List<Transform> depth2SpawnPoints;
    public List<Transform> depth3SpawnPoints;
    public List<Transform> depth4SpawnPoints;

    public GameObject spawnPointsDepth1;
    public GameObject spawnPointsDepth2;
    public GameObject spawnPointsDepth3;
    public GameObject spawnPointsDepth4;
    public List<GameObject> spawnPointPrefab;


    public GameObject Bubbles;

    [Header("UI Elements")]
    public TextMeshProUGUI Depth;
    public TextMeshProUGUI Coords;
    public TextMeshProUGUI PictureQuality;
    public TextMeshProUGUI WarningMessage;

    public TextMeshProUGUI tmp;

    [Header("Camera UI")]
    public GameObject CameraRecGO;

    float spawnCoolDownTimer = 1f;

    private List<SeaCreatureBehaviorComponent> currentSeaCreatures;

    private SubControllerComponent submarine;

    private bool showWarning = false;
    private bool showWarningBlink = true;
    private float showWarningTimer = .25f;

    private bool cameraRecBlink = true;
    private float cameraRecTimer = .4f;

    private int spawnCount1;
    private int spawnCount2;
    private int spawnCount3;
    private int spawnCount4;

    // Start is called before the first frame update
    void Start()
    {
        currentSeaCreatures = new List<SeaCreatureBehaviorComponent>();

        depth1SpawnPoints = new List<Transform>();
        depth2SpawnPoints = new List<Transform>();
        depth3SpawnPoints = new List<Transform>();
        depth4SpawnPoints = new List<Transform>();

        submarine = SubmarineGO.GetComponent<SubControllerComponent>();

        GenerateSpawnpoints();

        WarningMessage.text = $"";
    }

    private void GenerateSpawnpoints()
    {
        for(int x=1; x< 20;x++)
        {
            for (int y = 1; y < 20; y++)
            {
                float topMaxSpawn = 1f;
                float bottomMaxSpawn = 40f;

                var go = Instantiate(spawnPointPrefab[0], new Vector3(x*10 - 100, Random.Range(topMaxSpawn, bottomMaxSpawn) * -1, ((y * 10) * -1) +100) , Quaternion.identity);

                go.transform.parent = spawnPointsDepth1.transform;

                depth1SpawnPoints.Add(go.transform);
            }
        }

        for (int x = 1; x < 15; x++)
        {
            for (int y = 1; y < 15; y++)
            {
                float topMaxSpawn = 50f;
                float bottomMaxSpawn = 90f;

                var go = Instantiate(spawnPointPrefab[1], new Vector3(x * 15 - 100, Random.Range(topMaxSpawn, bottomMaxSpawn) * -1, ((y * 15) * -1) + 100), Quaternion.identity);

                go.transform.parent = spawnPointsDepth2.transform;

                depth2SpawnPoints.Add(go.transform);
            }
        }

        for (int x = 1; x < 10; x++)
        {
            for (int y = 1; y < 10; y++)
            {
                float topMaxSpawn = 100f;
                float bottomMaxSpawn = 140f;

                var go = Instantiate(spawnPointPrefab[2], new Vector3(x * 20 - 100, Random.Range(topMaxSpawn, bottomMaxSpawn) * -1, ((y * 20) * -1) + 100), Quaternion.identity);

                go.transform.parent = spawnPointsDepth3.transform;

                depth3SpawnPoints.Add(go.transform);
            }
        }

        for (int x = 1; x < 5; x++)
        {
            for (int y = 1; y < 5; y++)
            {
                float topMaxSpawn = 150f;
                float bottomMaxSpawn = 190f;

                var go = Instantiate(spawnPointPrefab[3], new Vector3(x * 25 - 100 + 50, Random.Range(topMaxSpawn, bottomMaxSpawn) * -1, ((y * 25) * -1) + 100 - 50), Quaternion.identity);

                go.transform.parent = spawnPointsDepth4.transform;

                depth4SpawnPoints.Add(go.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        SpawnDepth1Creatures();
        SpawnDepth2Creatures();
        SpawnDepth3Creatures();
        SpawnDepth4Creatures();

        UpdateSkyBoxBasedOnDepth();

        UpdateSeaCreatures();

        UpdateUI();
    }

    private void SpawnDepth1Creatures()
    {
        if (spawnCount1 < 40)
        {
            spawnCoolDownTimer -= Time.deltaTime;

            if (spawnCoolDownTimer < 0)
            {
                spawnCount1++;

                spawnCoolDownTimer = 1f;

                var go = Instantiate(SeaCreatures[0], depth1SpawnPoints[Random.Range(0, depth1SpawnPoints.Count)]);

                go.GetComponent<SeaCreatureBehaviorComponent>().Waypoint = depth1SpawnPoints[Random.Range(0, depth1SpawnPoints.Count)].position;

                go.GetComponent<SeaCreatureBehaviorComponent>().Depth = 1;

                currentSeaCreatures.Add(go.GetComponent<SeaCreatureBehaviorComponent>());
            }
        }
    }

    private void SpawnDepth2Creatures()
    {
        if (spawnCount2 < 20)
        {
            spawnCoolDownTimer -= Time.deltaTime;

            if (spawnCoolDownTimer < 0)
            {
                spawnCount2++;

                spawnCoolDownTimer = 1f;

                var go = Instantiate(SeaCreatures[1], depth2SpawnPoints[Random.Range(0, depth2SpawnPoints.Count)]);

                go.GetComponent<SeaCreatureBehaviorComponent>().Waypoint = depth2SpawnPoints[Random.Range(0, depth2SpawnPoints.Count)].position;

                go.GetComponent<SeaCreatureBehaviorComponent>().Depth = 2;

                currentSeaCreatures.Add(go.GetComponent<SeaCreatureBehaviorComponent>());
            }
        }
    }

    private void SpawnDepth3Creatures()
    {
        if (spawnCount3 < 10)
        {
            spawnCoolDownTimer -= Time.deltaTime;

            if (spawnCoolDownTimer < 0)
            {
                spawnCount3++;

                spawnCoolDownTimer = 1f;

                var go = Instantiate(SeaCreatures[2], depth3SpawnPoints[Random.Range(0, depth3SpawnPoints.Count)]);

                go.GetComponent<SeaCreatureBehaviorComponent>().Waypoint = depth3SpawnPoints[Random.Range(0, depth3SpawnPoints.Count)].position;

                go.GetComponent<SeaCreatureBehaviorComponent>().Depth = 3;

                currentSeaCreatures.Add(go.GetComponent<SeaCreatureBehaviorComponent>());
            }
        }
    }

    private void SpawnDepth4Creatures()
    {
        if (spawnCount4 < 5)
        {
            spawnCoolDownTimer -= Time.deltaTime;

            if (spawnCoolDownTimer < 0)
            {
                spawnCount4++;

                spawnCoolDownTimer = 1f;

                var go = Instantiate(SeaCreatures[3], depth4SpawnPoints[Random.Range(0, depth4SpawnPoints.Count)]);

                go.GetComponent<SeaCreatureBehaviorComponent>().Waypoint = depth4SpawnPoints[Random.Range(0, depth4SpawnPoints.Count)].position;

                go.GetComponent<SeaCreatureBehaviorComponent>().Depth = 4;

                currentSeaCreatures.Add(go.GetComponent<SeaCreatureBehaviorComponent>());
            }
        }
    }

    private void UpdateUI()
    {
        if (!submarine.IsOnSurface())
        {
            Depth.text = $"Depth = {(SubmarineGO.transform.position.y * -18):0.##} meters";
            Coords.text = $"Coords = Lat:{submarine.Coords().x:0.##} Long:{submarine.Coords().y:0.##}";

            Bubbles.SetActive(true);
        }
        else
        {
            Depth.text = $"";
            Coords.text = $"";

            Bubbles.SetActive(false);
        }

        if (submarine.IsUsingCamera())
        {

            cameraRecTimer -= Time.deltaTime;

            if (cameraRecBlink)
            {
                CameraRecGO.SetActive(true);

                if (cameraRecTimer < 0)
                {
                    cameraRecBlink = false;
                    cameraRecTimer = .4f;
                }
            }

            if (!cameraRecBlink)
            {
                CameraRecGO.SetActive(false);

                if (cameraRecTimer < 0)
                {
                    cameraRecBlink = true;
                    cameraRecTimer = .4f;
                }
            }

            //tmp.text = submarine.CurrentFishAngleState().ToString();

            if (submarine.IsFishInFOV())
            {
               
                PictureQuality.text = $"Fish detected";

                if (submarine.CurrentFishFocusState() == FocusStates.Far)
                {
                    PictureQuality.text = $"Fish detected (Too far)";
                }
                if (submarine.CurrentFishFocusState() == FocusStates.Good)
                {
                    PictureQuality.text = $"Fish detected - Good range";
                }
                if (submarine.CurrentFishFocusState() == FocusStates.Close)
                {
                    PictureQuality.text = $"Fish detected (Too close)";
                }
            }
            else
            {
                PictureQuality.text = $"Nothing detected";
            }
        }
        else
        {
            PictureQuality.text = $"";

            CameraRecGO.SetActive(false);
        }

        if (showWarning)
        {
            cameraRecTimer -= Time.deltaTime;

            if (showWarningBlink)
            {
                WarningMessage.text = "Warning - Losing signal with Mother Ship";

                if (showWarningTimer < 0)
                {
                    showWarningBlink = false;
                    showWarningTimer = .25f;
                }
            }

            if (!showWarningBlink)
            {
                WarningMessage.text = "";

                if (showWarningTimer < 0)
                {
                    showWarningBlink = true;
                    showWarningTimer = .25f;
                }
            }
            
        }


        if (submarine.GetDistFromCenter() > 60)
        {
            showWarning = true;
        }

        if (submarine.GetDistFromCenter() < 60)
        {
            showWarning = false;
        }

        if (submarine.GetDistFromCenter() > 70)
        {
            // todo: some penalty for going out of bounds?? or relect current angle
            showWarning = false;
            WarningMessage.text = $"";
            submarine.Reset();
        }
    }

    private void UpdateSkyBoxBasedOnDepth()
    {
        if (SubmarineGO.transform.position.y < -1.59)
        {
            RenderSettings.skybox = depth1SkyBoxMaterial;

            // max depth is -198 so scale intensity from that

            var val = Mathf.Lerp(0, 200, (SubmarineGO.transform.position.y * -1));

            //RenderSettings.ambientIntensity = .001f;

            //mainLight.intensity = 0.01f;
        }

        if (SubmarineGO.transform.position.y < -50 && SubmarineGO.transform.position.y >  -100)
        {
            RenderSettings.skybox = depth2SkyBoxMaterial;
        }

        if (SubmarineGO.transform.position.y < -100 && SubmarineGO.transform.position.y > -150)
        {
            RenderSettings.skybox = depth3SkyBoxMaterial;
        }

        if (SubmarineGO.transform.position.y < -150 && SubmarineGO.transform.position.y > -200)
        {
            RenderSettings.skybox = depth3SkyBoxMaterial;
        }

        if (SubmarineGO.transform.position.y > -1.5)
        {
            RenderSettings.skybox = surfaceSkyBoxMaterial;

            // max depth is -198 so scale intensity from that

            var val = Mathf.Lerp(0, 200, (SubmarineGO.transform.position.y * -1));

            //RenderSettings.ambientIntensity = .001f;

            //mainLight.intensity = 0.01f;
        }
    }
    private void UpdateSeaCreatures()
    {
        foreach (SeaCreatureBehaviorComponent seaCreature in currentSeaCreatures)
        {
            if (seaCreature.transform.position == seaCreature.Waypoint)
            {
                if (seaCreature.Depth == 1)
                {
                    seaCreature.Waypoint = depth1SpawnPoints[Random.Range(0, depth1SpawnPoints.Count)].position;
                }
                if (seaCreature.Depth == 2)
                {
                    seaCreature.Waypoint = depth2SpawnPoints[Random.Range(0, depth2SpawnPoints.Count)].position;
                }
                if (seaCreature.Depth == 3)
                {
                    seaCreature.Waypoint = depth3SpawnPoints[Random.Range(0, depth3SpawnPoints.Count)].position;
                }
                if (seaCreature.Depth == 4)
                {
                    seaCreature.Waypoint = depth4SpawnPoints[Random.Range(0, depth4SpawnPoints.Count)].position;
                }

            }
        }
    }
}
