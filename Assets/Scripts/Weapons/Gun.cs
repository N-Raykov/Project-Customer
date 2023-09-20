using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public abstract class Gun : MonoBehaviourWithPause{

    public event Action<int, int> OnAmmoChange;
    public event Action<float> OnSpreadChange;
    public event Action<float> OnReload;
    public event Action<bool> OnZoomChange;
    public event Action OnShoot;
    protected event Action OnStateChange;

    public enum States {
        Idle,
        Shoot,
        Reload
    }
    public States state { get; protected set; }
    protected float lastShotTime = 0;
    protected float spreadMultiplier;

    [Header("Data")]
    [SerializeField] public int extraAmmo;//i know i know
    [SerializeField] protected GunData gunData;
    [SerializeField] protected Animator animator;
    [SerializeField] protected PlayerInput input;
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected Camera weaponCamera;
    [SerializeField] protected LayerMask mask;
    public int currentAmmo { get; set; }
    protected Vector3 recoilTargetRotation = Vector3.zero;
    protected Vector3 pistolRotationPivotStartPosition;
    public bool isAiming { get; protected set; }
    protected float originalFOV;
    protected float originalFOVWeaponCamera;
    protected bool isAimingAllowed = true;

    [Header("Objects")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected Transform pistolRotationPivot;
    [SerializeField] protected Transform recoilPivot;
    [SerializeField] protected GameObject muzzleFlash;

    public bool canBeAccessed { get; set; }//must be set for each weapon

    protected virtual void Start() {
        state = States.Idle;
        currentAmmo = gunData.ammoCapacity;
        animator = transform.GetComponent<Animator>();
        OnAmmoChange?.Invoke(currentAmmo, extraAmmo);
        pistolRotationPivotStartPosition = pistolRotationPivot.localPosition;
        originalFOV = mainCamera.fieldOfView;
        originalFOVWeaponCamera = weaponCamera.fieldOfView;
    }

    protected override void UpdateWithPause() {
        CheckForActions();
        DecreaseSpreadMultiplier();
        DecreaseRecoilRotation();
    }


    protected virtual void CheckForActions() {
        CheckForShots();
        CheckForReload();
        CheckForAim();
    }

    protected void CheckForShots() {
        if (input.shootInput && state == States.Idle && currentAmmo > 0)
            Shoot();
    }

    protected void CheckForReload() {
        if (input.reloadInput && state == States.Idle && currentAmmo < gunData.ammoCapacity && extraAmmo > 0) {
            if (!isAiming)
                Reload();
            else {//quikly move back to the normal position and start reload
                StartCoroutine(MoveBackAndReload());
            }
        }

    }

    protected void CheckForAim(){
        if (input.aimInput)
            Aim();
        else {
            if (pistolRotationPivot.localPosition != pistolRotationPivotStartPosition&&isAiming) {
                pistolRotationPivot.DOLocalMove(pistolRotationPivotStartPosition, gunData.zoomInDuration);//1
                mainCamera.DOPause();
                transform.localEulerAngles -= new Vector3(0, 0, gunData.zChangeForAiming);//here
                pistolRotationPivot.localEulerAngles = new Vector3(0, -5, 0);
                isAiming = false;
                StartCoroutine(DecreaseFOVAndAddUI(gunData.zoomInDuration));
            }

        }
    }

    protected void Aim() {
        if (!isAiming && state != States.Reload&&isAimingAllowed) {
            pistolRotationPivot.DOPause();
            isAiming = true;
            transform.localEulerAngles += new Vector3(0, 0, gunData.zChangeForAiming);//here
            pistolRotationPivot.localEulerAngles = new Vector3(0, 0, 0);
            pistolRotationPivot.DOLocalMove(gunData.targetPosition, gunData.zoomInDuration);//2
            StartCoroutine(IncreaseFOVAndRemoveUI(gunData.zoomInDuration));
        }
    }

    protected IEnumerator IncreaseFOVAndRemoveUI(float duration) {

        yield return new WaitForSeconds(duration / 2);
        if (isAiming) {
            mainCamera.DOFieldOfView(originalFOV / gunData.zoomInFactor, duration / 2);
            weaponCamera.DOFieldOfView(originalFOVWeaponCamera/gunData.zoomInFactor,duration/2);
        }

        yield return new WaitForSeconds(duration / 4);
        if (isAiming)
            OnZoomChange?.Invoke(false);
        
    }

    protected IEnumerator DecreaseFOVAndAddUI(float duration) {
        yield return new WaitForSeconds(duration / 4);
        if (!isAiming) {
            mainCamera.DOFieldOfView(originalFOV, duration / 2);
            weaponCamera.DOFieldOfView(originalFOVWeaponCamera,duration/2);
            OnZoomChange?.Invoke(true);
        }
    }

    protected virtual void Shoot() {
        OnShoot?.Invoke();
        AddRecoil();
        StartShotAnimation();
        lastShotTime = Time.time;
        AddSpread();
        CreateBullet();
        state = States.Shoot;
        currentAmmo--;
        OnAmmoChange?.Invoke(currentAmmo, extraAmmo);
        StartCoroutine(ChangeStateAfterTime(gunData.shotCooldown,States.Idle));
    }

    protected virtual void CreateBullet() {
        GameObject b = Instantiate(bullet, muzzle.position, mainCamera.transform.rotation);
        Bullet bt = b.GetComponent<Bullet>();
        bt.damage = gunData.damage;
        bt.speed = gunData.bulletSpeed;
        bt.range = gunData.range;
        bt.AddSpeed(AimAtTarget());
        Instantiate(muzzleFlash, muzzle.position, mainCamera.transform.rotation, muzzle);
    }

    protected abstract void StartShotAnimation();

    protected void Reload() {
        StartReloadAnimation();
        state = States.Reload;
        extraAmmo += currentAmmo;
        OnReload?.Invoke(gunData.reloadTime);
        currentAmmo = Mathf.Min(gunData.ammoCapacity, extraAmmo);
        extraAmmo -= Mathf.Min(gunData.ammoCapacity, extraAmmo);
        OnStateChange += ChangeDisplayAmmo;
        StartCoroutine(ChangeStateAfterTime(gunData.reloadTime, States.Idle));
    }

    IEnumerator MoveBackAndReload() {

        isAimingAllowed = false;
        pistolRotationPivot.DOLocalMove(pistolRotationPivotStartPosition, gunData.zoomInDuration / 1.5f);//3
        mainCamera.DOPause();
        isAiming = false;
        transform.localEulerAngles -= new Vector3(0, 0, gunData.zChangeForAiming);//here
        pistolRotationPivot.localEulerAngles = new Vector3(0, -5, 0);
        StartCoroutine(DecreaseFOVAndAddUI(gunData.zoomInDuration / 1.5f));
        yield return new WaitForSeconds(gunData.zoomInDuration / 1.5f);
        Reload();
        isAimingAllowed = true;
    }

    protected void ChangeDisplayAmmo() {
        OnAmmoChange?.Invoke(currentAmmo, extraAmmo);
    }
    protected abstract void StartReloadAnimation();

    protected IEnumerator ChangeStateAfterTime(float time, States targetState) {
        yield return new WaitForSeconds(time);
        state = targetState;
        OnStateChange?.Invoke();
        OnStateChange = null;
    }

    //protected Vector3 AimAtTarget(){
    //    RaycastHit info;
    //    Vector3 targetPosition;

    //    if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out info, gunData.range))
    //        targetPosition = info.point;
    //    else
    //        targetPosition = mainCamera.transform.position + mainCamera.transform.forward * gunData.range;


    //    float spreadPercentage = 1 + spreadMultiplier / 100.0f;
    //    var spread = mainCamera.transform.rotation * new Vector3(UnityEngine.Random.Range(-gunData.spreadFactorX * spreadPercentage, gunData.spreadFactorX * spreadPercentage), UnityEngine.Random.Range(-gunData.spreadFactorY * spreadPercentage, gunData.spreadFactorY * spreadPercentage), 0);
    //    targetPosition += spread;

    //    return (targetPosition - muzzle.position).normalized;
    //}

    protected Vector3 AimAtTarget(){
        RaycastHit info;
        Vector3 targetPosition;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out info, gunData.range,mask))
            targetPosition = info.point;
        else
            targetPosition = mainCamera.transform.position + mainCamera.transform.forward * gunData.range;

        float spreadPercentage = 1 + spreadMultiplier / 100.0f;

        float randomX = UnityEngine.Random.Range(0, 2);
        float randomY = UnityEngine.Random.Range(0, 2);

        float newX = ((randomX == 0) ? -1 : 1)* UnityEngine.Random.Range(gunData.spreadFactorXMin * spreadPercentage, gunData.spreadFactorX * spreadPercentage);
        float newY = ((randomY == 0) ? -1 : 1)* UnityEngine.Random.Range(gunData.spreadFactorYMin * spreadPercentage, gunData.spreadFactorY * spreadPercentage);

        var spread = mainCamera.transform.rotation*new Vector3(newX,newY,0);

        //var spread = mainCamera.transform.rotation * (new Vector3(UnityEngine.Random.Range(-gunData.spreadFactorX * spreadPercentage, gunData.spreadFactorX * spreadPercentage), UnityEngine.Random.Range(-gunData.spreadFactorY * spreadPercentage, gunData.spreadFactorY * spreadPercentage), 0));
        targetPosition += spread;

        return (targetPosition - muzzle.position).normalized;
    }

    protected void DecreaseSpreadMultiplier() { 
        spreadMultiplier = Mathf.Max(spreadMultiplier - gunData.spreadDecreaseRate * Time.deltaTime, 0);
        OnSpreadChange?.Invoke(1 + spreadMultiplier / 100);
    
    }

    protected void AddSpread() {
        if (!isAiming)
            spreadMultiplier += gunData.spreadPercentageMultiplier;
        else
            spreadMultiplier += gunData.spreadPercentageMultiplierAim;
        OnSpreadChange?.Invoke(1 + spreadMultiplier / 100);
    }

    protected void AddRecoil(){
        if (!isAiming)
            recoilTargetRotation += new Vector3(gunData.recoilHipFire.x, 0, 0);
        else
            recoilTargetRotation += new Vector3(gunData.recoilAim.x, 0, 0);
    }

    protected void DecreaseRecoilRotation() {
        recoilTargetRotation = Vector3.Lerp(recoilTargetRotation, Vector3.zero, gunData.returnSpeed * Time.deltaTime);
        recoilPivot.localEulerAngles = recoilTargetRotation;
    }

    //shooting to fast makes bullets fire in the wrong diretion when zoomed in
    //probably bcz the raycast hits the gun
    //replace everything weird with the tweens
}
