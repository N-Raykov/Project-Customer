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
    protected CameraControls cameraControls;
    public int currentAmmo { get; set; }
    protected Vector3 recoilTargetRotation = Vector3.zero;
    protected Vector3 pistolRotationPivotStartPosition;
    protected bool isAiming;
    protected float originalFOV;
    bool isAimingAllowed = true;

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
        cameraControls = mainCamera.GetComponent<CameraControls>();
        animator = transform.GetComponent<Animator>();
        OnAmmoChange?.Invoke(currentAmmo, extraAmmo);
        pistolRotationPivotStartPosition = pistolRotationPivot.localPosition;
        originalFOV = mainCamera.fieldOfView;
    }

    protected override void UpdateWithPause() {
        CheckForActions();
        DecreaseSpreadMultiplier();
        DecreaseRecoilRotation();
        //Debug.Log(recoilTargetRotation);
    }

    protected override void FixedUpdateWithPause() {

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
                pistolRotationPivot.DOLocalMove(pistolRotationPivotStartPosition, gunData.zoomInDuration);
                mainCamera.DOPause();
                isAiming = false;
                StartCoroutine(DecreaseFOVAndAddUI(gunData.zoomInDuration));
            }

        }
    }

    protected void Aim() {
        if (!isAiming && state != States.Reload&&isAimingAllowed) {
            pistolRotationPivot.DOPause();
            isAiming = true;
            pistolRotationPivot.DOLocalMove(gunData.targetPosition,gunData.zoomInDuration);
            StartCoroutine(IncreaseFOVAndRemoveUI(gunData.zoomInDuration));
        }
    }

    protected IEnumerator IncreaseFOVAndRemoveUI(float duration) {

        yield return new WaitForSeconds(duration / 2);
        if (isAiming) 
            mainCamera.DOFieldOfView(originalFOV / gunData.zoomInFactor, duration / 2);
        yield return new WaitForSeconds(duration / 4);
        if (isAiming)
            OnZoomChange?.Invoke(false);
        
    }

    protected IEnumerator DecreaseFOVAndAddUI(float duration) {
        yield return new WaitForSeconds(duration / 4);
        if (!isAiming) {
            mainCamera.DOFieldOfView(originalFOV, duration / 2);
            OnZoomChange?.Invoke(true);
        }
    }

    protected virtual void Shoot() {
        AddRecoil();
        StartShotAnimation();
        lastShotTime = Time.time;
        AddSpread();
        GameObject b = Instantiate(bullet, muzzle.position, mainCamera.transform.rotation);
        b.GetComponent<Bullet>().AddSpeed(AimAtTarget());
        b.GetComponent<Bullet>().damage = gunData.damage;
        Instantiate(muzzleFlash, muzzle.position, mainCamera.transform.rotation, muzzle);
        state = States.Shoot;
        currentAmmo--;
        OnAmmoChange?.Invoke(currentAmmo, extraAmmo);
        StartCoroutine(ChangeStateAfterTime(gunData.shotCooldown,States.Idle));
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
        pistolRotationPivot.DOLocalMove(pistolRotationPivotStartPosition, gunData.zoomInDuration / 1.5f);
        mainCamera.DOPause();
        isAiming = false;
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

    protected Vector3 AimAtTarget() {//cast from the camera
        RaycastHit info;
        Vector3 targetPosition;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out info, gunData.range)) 
            targetPosition = info.point;
        else
            targetPosition = mainCamera.transform.position + mainCamera.transform.forward * gunData.range;

        float spreadPercentage = 1 + spreadMultiplier / 100;
        targetPosition += new Vector3(UnityEngine.Random.Range(-gunData.spreadFactorX*spreadPercentage, gunData.spreadFactorX * spreadPercentage), UnityEngine.Random.Range(-gunData.spreadFactorY * spreadPercentage, gunData.spreadFactorY * spreadPercentage),0);

        return (targetPosition - muzzle.position).normalized;
    }

    protected void DecreaseSpreadMultiplier() { //depending on how big the spread multiplier is increase the time it takes to start the decrease
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
        //Debug.Log("working");
        //if (!isAiming)
        //    recoilTargetRotation += new Vector3(gunData.recoilHipFire.x, UnityEngine.Random.Range(-gunData.recoilHipFire.y, gunData.recoilHipFire.y), UnityEngine.Random.Range(-gunData.recoilHipFire.z, gunData.recoilHipFire.z));
        //else
        //   recoilTargetRotation += new Vector3(gunData.recoilAim.x, UnityEngine.Random.Range(-gunData.recoilAim.y, gunData.recoilAim.y), UnityEngine.Random.Range(-gunData.recoilAim.z, gunData.recoilAim.z));

        if (!isAiming)
            recoilTargetRotation += new Vector3(gunData.recoilHipFire.x, 0, 0);
        else
            recoilTargetRotation += new Vector3(gunData.recoilAim.x, 0, 0);

    }

    protected void DecreaseRecoilRotation() {//could add a similar system to the spread; dont move towards the center while shooting and start after you stop
        recoilTargetRotation = Vector3.Lerp(recoilTargetRotation, Vector3.zero, gunData.returnSpeed * Time.deltaTime);
        recoilPivot.localEulerAngles = recoilTargetRotation;
    }

    //less spread and recoil when zooming in

    //shooting to fast makes bullets fire in the wrong diretion when zoomed in
    //probably bcz the raycast hits the gun
    //replace everything weird with the tweens
}
