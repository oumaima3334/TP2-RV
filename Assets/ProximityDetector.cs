using UnityEngine;
using TMPro;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform controller;
    [SerializeField] private GameObject[] cubes;
    [SerializeField] private Color proximityColor = Color.green;
    [SerializeField] private float triggerDistance = 0.2f;
    [SerializeField] private float scaleCoefficient = 1.2f;
    [SerializeField] private float angleThreshold = 20f;

    private Renderer[] renderers;
    private Color[] originalColors;
    private Vector3[] originalScales;
    private LineRenderer line;
    private TextMeshPro text;

    private void Start()
    {
        renderers = new Renderer[cubes.Length];
        originalColors = new Color[cubes.Length];
        originalScales = new Vector3[cubes.Length];

        for (int i = 0; i < cubes.Length; i++)
        {
            renderers[i] = cubes[i].GetComponent<Renderer>();
            originalColors[i] = renderers[i].material.color;
            originalScales[i] = cubes[i].transform.localScale;
        }

        
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.005f;
        line.endWidth = 0.005f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.color = Color.white;
        line.enabled = false;

       
        GameObject textObject = new GameObject("Distance and Angle");
        textObject.transform.SetParent(controller);
        textObject.transform.localPosition = new Vector3(0f, 0.05f, 0.15f);
        textObject.transform.localRotation = Quaternion.identity;
        text = textObject.AddComponent<TextMeshPro>();
        text.fontSize = 0.5f;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.enabled = false;
    }

    private void Update()
    {
        int closestCube = -1;
        float shortestDistance = Mathf.Infinity;

        
        for (int i = 0; i < cubes.Length; i++)
        {
            float distance = Vector3.Distance(controller.position, cubes[i].transform.position);

            if (distance < triggerDistance)
                renderers[i].material.color = proximityColor;
            else
                renderers[i].material.color = originalColors[i];

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestCube = i;
            }
        }

        if (closestCube == -1) return;

        
        Vector3 direction = cubes[closestCube].transform.position - controller.position;
        float angle = Vector3.Angle(controller.forward, direction);
        bool isPointed = angle < angleThreshold;

        
        for (int i = 0; i < cubes.Length; i++)
        {
            if (i == closestCube && shortestDistance < triggerDistance && isPointed)
                cubes[i].transform.localScale = originalScales[i] * scaleCoefficient;
            else
                cubes[i].transform.localScale = originalScales[i];
        }

        
        line.enabled = isPointed;
        text.enabled = isPointed;

        if (isPointed)
        {
            line.SetPosition(0, controller.position);
            line.SetPosition(1, cubes[closestCube].transform.position);
            text.text = "Distance : " + shortestDistance.ToString("F2") + " m\n" +
                        "Angle : " + angle.ToString("F1") + "°";
        }
    }
}