using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] Transform spawn;
    [SerializeField] GunType type;
    [SerializeField] float rpm = 0.1f;

    private float secondsBetweenShots;
    private float nextPossibleShoot;

    private AudioSource audioSource;

    private void Start()
    {
        secondsBetweenShots = 60f / rpm;
        audioSource = GetComponent<AudioSource>();
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
        audioSource.PlayOneShot(audioSource.clip);
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
}

public enum GunType
{
    Semi, Auto, Burst,
}
