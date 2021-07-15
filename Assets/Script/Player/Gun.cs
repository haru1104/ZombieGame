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
    // public ParticleSystem blood;
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
        GameObject _target = null;
        RaycastHit hit;

        Vector3 hitPos = new Vector3(0, 0, 0);
        string target = "";

        if (Physics.Raycast(fireEffectTransform.position , fireEffectTransform.forward , out hit , bulletDistance))
        {
            //���̰� � ������Ʈ�� �浹�� ��� 
            _target = hit.collider.gameObject;
            target = _target.tag;

            if (target != null)
            {
                DamageSet();
            }

            //������ġ ����
            hitPos = hit.point;

            // Debug.LogWarning(target);
        }
        else
        {
            //���̰� �ٸ� ������Ʈ�� �浹���� �ʾҴٸ� 
            //�Ѿ��� �ִ� �����Ÿ����� ���ư������� ��ġ�� ����
            hitPos = fireEffectTransform.position + fireEffectTransform.forward * bulletDistance;
        }

        if (target != null) {
            StartCoroutine(ShotEffect(hitPos, _target));
        }

    }
    private void DamageSet()
    {
        //���� ���Ǵ� ���� ���ӿ�����Ʈ �±׷� ����ͼ� �� �Ѹ��� �������� �����ϰ� �����浹��  enemy hp ��ũ��Ʈ �� hp ���� �� --
    }

    private IEnumerator ShotEffect(Vector3 pos, GameObject target)
    {
        muzzleFlashEffect.Play();

        bulletLineRenderer.SetPosition(0, fireEffectTransform.position);
        bulletLineRenderer.SetPosition(1, pos);
        bulletLineRenderer.enabled = true;

        if (target != null && target.tag == "Zombie") {
            if (target.transform.parent == null) {
                target.GetComponentInChildren<ParticleSystem>().gameObject.transform.position = pos;
                target.GetComponent<Zombie>().onDamaged();
            }
            else {
                target.transform.parent.GetComponentInChildren<ParticleSystem>().gameObject.transform.position = pos;
                target.transform.parent.GetComponent<Zombie>().onDamaged();
            }
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
