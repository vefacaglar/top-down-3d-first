using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform spawn;
    [SerializeField] GunType type;

    public void Shoot()
    {
        Ray ray = new Ray(spawn.position, spawn.forward);
        RaycastHit hit;

        float range = 100f;

        if (Physics.Raycast(ray, out hit, 100))
        {
            range = hit.distance;
        }

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);
    }

    public void ShootContinuous()
    {
        if (type == GunType.Auto)
        {
            Shoot();
        }
    }
}

public enum GunType
{
    Semi, Auto, Burst,
}
