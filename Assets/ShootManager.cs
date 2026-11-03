using UnityEngine;
using UnityEngine.XR;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private Transform controller;            
    [SerializeField] private float angleTolerance = 10f;      
    [SerializeField] private GameObject hitParticlesPrefab;   
    [SerializeField] private AudioClip hitSound;              

    private GameObject[] targets;
    private InputDevice _rightController;
    private bool previousTriggerPressed = false;

    void Start()
    {
        
        targets = GameObject.FindGameObjectsWithTag("Target");

       
        _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        
        if (!_rightController.isValid)
        {
            _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (!_rightController.isValid) return;
        }

        
        if (_rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed && !previousTriggerPressed)
                Shoot();

            previousTriggerPressed = triggerPressed;
        }
    }

    private void Shoot()
    {
        GameObject bestTarget = null;
        float bestAngle = angleTolerance;

        
        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            Vector3 direction = target.transform.position - controller.position;
            float angle = Vector3.Angle(controller.forward, direction);

            
            if (angle < bestAngle)
            {
                bestAngle = angle;
                bestTarget = target;
            }
        }

        if (bestTarget == null) return; // aucune cible touchée

       
        GameObject particles = Instantiate(hitParticlesPrefab, bestTarget.transform.position, Quaternion.identity);
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        ps.Play();
        Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax);

        
        AudioSource audioSource = bestTarget.GetComponent<AudioSource>();
        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }
}