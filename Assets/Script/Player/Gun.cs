using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunCurrentState // 현재 총의 상태
    {
        Ready, //탄창에 총알이 있고 지금 바로 총을 사용할수 있는 상태
        Empty,//탄창이 다 떨어지고 없을때
        ReLoading // 재장전
    }
    public GunCurrentState gunCurrentState { get; private set; }

    public Transform fireEffectTransform;
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem blood;
    public GameObject bloodTransform;
    private LineRenderer bulletLineRenderer;



    public float damage; //총의 데미지 값 (각 총마다 데미지 값을 만들어서 태그로 비교하는 함수 제작)
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
    private void Fire()
    {
        //총 발사시점 갱신
        lastFireTime = Time.time;

        Shot();
    }
    private void Shot()
    {
        RaycastHit hit;

        Vector3 hitPos = new Vector3(0, 0, 0);
        string target="";

        if (Physics.Raycast(fireEffectTransform.position , fireEffectTransform.forward , out hit , bulletDistance))
        {
            //레이가 어떤 오브젝트에 충돌한 경우 
            target = hit.collider.gameObject.tag;
            if (target != null)
            {
                DamageSet();
            }
            //맞은위치 저장
            hitPos = hit.point;
            Debug.LogWarning(target);
        }
        else
        {
            //레이가 다른 오브젝트와 충돌하지 않았다면 
            //총알이 최대 사정거리까지 날아갔을때의 위치를 저장
            hitPos = fireEffectTransform.position + fireEffectTransform.forward * bulletDistance;
        }

        StartCoroutine(ShotEffect(hitPos , target));

    }
    private void DamageSet()
    {
        //현재 사용되는 총을 게임오브젝트 태그로 갖고와서 각 총마다 데미지를 세팅하고 레이충돌시  enemy hp 스크립트 에 hp 벨류 값 --
    }

    private IEnumerator ShotEffect(Vector3 pos, string tag)
    {
        muzzleFlashEffect.Play();

        bulletLineRenderer.SetPosition(0, fireEffectTransform.position);
        bulletLineRenderer.SetPosition(1, pos);
        bulletLineRenderer.enabled = true;
        if (tag == "Zombie")
        {
            bloodTransform.transform.position = pos;
            blood.Play();
        }
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
}
