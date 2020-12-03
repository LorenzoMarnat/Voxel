using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [Range(0.9f, 3)]
    public float potential = 0.9f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");

        foreach (GameObject cube in cubes)
        {
            if (cube != null)
            {
                bool active = cube.GetComponent<Renderer>().enabled;
                if (active && cube.GetComponent<Cube>().potential < potential)
                    cube.GetComponent<Renderer>().enabled = false;

                else if (!active && cube.GetComponent<Cube>().potential >= potential)
                    cube.GetComponent<Renderer>().enabled = true;
            }

        }
    }

    public void HideAndShow()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject cube in cubes)
            cube.GetComponent<Renderer>().enabled = !cube.GetComponent<Renderer>().enabled;
    }
}
