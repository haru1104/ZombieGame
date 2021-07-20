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

    public float damage = 20f; //���� ������ �� (�� �Ѹ��� ������ ���� ���� �±׷� ���ϴ� �Լ� ����)
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

            //������ġ ����
            hitPos = hit.point;
        }
        else
        {
            //���̰� �ٸ� ������Ʈ�� �浹���� �ʾҴٸ� 
            //�Ѿ��� �ִ� �����Ÿ����� ���ư������� ��ġ�� ����
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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
}
