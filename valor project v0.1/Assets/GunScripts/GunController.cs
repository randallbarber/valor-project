using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviourPun
{
    [HideInInspector] public bool aiming;
    [SerializeField] float damage = 10f;
    [SerializeField] float headshotDamage = 20f;
    [SerializeField] float range = 100f;
    [SerializeField] float FireRate = 4f;
    [SerializeField] float FireSpread = 0.05f;
    [SerializeField] float AimSpread = 0.01f;
    [SerializeField] int MagSize = 15;
    [SerializeField] float Force = 100f;
    [SerializeField] float AdsSpeed = 5f;
    [SerializeField] float AdsFov = 50f;
    [SerializeField] float IdleFov = 70f;
    float NextTimeToFire = 0f;
    bool Reloading;
    int ClipSize;

    LayerMask HitApplicable;
    [SerializeField] Transform gunPOS;
    [SerializeField] Vector3 idlePOS;
    [SerializeField] Vector3 ADSPOS;
    [SerializeField] Transform lineStart;

    [SerializeField] GameObject impact;
    [SerializeField] GameObject HitSound;
    [SerializeField] GameObject headShotSound;
    [SerializeField] GameObject gunshotSound;
    [SerializeField] TrailRenderer line;
    [SerializeField] ParticleSystem muzzleFlash;

    AudioSource reloadSound;
    Animator animator;
    Movement movement;
    Camera cam;
    GameObject clonedFolder;
    private void Awake()
    {
        ClipSize = MagSize;
        HitApplicable = LayerMask.GetMask("Ground", "Enemy", "Default");
        clonedFolder = GameObject.Find("ClonedFolder");
        animator = gameObject.GetComponent<Animator>();
        reloadSound = gameObject.GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        NextTimeToFire = Time.time + 0.5f;
        cam = GetComponentInParent<Camera>();
        movement = GetComponentInParent<Movement>();
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButton("Fire1") && Time.time >= NextTimeToFire && movement.Sprinting == false && Reloading == false && ClipSize > 0)
            {
                NextTimeToFire = Time.time + 1f / FireRate;
                ClipSize -= 1;
                shoot();
            }
            if (Input.GetButtonDown("Fire1"))
            {
                photonView.RPC("_IsShooting", RpcTarget.Others, true);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                photonView.RPC("_IsShooting", RpcTarget.Others, false);
            }
            if (Input.GetButton("Fire2") && movement.Sprinting == false && Reloading == false)
            {
                photonView.RPC("RPC_UpdateAim", RpcTarget.All, true, ADSPOS, AdsFov);
            }
            else
            {
                photonView.RPC("RPC_UpdateAim", RpcTarget.All, false, idlePOS, IdleFov);
            }
            if (Input.GetButton("Reload") && movement.Sprinting == false && aiming == false)
            {
                animator.Play("Reloading");
                reloadSound.Play();
                ClipSize = MagSize;
                NextTimeToFire = Time.time + 1f;
            }
        }
    }
    [PunRPC]
    void _IsShooting(bool TrueOrFalse)
    {
        animator.SetBool("IsShooting", TrueOrFalse);
    }
    [PunRPC]
    void RPC_UpdateAim(bool IsAiming, Vector3 TargetPOS, float TargetFOV)
    {
        aiming = IsAiming;
        gunPOS.localPosition = Vector3.Lerp(gunPOS.localPosition, TargetPOS, AdsSpeed * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, TargetFOV, AdsSpeed * Time.deltaTime);
    }
    GameObject gunshotclone;
    void shoot()
    {
        RaycastHit hit;

        animator.Play("recoil", 0, 0f);
        muzzleFlash.Play();

        gunshotclone = Instantiate(gunshotSound, cam.transform.position, cam.transform.rotation);
        Destroy(gunshotclone, 1f);

        if (Physics.Raycast(cam.transform.position, GetShootingDirection(), out hit, range, HitApplicable))
        {
            GameObject trailGO = PhotonNetwork.Instantiate("Trail", lineStart.position, Quaternion.identity);
            int TrailID = trailGO.GetComponent<PhotonView>().ViewID;
            photonView.RPC("RPC_SpawnTrail", RpcTarget.All, TrailID, hit.point);

            
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                PhotonView TarPly_PV = hit.transform.GetComponent<PhotonView>();
                int TarPlyID = TarPly_PV.ViewID;
                    if (hit.collider.tag == "Head")
                    {
                    GameObject HitSoundClone = Instantiate(HitSound, cam.transform.position, cam.transform.rotation, clonedFolder.transform);
                    Destroy(HitSoundClone, 1f);
                    photonView.RPC("RPC_TarPly_TakeDamage", RpcTarget.All, TarPlyID, true);
                    }
                    else
                    {
                        GameObject HitSoundClone = Instantiate(HitSound, cam.transform.position, cam.transform.rotation, clonedFolder.transform);
                        Destroy(HitSoundClone, 1f);
                        photonView.RPC("RPC_TarPly_TakeDamage", RpcTarget.All, TarPlyID, false);
                    }
                    GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal), clonedFolder.transform);
                    Destroy(impactGO, 2f);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal*Force);
            }
        }
    }
    Vector3 GetShootingDirection()
    {
        Vector3 direction = cam.transform.forward;
        if (aiming == false)
        {
            direction += new Vector3(
                Random.Range(-FireSpread, FireSpread),
                Random.Range(-FireSpread, FireSpread),
                Random.Range(-FireSpread, FireSpread)
                );
        }
        else
        {
            direction += new Vector3(
                Random.Range(-AimSpread, AimSpread),
                Random.Range(-AimSpread, AimSpread),
                Random.Range(-AimSpread, AimSpread)
                );
        }
        direction.Normalize();
        return direction;
    }
    [PunRPC]
    void RPC_TarPly_TakeDamage(int TarPlyID, bool headshot)
    {
        PhotonView tarPlyPV = PhotonView.Find(TarPlyID);
        GameObject tarPly = tarPlyPV.gameObject;

        Health health = tarPly.transform.GetComponent<Health>();
        if (health != null)
        {
            if (headshot)
            {
                health.TakeDamage(headshotDamage);
            }
            else
            {
                health.TakeDamage(damage);
            }
        }
        Target target = tarPly.transform.GetComponent<Target>();
        if (target != null)
        {
            if (headshot)
            {
                target.TakeDamage(headshotDamage);
            }
            else
            {
                target.TakeDamage(damage);
            }
        }
    }
    [PunRPC]
    void RPC_SpawnTrail(int trailID, Vector3 hit)
    {
        TrailRenderer trail = PhotonView.Find(trailID).gameObject.GetComponent<TrailRenderer>();
        StartCoroutine(SpawnTrail(trail, hit));
    }
        IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint)
    {
        float time = 0f;
        Vector3 startPosition = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hitPoint;
        Destroy(trail.gameObject, trail.time);
    }
}
