using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] Transform spawn;
    [SerializeField] GunType type;
    [SerializeField] float rpm = 0.1f;

    private float secondsBetweenShots;
    private float nextPossibleShoot;

    private LineRenderer tracer;
    private AudioSource audioSource;

    private void Start()
    {
        secondsBetweenShots = 60f / rpm;
        audioSource = GetComponent<AudioSource>();
        tracer = GetComponent<LineRenderer>();
    }

    public void Shoot()
    {
        if (!CanShoot()) return;

        Ray ray = new Ray(spawn.position, spawn.forward);
        RaycastHit hit;

        float range = 20f;

        if (Physics.Raycast(ray, out hit, range))
        {
            range = hit.distance;
        }

        nextPossibleShoot = Time.time + secondsBetweenShots;
        // audioSource.PlayOneShot(audioSource.clip);
        audioSource.Play();

        if (tracer != null)
        {
            StartCoroutine("RenderTracer", ray.direction * range);
        }
    }

    public void ShootContinuous()
    {
        if (type == GunType.Auto)
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        bool canShoot = true;

        if (Time.time < nextPossibleShoot)
        {
            canShoot = false;
        }

        return canShoot;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, spawn.position);
        tracer.SetPosition(1, spawn.position + hitPoint);
        yield return null;
        tracer.enabled = false;
    }

}

public enum GunType
{
    Semi, Auto, Burst,
}
