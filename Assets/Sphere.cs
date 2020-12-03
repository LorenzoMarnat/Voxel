using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public GameObject cubeGO;

    public float cubeSize = 1f;

    public float potential = 10;

    private int cubesX;

    private int cubesY;

    private int cubesZ;

    private Vector3 origine;

    private List<Vector3> spheres;
    private List<float> diametreSpheres;

    public List<GameObject> listCubes;
    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<Vector3>();
        diametreSpheres = new List<float>();
        listCubes = new List<GameObject>();

        spheres.Add(new Vector3(1, 1, 1));
        diametreSpheres.Add(1);

        spheres.Add(new Vector3(2.5f, 1, 1));
        diametreSpheres.Add(1);

        spheres.Add(new Vector3(1, 1, 2.5f));
        diametreSpheres.Add(1);

        while (cubeSize > diametreSpheres.Min() / 2f)
            cubeSize /= 2;

        SetBox();

        //Union();
        //Intersection();

        //SetOctreeUnion();
        //SetOctreeIntersection();

        Grid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<GameObject> cubes = listCubes;
            listCubes = new List<GameObject>();
            OctreeUnion(cubes);
            //OctreeIntersection(cubes);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                GameObject go = hit.collider.gameObject;

                if (go.tag == "Cube")
                {
                    Destroy(go);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                GameObject go = hit.collider.gameObject;

                if (go.tag == "Cube")
                {
                    Vector3 incomingVec = hit.normal - Vector3.up;
                    Vector3 offset = Vector3.zero;
                    float gocubeSize = (go.GetComponent<Cube>().cubeSize / 2) + (cubeSize / 2);

                    if (incomingVec == new Vector3(0, -1, -1))
                        offset = new Vector3(0, 0, gocubeSize);

                    if (incomingVec == new Vector3(0, -1, 1))
                        offset = new Vector3(0, 0, -gocubeSize);

                    if (incomingVec == new Vector3(0, 0, 0))
                        offset = new Vector3(0, -gocubeSize, 0);

                    if (incomingVec == new Vector3(1, 1, 1))
                        offset = new Vector3(0, gocubeSize, 0);

                    if (incomingVec == new Vector3(-1, -1, 0))
                        offset = new Vector3(gocubeSize, 0, 0);

                    if (incomingVec == new Vector3(1, -1, 0))
                        offset = new Vector3(-gocubeSize, 0, 0);

                    GameObject c = Instantiate(cubeGO, go.transform.position - offset, Quaternion.identity);
                    c.GetComponent<Cube>().cubeSize = cubeSize;
                    c.GetComponent<Cube>().potential = Potential(go.transform.position);
                    c.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                    listCubes.Add(c);
                }
            }
        }
    }

    private float Potential(Vector3 cube)
    {
        float potential = 1;

        for (int i = 0; i < spheres.Count; i++)
        {
            float distance = Vector3.Distance(cube, spheres[i]);
            float rayon = diametreSpheres[i];// * 2f;
            if (distance < rayon)
                potential += ((rayon - distance) / rayon); //* potential;
        }
        return potential;
    }

    private void Grid()
    {
        for (int i = 0; i < cubesX; i++)
        {
            for (int j = 0; j < cubesY; j++)
            {
                for (int k = 0; k < cubesZ; k++)
                {
                    Vector3 center = new Vector3(origine.x + i * cubeSize, origine.y + j * cubeSize, origine.z + k * cubeSize);


                    GameObject c = Instantiate(cubeGO, center, Quaternion.identity);
                    c.GetComponent<Cube>().cubeSize = cubeSize;
                    c.GetComponent<Cube>().potential = Potential(c.transform.position);
                    c.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);


                }
            }
        }
    }

    private void Union()
    {
        for (int i = 0; i < cubesX; i++)
        {
            for (int j = 0; j < cubesY; j++)
            {
                for (int k = 0; k < cubesZ; k++)
                {
                    Vector3 center = new Vector3(origine.x + i * cubeSize, origine.y + j * cubeSize, origine.z + k * cubeSize);

                    bool inSphere = false;
                    int d = 0;
                    foreach (Vector3 sphere in spheres)
                    {
                        if (Vector3.Distance(new Vector3(center.x, center.y, center.z), spheres[d]) <= diametreSpheres[d] / 2f)
                        {
                            inSphere = true;
                            break;
                        }
                        d++;
                    }
                    if (inSphere)
                    {
                        GameObject c = Instantiate(cubeGO, center, Quaternion.identity);
                        c.GetComponent<Cube>().cubeSize = cubeSize;
                        c.GetComponent<Cube>().potential = Potential(c.transform.position);
                        c.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                    }

                }
            }
        }
    }

    private void Intersection()
    {
        for (int i = 0; i < cubesX; i++)
        {
            for (int j = 0; j < cubesY; j++)
            {
                for (int k = 0; k < cubesZ; k++)
                {
                    Vector3 center = new Vector3(origine.x + i * cubeSize, origine.y + j * cubeSize, origine.z + k * cubeSize);

                    int inSphere = 0;
                    int d = 0;
                    foreach (Vector3 sphere in spheres)
                    {
                        if (Vector3.Distance(new Vector3(center.x, center.y, center.z), spheres[d]) <= diametreSpheres[d] / 2f)
                        {
                            inSphere++;
                        }
                        d++;
                    }
                    if (inSphere == spheres.Count)
                    {
                        GameObject c = Instantiate(cubeGO, center, Quaternion.identity);
                        c.GetComponent<Cube>().cubeSize = cubeSize;
                        c.GetComponent<Cube>().potential = Potential(c.transform.position);
                        c.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                    }

                }
            }
        }
    }

    private void SetBox()
    {
        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        float minZ = Mathf.Infinity;

        float maxX = Mathf.NegativeInfinity;
        float maxY = Mathf.NegativeInfinity;
        float maxZ = Mathf.NegativeInfinity;

        int d = 0;
        foreach (Vector3 sphere in spheres)
        {
            if (sphere.x + diametreSpheres[d] > maxX)
                maxX = sphere.x + diametreSpheres[d];
            if (sphere.x - diametreSpheres[d] < minX)
                minX = sphere.x - diametreSpheres[d];

            if (sphere.y + diametreSpheres[d] > maxY)
                maxY = sphere.y + diametreSpheres[d];
            if (sphere.y - diametreSpheres[d] < minY)
                minY = sphere.y - diametreSpheres[d];

            if (sphere.z + diametreSpheres[d] > maxZ)
                maxZ = sphere.z + diametreSpheres[d];
            if (sphere.z - diametreSpheres[d] < minZ)
                minZ = sphere.z - diametreSpheres[d];
        }

        origine = new Vector3(minX, minY, minZ);

        int tailleX = (int)Mathf.Ceil(maxX - minX);
        int tailleY = (int)Mathf.Ceil(maxY - minY);
        int tailleZ = (int)Mathf.Ceil(maxZ - minZ);

        cubesX = (int)(tailleX * (1 / cubeSize));
        cubesY = (int)(tailleY * (1 / cubeSize));
        cubesZ = (int)(tailleZ * (1 / cubeSize));
    }

    private int InSphere(Vector3 center, int d)
    {
        int inSphere = 0;
        if (Vector3.Distance(new Vector3(center.x + cubeSize / 2f, center.y + cubeSize / 2f, center.z + cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }
        if (Vector3.Distance(new Vector3(center.x + cubeSize / 2f, center.y + cubeSize / 2f, center.z - cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }
        if (Vector3.Distance(new Vector3(center.x + cubeSize / 2f, center.y - cubeSize / 2f, center.z + cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }
        if (Vector3.Distance(new Vector3(center.x + cubeSize / 2f, center.y - cubeSize / 2f, center.z - cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }

        if (Vector3.Distance(new Vector3(center.x - cubeSize / 2f, center.y + cubeSize / 2f, center.z + cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }
        if (Vector3.Distance(new Vector3(center.x - cubeSize / 2f, center.y + cubeSize / 2f, center.z - cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }
        if (Vector3.Distance(new Vector3(center.x - cubeSize / 2f, center.y - cubeSize / 2f, center.z + cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }
        if (Vector3.Distance(new Vector3(center.x - cubeSize / 2f, center.y - cubeSize / 2f, center.z - cubeSize / 2f), spheres[d]) <= diametreSpheres[d] / 2f)
        {
            inSphere++;
        }
        return inSphere;
    }
    private void SetOctreeUnion()
    {
        for (int i = 0; i < cubesX; i++)
        {
            for (int j = 0; j < cubesY; j++)
            {
                for (int k = 0; k < cubesZ; k++)
                {
                    Vector3 center = new Vector3(origine.x + i * cubeSize, origine.y + j * cubeSize, origine.z + k * cubeSize);
                    InSphereUnion(center);
                }
            }
        }
    }

    private void InSphereUnion(Vector3 pos)
    {
        int maxInSphere = 0;
        int d = 0;
        foreach (Vector3 sphere in spheres)
        {
            int inSphere = InSphere(pos, d);
            if (inSphere > maxInSphere)
                maxInSphere = inSphere;
            d++;
        }
        if (maxInSphere > 0)
        {
            GameObject c = Instantiate(cubeGO, pos, Quaternion.identity);
            c.GetComponent<Cube>().cubeSize = cubeSize;
            c.GetComponent<Cube>().potential = Potential(c.transform.position);
            c.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

            if (maxInSphere != 8)
                listCubes.Add(c);
        }
    }

    private void OctreeUnion(List<GameObject> cubes)
    {
        cubeSize /= 2f;
        foreach (GameObject cube in cubes)
        {
            if (cube != null)
            {
                Vector3 center = cube.transform.position;

                InSphereUnion(new Vector3(center.x + cubeSize / 2f, center.y + cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereUnion(new Vector3(center.x + cubeSize / 2f, center.y + cubeSize / 2f, center.z - cubeSize / 2f));
                InSphereUnion(new Vector3(center.x + cubeSize / 2f, center.y - cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereUnion(new Vector3(center.x + cubeSize / 2f, center.y - cubeSize / 2f, center.z - cubeSize / 2f));
                InSphereUnion(new Vector3(center.x - cubeSize / 2f, center.y + cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereUnion(new Vector3(center.x - cubeSize / 2f, center.y + cubeSize / 2f, center.z - cubeSize / 2f));
                InSphereUnion(new Vector3(center.x - cubeSize / 2f, center.y - cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereUnion(new Vector3(center.x - cubeSize / 2f, center.y - cubeSize / 2f, center.z - cubeSize / 2f));
                Destroy(cube);
            }
        }
    }

    private void SetOctreeIntersection()
    {
        for (int i = 0; i < cubesX; i++)
        {
            for (int j = 0; j < cubesY; j++)
            {
                for (int k = 0; k < cubesZ; k++)
                {
                    Vector3 center = new Vector3(origine.x + i * cubeSize, origine.y + j * cubeSize, origine.z + k * cubeSize);
                    InSphereIntersection(center);
                }
            }
        }
    }
    private void InSphereIntersection(Vector3 pos)
    {
        int totalInSphere = 0;
        int intersection = 0;
        int d = 0;
        foreach (Vector3 sphere in spheres)
        {
            int inSphere = InSphere(pos, d);
            totalInSphere += inSphere;
            if (inSphere > 0)
                intersection++;
            d++;
        }
        if (intersection == spheres.Count)
        {
            GameObject c = Instantiate(cubeGO, pos, Quaternion.identity);
            c.GetComponent<Cube>().cubeSize = cubeSize;
            c.GetComponent<Cube>().potential = Potential(c.transform.position);
            c.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

            if (totalInSphere != 8 * spheres.Count)
                listCubes.Add(c);
        }
    }

    private void OctreeIntersection(List<GameObject> cubes)
    {
        cubeSize /= 2f;
        foreach (GameObject cube in cubes)
        {
            if (cube != null)
            {
                Vector3 center = cube.transform.position;

                InSphereIntersection(new Vector3(center.x + cubeSize / 2f, center.y + cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereIntersection(new Vector3(center.x + cubeSize / 2f, center.y + cubeSize / 2f, center.z - cubeSize / 2f));
                InSphereIntersection(new Vector3(center.x + cubeSize / 2f, center.y - cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereIntersection(new Vector3(center.x + cubeSize / 2f, center.y - cubeSize / 2f, center.z - cubeSize / 2f));
                InSphereIntersection(new Vector3(center.x - cubeSize / 2f, center.y + cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereIntersection(new Vector3(center.x - cubeSize / 2f, center.y + cubeSize / 2f, center.z - cubeSize / 2f));
                InSphereIntersection(new Vector3(center.x - cubeSize / 2f, center.y - cubeSize / 2f, center.z + cubeSize / 2f));
                InSphereIntersection(new Vector3(center.x - cubeSize / 2f, center.y - cubeSize / 2f, center.z - cubeSize / 2f));

                Destroy(cube);
            }
        }
    }
}
