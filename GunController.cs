using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;
    /* 
     * 1초에 1씩 FireRate를 깍음
     * currentFireRate가 0이되면 발사되도록 함
     * */
    private float currentFireRate;
    /* 해당 총기의 총기 소음 객체 */
    private AudioSource gunAudioSource;
    /* 재장전 중인 상태 여부 파악 변수 */
    private bool isReload;
    /* 정조준 중인 상태 여부 파악 변수 */
    private bool isFineSightMode;

    [SerializeField]
    /* 정조준 전 원래 위치 */
    private Vector3 originPos;
    /* 현재 화면 위치 */
    private Vector3 currentOriginPos;

    void Start()
    {
        /* .총기 발사음 오디오 함수 정의 */
        gunAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /* 총 발사 주기 계산 */
        GunFireRateCalc();
        /* 총 발사 시도 */
        TryFire();
        /* 재장전 시도 */
        TryReload();
        /* 정조준 시도 */
        TryFineSight();
    }

    private void GunFireRateCalc()
    {
        /* currentFireRate가 0 이상이면 매 프레임마다 currentFireRate를 감소시킴 */
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    // 총알 발사 시도 함수
    private void TryFire()
    {
        /* 
         * 누르고 있으면 발사되도록 함
         * currentFireRate가 0보다 작을 경우 발사되도록 함
         */
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    // 키를 통한 재장전 시도 함수
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload &&
            currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") || Input.GetButtonUp("Fire2"))
        {
            FineSight();
        }
    }

    // 총알을 발사 했을 때의 함수
    private void Fire()
    {
        /* 
         * 재장전 중인 상태가 아닐 경우
         * -> 총을 발사하거나, 재장전을 시도할 수 있음
         */
        if (!isReload)
        {
            /* 현재 장전된 탄 수가 남아 있고, 재장전 중인 상태가 아닐 경우 */
            if (currentGun.currentBulletCount > 0)
                /* 총 발사 이후의 함수 실행 */
                Shoot();
            else
                /* 그렇지 않을 경우 장전된 탄이 없으므로 재장전 실행 */
                StartCoroutine(ReloadCoroutine());
        }
    }

    // 총알 발사 이후 함수
    private void Shoot()
    {
        --currentGun.currentBulletCount;
        /* 한 발 연사 시간 초기화 */
        currentFireRate = currentGun.fireRate;
        /* 발사와 동시에 총기 소음 실행 */
        PlaySE(currentGun.fireSound);
        /* 해당 Particle System 실행 */
        currentGun.muzzleFlash.Play();
    }

    // 정조준 실행 함수
    private void FineSight()
    {
        /* 정조준 상태를 변경 */
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        /* */
        if(isFineSightMode)
        {
            currentOriginPos = currentGun.fineSightOriginPos;
        }
        else
        {
            currentOriginPos = originPos;
        }

        StartCoroutine(FineSightActivateCoroutine());
    }

    // 총기 소리 실행 함수
    private void PlaySE(AudioClip _clip)
    {
        /* 총기 발사 소음 입력 */
        gunAudioSource.clip = _clip;
        /* 총기 소음을 실행함 */
        gunAudioSource.Play();
    }

    // 재장전을 시도하는 함수
    IEnumerator ReloadCoroutine()
    {
        /* 현재 가지고 있는 탄이 있을 경우 */
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            /* 재장전 애니메이션을 실행 */
            currentGun.anim.SetTrigger("Reload");

            yield return new WaitForSeconds(currentGun.reloadTime);

            /* 재장전을 하기 위해 현재 장전된 탄 수를 현재 가지고 있는 탄 수로 귀화시킴 */
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;
            /* 현재 가지고 있는 탄 수가 탄알집 탄 수보다 클 경우 */
            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {   
                /* 재장전 탄 수만큼 현재 장전한 탄 수를 증가시킴 */
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                /* 가지고 있는 탄 수는 장전한 탄 수만큼 감소시킴  */
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            /* 현재 가지고 있는 탄 수가 탄알집 탄 수보다 작을 경우 */
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        /* 만일 현재 가지고 있는 탄이 없을 경우 */
        else
        {
            Debug.Log("소유한 탄이 없음");
        }
    }

    // 정조준 상태에 따른 코루틴 함수
    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentOriginPos, 0.2f);
            yield return null;
        }
    }
}
