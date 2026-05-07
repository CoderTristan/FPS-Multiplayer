using UnityEngine;
using PurrNet;
using System.Collections;
using PurrNet.StateMachine;
using System.Collections.Generic;

public class Gun : StateNode
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float range = 20f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float fireRate = .5f;
    [SerializeField] private float recoilDuration = .2f;
    [SerializeField] private float recoilStrength = 1f;
    [SerializeField] private float rotationAmount = 25f;
    [SerializeField] private AnimationCurve recoilCurve;
    [SerializeField] private AnimationCurve rotationCurve;


    [SerializeField] private bool automatic;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem environmentHitEffect, playerHitEffect;

    [SerializeField] private List<Renderer> renderers = new();

    
    private float _lastFireTime;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Coroutine _recoilCoroutine;

    private void Awake()
    {
        ToggleVisuals(false);
    }

    public override void Enter()
    {
        base.Enter();
        ToggleVisuals(true);
    }

    public override void Exit()
    {
        base.Exit();
        ToggleVisuals(false);
    }

    private void Start()
    {
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    private void ToggleVisuals(bool toggle)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = toggle;
        }
    }


    protected override void OnSpawned()
    {
        base.OnSpawned();
        enabled = isOwner;
    }

    public override void StateUpdate(bool asServer)
    {
        
        if (automatic)
    {
        if (!Input.GetKey(KeyCode.Mouse0))
            return;
    }
    
    else
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;
    }

    if (_lastFireTime + fireRate > Time.unscaledTime)
        return;

    _lastFireTime = Time.unscaledTime;

    
    PlayShotEffect();

    if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, range, hitLayer))
        return;

    if (hit.transform.TryGetComponent(out PlayerHealth playerHealth))
        {
            if (environmentHitEffect)
            {
                Instantiate(environmentHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            playerHealth.ChangeHealth(-damage);
        }
    }

    
    [ObserversRpc(runLocally:true)]
    private void PlayShotEffect()
    {
        if (muzzleFlash == null) return;
        muzzleFlash.Play();
        if (_recoilCoroutine != null)
            StopCoroutine(_recoilCoroutine);
        _recoilCoroutine = StartCoroutine(PlayRecoil());
    }

    private IEnumerator PlayRecoil()
    {
        float elapsed = 0f;
        while (elapsed < recoilDuration)
        {
            elapsed += Time.deltaTime;
            float curveTime = elapsed / recoilDuration;
            float recoilValue = recoilCurve.Evaluate(curveTime);
            Vector3 recoilOffset = Vector3.back * recoilValue * recoilStrength;
            transform.localPosition = _originalPosition + recoilOffset;

            float rotationValue = rotationCurve.Evaluate(curveTime);
            Vector3 rotationOffset = new Vector3(0f, 0f, -rotationValue * rotationAmount);
            transform.localRotation = _originalRotation * Quaternion.Euler(rotationOffset);

            yield return null;
        }

        transform.localPosition = _originalPosition;
        transform.localRotation = _originalRotation;
    }
}
