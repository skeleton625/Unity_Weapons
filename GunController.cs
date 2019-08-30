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

    /* 일반 사격 시, 반동 위치 */
    private Vector3 recoilBack;
    /* 정조준 시, 반동 위치 */
    private Vector3 retroActionRecoilBack;
    /* 총격을 받은 오브젝트에 대한 정보 */
    private RaycastHit hitInfo;

    [SerializeField]
    /* 총알의 방향이 플레이어의 화면에 정중앙으로 설정하기 위함 */
    private Camera theCamera;

    [SerializeField]
    /* 총 피격에 대한 효과 오브젝트 */
    private GameObject hitEffectPrefab;

    void Start()
    {
        originPos = Vector3.zero;
        /* .총기 발사음 오디오 함수 정의 */
        gunAudioSource = GetComponent<AudioSource>();
        /* 총구가 x축 방향을 향하고 있기 때문에 x축의 위치를 변경시켜 줌 */
        /* 일반 사격 시, 반동 위치 정의 */
        recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        /* 정조준 시, 반동 위치 정의 */
        retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce,
                                                currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

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
            currentGun.currentBulletCount < currentGun.reloadBulletCount )
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void TryFineSight()
    {
        if(!isReload)
        {
            /* 정조준 버튼을 누르고 있을 때만 정조준이 되도록 함 */
            if (Input.GetButtonDown("Fire2") && !isReload)
            {
                FineSight();
            }
            /* 정조준 버튼을 땟을 때, 정조준 상태가 취소되도록 함 */
            else if(Input.GetButtonUp("Fire2") && !isReload)
            {
                CancelFineSight();
            }
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
            {
                CancelFineSight();
                /* 그렇지 않을 경우 장전된 탄이 없으므로 재장전 실행 */
                StartCoroutine(ReloadCoroutine());
            }
                
        }
    }

    // 총알 발사 이후 함수
    private void Shoot()
    {
        --currentGun.currentBulletCount;
        /* 한 발 연사 시간 초기화 */
        currentFireRate = currentGun.fireRate;
        /* 총격이 오브젝트에 맞는지 확인 */
        Hit();
        /* 발사와 동시에 총기 소음 실행 */
        PlaySE(currentGun.fireSound);
        /* 해당 Particle System 실행 */
        currentGun.muzzleFlash.Play();

        /* 정조준과 일반 조준의 반복문의 무한 반복을 막기 위함 */
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    // 쏘는 그대로 오브젝트에 맞도록 구현함
    /* 이외의 방법은 미리 생성한 다음 비활성화 하는 방법이 존재 있음 */
    private void Hit()
    {
        /* 레이저 발사 위치를 절대 좌표로 정의 -> 상대 좌표는 발사하는 위치가 변하지 않기 때문 */
        if(Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, currentGun.range))
        {
            /*
             * 피격된 위치에 효과 오브젝트가 생성되도록 함
             * RaycastHit.point -> 레이저와 충돌된 위치 좌표
             * RaycastHit.normal -> 레이저와 충돌된 위치의 표면 좌표
             * Quaternion.LooKRotation -> 매개변수가 가르키는 방향으로 회전한 값을 주는 함수
             */
            GameObject clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            /* 생성된 피격 효과 오브젝트가 2초 뒤에 삭제되도록 함 */
            Destroy(clone, 2f);
        }
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
            StopAllCoroutines();
            StartCoroutine(FineSightActCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactCoroutine());
        }
    }

    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
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
        if (currentGun.carryBulletCount > 0)
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
    IEnumerator FineSightActCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
        currentGun.transform.localPosition = currentGun.fineSightOriginPos;
    }

    IEnumerator FineSightDeactCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
        currentGun.transform.localPosition = originPos;
    }

    /* 총 발사에 대한 반동을 주는 함수 */
    IEnumerator RetroActionCoroutine()
    {
        /* 정조준 상태일 경우 */
        if(isFineSightMode)
        {
            /* 
             * 반동이 이뤄진 이후 중복을 막기 위해 처음 위치로 되돌림
             * > 연출 목적
             */
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작
            /* 특정 x축 위치를 지나면 반복문을 종료하도록 함 */
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                /* 반동 위치에 도달할 때까지 0.4f로 빠르게 이동 */
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원위치 이동
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                /* 도달한 반동 위치에서 원위치로 0.2f로 느리게 이동 */
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
                yield return null;
            }
        }
        else
        {
            /* 
             * 반동이 이뤄진 이후 중복을 막기 위해 처음 위치로 되돌림
             * > 연출 목적
             */
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            /* 특정 x축 위치를 지나면 반복문을 종료하도록 함 */
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                /* 반동 위치에 도달할 때까지 0.4f로 빠르게 이동 */
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원위치 이동
            while(currentGun.transform.localPosition != originPos)
            {
                /* 도달한 반동 위치에서 원위치로 0.2f로 느리게 이동 */
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
                yield return null;
            }
        }
    }
}
