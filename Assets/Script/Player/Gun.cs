using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private LineRenderer bulletLineRenderer;
    private SoundManager audioManager;

    private int maxAmmo = 20;
    private int nowAmmo = 20;

    public enum GunCurrentState // 현재 총의 상태
    {
        Ready, //탄창에 총알이 있고 지금 바로 총을 사용할수 있는 상태
        Empty,//탄창이 다 떨어지고 없을때
        ReLoading // 재장전
    }

    public GunCurrentState gunCurrentState { get; private set; }

    public Transform fireEffectTransform;
    public ParticleSystem muzzleFlashEffect;

    public Text remainAmmo;

    public float damage = 20f; //총의 데미지 값 (각 총마다 데미지 값을 만들어서 태그로 비교하는 함수 제작)
    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float bulletDistance = 50.0f;
    public float lastFireTime;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();

        //사용할 점을 두개로 변경 
        bulletLineRenderer.positionCount = 2;
        //라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;

    }

    private void Start() {
        audioManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
    }

    private void Update() {
        updateAmmoStatus();
    }
  
    private void updateAmmoStatus() {
        remainAmmo.text = nowAmmo + " / " + maxAmmo;
    }

    public void Fire()
    {
        //총 발사시점 갱신
        lastFireTime = Time.time;

        if (nowAmmo > 0) {
            Shot();
        }
        else {
            reloadAmmo();
        }
    }

    private void Shot()
    {
        nowAmmo--;
        audioManager.GunShotSound();

        GameObject _target = null;
        RaycastHit hit;

        Vector3 hitPos = new Vector3(0, 0, 0);
        string target = "";

        if (Physics.Raycast(fireEffectTransform.position , fireEffectTransform.forward , out hit , bulletDistance))
        {
            //레이가 어떤 오브젝트에 충돌한 경우 
            _target = hit.collider.gameObject;
            target = _target.tag;

            //맞은위치 저장
            hitPos = hit.point;
        }
        else
        {
            //레이가 다른 오브젝트와 충돌하지 않았다면 
            //총알이 최대 사정거리까지 날아갔을때의 위치를 저장
            hitPos = fireEffectTransform.position + fireEffectTransform.forward * bulletDistance;
        }

        if (target != null) {
            StartCoroutine(FireGun(hitPos, _target));
        }

    }

    private IEnumerator FireGun(Vector3 pos, GameObject target)
    {
        muzzleFlashEffect.Play();

        bulletLineRenderer.SetPosition(0, fireEffectTransform.position);
        bulletLineRenderer.SetPosition(1, pos);
        bulletLineRenderer.enabled = true;

        GameObject zombie = target;

        if (target != null) {
            if (target.transform.parent != null) {
                zombie = target.transform.parent.gameObject;
            }

            if (zombie.tag == "Zombie") {
                zombie.GetComponentInChildren<ParticleSystem>().gameObject.transform.position = pos;
                zombie.GetComponent<Zombie>().onDamaged(damage);
            }
        }

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    public void reloadAmmo() {
        audioManager.GunReloadSound();
        Debug.Log("Reloading cover me!!!");
        nowAmmo = 20;
    }
}
