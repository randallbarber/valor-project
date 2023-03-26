using System.Collections;
using UnityEngine;

public class ShotGunScript: MonoBehaviour
{
    public float damage = 10f;
    float range = 100f;
    public float NextTimeToFire = 0f;
    float FireRate = 1.3f;
    float Force = 100;

    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;

    public Transform gunPOS;
    public Transform idlePOS;
    public Transform ADSPOS;

    public Animator animator;

    float ADSspeed = 5f;
    public bool aiming = false;

    public AudioSource gunshot;
    public Movement movement;
    private void OnEnable()
    {
        cam = GameObject.Find("Player").GetComponentInChildren<Camera>();
        movement = GameObject.Find("Player").GetComponent<Movement>();
        NextTimeToFire = Time.time + 1f;
    }
    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= NextTimeToFire && movement.Sprinting == false)
        {
            animator.Play("ShoptGunRecoil", 0, 0f);
            muzzleFlash.Play();
            gunshot.Play();
            NextTimeToFire = Time.time + 1f/FireRate;
            shoot();
            shoot();
            shoot();
            shoot();
            shoot();
            shoot();
            shoot();
        }
        if (Input.GetButton("Fire2") && movement.Sprinting == false)
        {
            aiming = true;
            gunPOS.localPosition = Vector3.Lerp(gunPOS.localPosition, ADSPOS.localPosition, ADSspeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 50, ADSspeed * Time.deltaTime);
        }
        else
        {
            aiming = false;
            gunPOS.localPosition = Vector3.Lerp(gunPOS.localPosition, idlePOS.localPosition, ADSspeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, ADSspeed * Time.deltaTime);
        }
    }
    public LayerMask enemy;
    public TrailRenderer line;
    public Transform lineStart;
    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, GetShootingDirection(), out hit, range))
        {
            TrailRenderer trail = Instantiate(line, lineStart.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);

                target.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * Force);
            }
        }
    }
    Vector3 GetShootingDirection()
    {
        Vector3 direction = cam.transform.forward;
        direction += new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f)
            );
        direction.Normalize();
        return direction;
    }
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0f;
        Vector3 startPosition = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }
}
