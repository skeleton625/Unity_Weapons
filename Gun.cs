using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /* 총기 종류 */
    public string gunName;
    /* 총기 사정거리 */
    public float range;
    /* 총기 정확도 */
    public float accuracy;
    /* 총기 연사속도 */
    public float fireRate;
    /* 총기 재장전 시간 */
    public float reloadTime;
    /* 총기 피해량 */
    public int damage;
    /* 총기 탄알집의 전체 탄 수 */
    public int reloadBulletCount;
    /* 총기 탄알집의 현재 탄 수 */
    public int currentBulletCount;
    /* 최대 소유 가능 총알 개수 */
    public int maxBulletCount;
    /* 현재 소유하고 있는 총알 개수 */
    public int carryBulletCount;
    /* 반동 세기 */
    public float retroActionForce;
    /* 정조준시 반동 세기 */
    public float retroActionFineSightForce;
    /* 정조준시 시야 위치 */
    public Vector3 fineSightOriginPos;
    /* 총기 애니메이션 */
    public Animator anim;
    /* 
     * 총기 화염 이펙트 객체
     * 보통 특정 이펙트를 나타내기 위해 ParticleSystem을 사용함   
     */
    public ParticleSystem muzzleFlash;
    /* 총기 발사 소음 */
    public AudioClip fireSound;
}
