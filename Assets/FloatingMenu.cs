using UnityEngine;
using TMPro;

public class FloatingMenu : MonoBehaviour
{
    [SerializeField] private float distanceDevant = 0.15f; 

    private TextMeshPro text;
    private Transform cam;

    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        cam = Camera.main.transform;

        
        transform.localPosition = new Vector3(0f, 0f, distanceDevant);
    }

    void Update()
    {
       
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);

       
        text.text = "Local : " + transform.localPosition.ToString("F2") + "\n" +
                    "Monde : " + transform.position.ToString("F2");
    }
}