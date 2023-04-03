using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class GunController : MonoBehaviourPun
{
    [HideInInspector] public bool aiming;
    [Header("Type")]
    [SerializeField] bool fullAuto;
    [SerializeField] bool semiAuto;
    [SerializeField] bool shotgun;
    [SerializeField] bool HasSlide;
    [Header("Specs")]
    [SerializeField] float damage = 10f;
    [SerializeField] float headshotDamage = 20f;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] float range = 100f;
    [SerializeField] float FireRate = 4f;
    [SerializeField] float FireSpread = 0.05f;
    [SerializeField] float AimSpread = 0.01f;
    [SerializeField] int MagSize = 15;
    [SerializeField] float Force = 100f;
    [SerializeField] float AdsSpeed = 5f;
    [SerializeField] float AdsFov = 50f;
    [SerializeField] float IdleFov = 70f;

    bool msgSent;
    bool recoilActivate;
    float NextTimeToFire = 0f;
    int ClipSize;

    LayerMask HitApplicable;

    [Header("Transforms")]
    [SerializeField] Transform gunPOS;
    [SerializeField] Transform SlidePOS;
    [SerializeField] Transform lineStart;
    [Header("Positions")]
    [SerializeField] Vector3 RecoilPosition = new Vector3(0f, 0f, -0.05f);
    [SerializeField] Vector3 idlePOS;
    [SerializeField] Vector3 ADSPOS;
    [SerializeField] Vector3 idleSlidePOS;
    [Header("Effects/Sounds")]
    [SerializeField] GameObject impact;
    [SerializeField] GameObject HitSound;
    [SerializeField] GameObject headShotSound;
    [SerializeField] GameObject gunshotSound;
    [SerializeField] TrailRenderer line;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject serverMSG;

    // ITEMS/OBJECTS // 
    AudioSource reloadSound;
    Animator animator;
    Movement movement;
    Camera cam;
    GameObject clonedFolder;
    TMP_Text clipText;
    Image crosshair;
    Transform contentView;
    PlayerRoomInfo localRoomInfo;

    private void Awake()
    {
        ClipSize = MagSize;
        contentView = GameObject.Find("Content").transform;
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        clipText = GameObject.Find("clipAmount").GetComponent<TMP_Text>();
        HitApplicable = LayerMask.GetMask("Ground", "Enemy", "Default");
        clonedFolder = GameObject.Find("ClonedFolder");
        animator = gameObject.GetComponent<Animator>();
        reloadSound = gameObject.GetComponent<AudioSource>();
        localRoomInfo = GameObject.Find("PlayerRoomInfo").GetComponent<PlayerRoomInfo>();
    }
    private void OnEnable()
    {
        NextTimeToFire = Time.time + 0.5f;
        clipText.text = ClipSize + "/" + MagSize;
        cam = GetComponentInParent<Camera>();
        movement = GetComponentInParent<Movement>();
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (fullAuto)
            {
                if (Input.GetButton("Fire1") && Time.time >= NextTimeToFire && movement.Sprinting == false && ClipSize > 0)
                {
                    NextTimeToFire = Time.time + 1f / FireRate;
                    ClipSize -= 1;
                    clipText.text = ClipSize + "/" + MagSize;

                    StartCoroutine(PlayRecoil());
                    muzzleFlash.Play();
                    gunshotclone = Instantiate(gunshotSound, cam.transform.position, cam.transform.rotation);
                    Destroy(gunshotclone, 1f);
                    CreateBullet();
                }
            }
            if (semiAuto)
            {
                if (Input.GetButtonDown("Fire1") && Time.time >= NextTimeToFire && movement.Sprinting == false && ClipSize > 0)
                {
                    NextTimeToFire = Time.time + 1f / FireRate;
                    ClipSize -= 1;
                    clipText.text = ClipSize + "/" + MagSize;

                    StartCoroutine(PlayRecoil());
                    muzzleFlash.Play();
                    gunshotclone = Instantiate(gunshotSound, cam.transform.position, cam.transform.rotation);
                    Destroy(gunshotclone, 1f);
                    CreateBullet();
                }
            }
            if (shotgun)
            {
                if (Input.GetButtonDown("Fire1") && Time.time >= NextTimeToFire && movement.Sprinting == false && ClipSize > 0)
                {
                    NextTimeToFire = Time.time + 1f / FireRate;
                    ClipSize -= 1;
                    clipText.text = ClipSize + "/" + MagSize;

                    StartCoroutine(PlayRecoil());
                    muzzleFlash.Play();
                    gunshotclone = Instantiate(gunshotSound, cam.transform.position, cam.transform.rotation);
                    Destroy(gunshotclone, 1f);
                    CreateBullet();
                    CreateBullet();
                    CreateBullet();
                    CreateBullet();
                    CreateBullet();
                    CreateBullet();
                    CreateBullet();
                    CreateBullet();
                }
            }
            if (Input.GetButton("Fire2") && movement.Sprinting == false)
            {
                aiming = true;
                gunPOS.localPosition = Vector3.Lerp(gunPOS.localPosition, ADSPOS, AdsSpeed * Time.deltaTime);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, AdsFov, AdsSpeed * Time.deltaTime);
                if (HasSlide)
                {
                    SlidePOS.localPosition = Vector3.Lerp(SlidePOS.localPosition, idleSlidePOS, 8f * Time.deltaTime);
                }
                crosshair.CrossFadeColor(Color.clear, 0.75f, true, true);
            }
            else
            {
                aiming = false;
                gunPOS.localPosition = Vector3.Lerp(gunPOS.localPosition, idlePOS, AdsSpeed * Time.deltaTime);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, IdleFov, AdsSpeed * Time.deltaTime);
                if (HasSlide)
                {
                    SlidePOS.localPosition = Vector3.Lerp(SlidePOS.localPosition, idleSlidePOS, 8f * Time.deltaTime);
                }
                crosshair.CrossFadeColor(Color.white, 0.25f, true, true);
            }
            if (Input.GetButtonDown("Reload") && movement.Sprinting == false && aiming == false)
            {
                animator.Play("Reloading");
                reloadSound.Play();
                ClipSize = MagSize;
                clipText.text = ClipSize + "/" + MagSize;
                NextTimeToFire = Time.time + reloadTime;
            }
            if (recoilActivate)
            {
                
                Vector3 SlideRecoilPosition = new Vector3(0.9f, 0f, 0f);
                gunPOS.localPosition = Vector3.Lerp(gunPOS.localPosition, gunPOS.localPosition + RecoilPosition, 15f * Time.deltaTime);
                if (HasSlide)
                {
                    SlidePOS.localPosition = Vector3.Lerp(SlidePOS.localPosition, SlidePOS.localPosition + SlideRecoilPosition, 15f * Time.deltaTime);
                }
            }
        }
    }
    GameObject gunshotclone;
    void CreateBullet()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, GetShootingDirection(), out hit, range, HitApplicable))
        {
            GameObject trailGO = PhotonNetwork.Instantiate("Trail", lineStart.position, Quaternion.identity);
            int TrailID = trailGO.GetComponent<PhotonView>().ViewID;
            photonView.RPC("RPC_SpawnTrail", RpcTarget.All, TrailID, hit.point);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Health enemyHealth = hit.collider.GetComponent<Health>();
                PhotonView TarPly_PV = hit.transform.GetComponent<PhotonView>();
                int TarPlyID = TarPly_PV.ViewID;
                int PlyActrN = TarPly_PV.OwnerActorNr;
                int localID = photonView.ViewID;
                    if (hit.collider.tag == "Head")
                    {
                    GameObject HitSoundClone = Instantiate(HitSound, cam.transform.position, cam.transform.rotation, clonedFolder.transform);
                        Destroy(HitSoundClone, 1f);
                        photonView.RPC("RPC_TarPly_TakeDamage", RpcTarget.All, TarPlyID, true, localID);
                    }
                    else
                    {
                    GameObject HitSoundClone = Instantiate(HitSound, cam.transform.position, cam.transform.rotation, clonedFolder.transform);
                        Destroy(HitSoundClone, 1f);
                        photonView.RPC("RPC_TarPly_TakeDamage", RpcTarget.All, TarPlyID, false, localID);
                    }
                    GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal), clonedFolder.transform);
                    Destroy(impactGO, 2f);

                if (enemyHealth.dead == true)
                {
                    if (!msgSent)
                    {
                        StartCoroutine(DelayBetweenKills());
                        localRoomInfo.KilledPlayer();
                        string killedPlayerName = PhotonNetwork.CurrentRoom.GetPlayer(PlyActrN).NickName;
                        photonView.RPC("SendServerMessage", RpcTarget.All, PhotonNetwork.NickName, killedPlayerName);
                    }
                }
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal*Force);
            }
        }
    }
    IEnumerator DelayBetweenKills()
    {
        msgSent = true;
        yield return new WaitForSeconds(1f);
        msgSent = false;
    }
    IEnumerator PlayRecoil()
    {
        recoilActivate = true;
        yield return new WaitForSeconds(0.1f);
        recoilActivate = false;
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
    void SendServerMessage(string Killer, string Killed)
    {
        GameObject srvrMSG = Instantiate(serverMSG, contentView);
        srvrMSG.GetComponent<TMP_Text>().text = Killer + " killed " + Killed;
    }
    [PunRPC]
    void RPC_TarPly_TakeDamage(int TarPlyID, bool headshot, int AttackerPlyID)
    {
        PhotonView tarPlyPV = PhotonView.Find(TarPlyID);
        GameObject tarPly = tarPlyPV.gameObject;

        Health health = tarPly.transform.GetComponent<Health>();
        if (health != null)
        {
            if (headshot)
            {
                health.SetKillerID(AttackerPlyID);
                health.TakeDamage(headshotDamage);
            }
            else
            {
                health.SetKillerID(AttackerPlyID);
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