using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    /* 해당 총기의 총기 소음 객체 */
    private AudioSource gunAudioSource;

    /* 
     * 1초에 1씩 FireRate를 깍음
     * currentFireRate가 0이되면 발사되도록 함
     * */
    private float currentFireRate;

    void Start()
    {
        gunAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
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
    
    // 총알을 발사 했을 때의 함수
    private void Fire()
    {
        /* 한 발 연사 시간 초기화 */
        currentFireRate = currentGun.fireRate;
        /* 총알 발사 이후 함수 실행 */
        Shoot();
    }

    // 총알 발사 이후 함수
    private void Shoot()
    {
        /* 발사와 동시에 총기 소음 실행 */
        PlaySE(currentGun.fireSound);
        /* 해당 Particle System 실행 */
        currentGun.muzzleFlash.Play();
        Debug.Log("Shooting");
    }

    // 총기 소리 실행 함수
    private void PlaySE(AudioClip _clip)
    {
        /* 총기 발사 소음 입력 */
        gunAudioSource.clip = _clip;
        /* 총기 소음을 실행함 */
        gunAudioSource.Play();
    }
}
