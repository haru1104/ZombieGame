using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunCurrentState // ���� ���� ����
    {
        Ready, //źâ�� �Ѿ��� �ְ� ���� �ٷ� ���� ����Ҽ� �ִ� ����
        Empty,//źâ�� �� �������� ������
        ReLoading // ������
    }
    public GunCurrentState gunCurrentState { get; private set; }

    public Transform fireEffectTransform;
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem blood;
    public GameObject bloodTransform;
    private LineRenderer bulletLineRenderer;



    public float damage; //���� ������ �� (�� �Ѹ��� ������ ���� ���� �±׷� ���ϴ� �Լ� ����)
    public float timeBetFire = 0.12f; // �Ѿ� �߻� ����
    public float bulletDistance = 50.0f;
    public float lastFireTime;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();

        //����� ���� �ΰ��� ���� 
        bulletLineRenderer.positionCount = 2;
        //���� �������� ��Ȱ��ȭ
        bulletLineRenderer.enabled = false;

    }
    private void Fire()
    {
        //�� �߻���� ����
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
            //���̰� � ������Ʈ�� �浹�� ��� 
            target = hit.collider.gameObject.tag;
            if (target != null)
            {
                DamageSet();
            }
            //������ġ ����
            hitPos = hit.point;
            Debug.LogWarning(target);
        }
        else
        {
            //���̰� �ٸ� ������Ʈ�� �浹���� �ʾҴٸ� 
            //�Ѿ��� �ִ� �����Ÿ����� ���ư������� ��ġ�� ����
            hitPos = fireEffectTransform.position + fireEffectTransform.forward * bulletDistance;
        }

        StartCoroutine(ShotEffect(hitPos , target));

    }
    private void DamageSet()
    {
        //���� ���Ǵ� ���� ���ӿ�����Ʈ �±׷� ����ͼ� �� �Ѹ��� �������� �����ϰ� �����浹��  enemy hp ��ũ��Ʈ �� hp ���� �� --
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
