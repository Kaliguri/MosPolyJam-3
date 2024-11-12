using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphereManager : MonoBehaviour
{
    [Title("SphereSettings")]
    [SerializeField] GameObject spherePrefab;
    [SerializeField] GameObject perfectSpherePrefab;
    [SerializeField] public int sphereCount = 10;
    [SerializeField] int maxSphereCount = 10;
    [SerializeField] float maxOrbitRadius = 2f;
    [SerializeField] float orbitSpeed = 30f;
    [SerializeField] float moveToCenterSpeed = 5f;
    [SerializeField] float regularSpherePoints = 1; 
    [SerializeField] float perfectSpherePoints = 3;

    private List<GameObject> sphereList = new List<GameObject>();
    private List<GameObject> perfectSphereList = new List<GameObject>();
    private GameObject player;
    private float orbitRadius = 0f;

    public static PlayerSphereManager instance = null;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerTag>().gameObject;
        orbitRadius = maxOrbitRadius;

        for (int i = 0; i < maxSphereCount; i++)
        {
            GameObject sphere = Instantiate(spherePrefab, player.transform.position, Quaternion.identity);
            sphere.SetActive(false);
            sphereList.Add(sphere);

            GameObject perfectSphere = Instantiate(perfectSpherePrefab, player.transform.position, Quaternion.identity);
            perfectSphere.SetActive(false);
            perfectSphereList.Add(perfectSphere);
        }
    }

    [Button("ActivateSphere")]
    public void ActivateSphere(bool isPerfectParry)
    {
        int activeSphereCount = 0;

        foreach (var sphereList in new List<List<GameObject>> { sphereList, perfectSphereList })
        {
            foreach (GameObject sphere in sphereList)
            {
                if (sphere.activeSelf)
                    activeSphereCount++;
            }
        }

        if (activeSphereCount < sphereCount)
        {
            List<GameObject> targetList = isPerfectParry ? perfectSphereList : sphereList;

            foreach (GameObject sphere in targetList)
            {
                if (!sphere.activeSelf)
                {
                    sphere.transform.position = player.transform.position;
                    sphere.SetActive(true);
                    ArrangeSpheresInCircle();
                    break;
                }
            }
        }
    }

    [Button("PullSpheresToCenter")]
    public float PullSpheresToCenter()
    {
        float totalPoints = 0;
        foreach (var _sphereList in new List<List<GameObject>> { sphereList, perfectSphereList })
        {
            foreach (GameObject sphere in _sphereList)
            {
                if (sphere.activeSelf)
                {
                    if (sphere.GetComponent<PerfectSphereTag>() != null)
                    {
                        totalPoints += perfectSpherePoints;
                    }
                    else
                    {
                        totalPoints += regularSpherePoints;
                    }
                }
            }
        }

        StartCoroutine(MoveSpheresToCenterCoroutine());

        return totalPoints;
    }

    private IEnumerator MoveSpheresToCenterCoroutine()
    {
        bool allReachedCenter = false;

        while (!allReachedCenter)
        {
            allReachedCenter = true;

            foreach (var _sphereList in new List<List<GameObject>> { sphereList, perfectSphereList })
            {
                foreach (GameObject sphere in _sphereList)
                {
                    if (sphere.activeSelf)
                    {
                        if (Vector3.Distance(sphere.transform.position, player.transform.position) > 0.1f)
                        {
                            allReachedCenter = false;
                        }
                        else
                        {
                            sphere.SetActive(false);
                        }
                    }
                }
            }

            orbitRadius = Mathf.Max(0, orbitRadius - moveToCenterSpeed * Time.deltaTime);

            //Debug.Log(orbitRadius);

            yield return null;
        }

        orbitRadius = maxOrbitRadius;
    }

    private void ArrangeSpheresInCircle()
    {
        int activeSphereCount = 0;

        foreach (var sphereList in new List<List<GameObject>> { sphereList, perfectSphereList })
        {
            foreach (GameObject sphere in sphereList)
            {
                if (sphere.activeSelf)
                    activeSphereCount++;
            }
        }

        float angleStep = 360f / activeSphereCount;
        float angle = 0f;

        foreach (var sphereList in new List<List<GameObject>> { sphereList, perfectSphereList })
        {
            foreach (GameObject sphere in sphereList)
            {
                if (sphere.activeSelf)
                {
                    Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * orbitRadius;
                    sphere.transform.position = player.transform.position + offset;
                    angle += angleStep;
                }
            }
        }
    }

    void FixedUpdate()
    {
        int activeSphereCount = 0;

        foreach (var sphereList in new List<List<GameObject>> { sphereList, perfectSphereList })
        {
            foreach (GameObject sphere in sphereList)
            {
                if (sphere.activeSelf)
                    activeSphereCount++;
            }
        }

        float angleStep = 360f / activeSphereCount;
        float angle = Time.time * orbitSpeed;

        foreach (var sphereList in new List<List<GameObject>> { sphereList, perfectSphereList })
        {
            foreach (GameObject sphere in sphereList)
            {
                if (sphere.activeSelf)
                {
                    Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * orbitRadius;
                    sphere.transform.position = player.transform.position + offset;
                    angle += angleStep;
                }
            }
        }
    }
}
