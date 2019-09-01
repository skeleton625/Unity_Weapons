using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    /* 크로스 헤어 애니메이션 객체 */
    private Animator anim;

    [SerializeField]
    /* 무기에 따른 크로스헤어 활성화 유무를 위한 오브젝트 객체 */
    private GameObject goCrossHairHUD;

    /* 상태에 따른 피격 정확도 */
    private float gunAccuracy;

    /* 각 함수들은 해당 상태에 따른 참, 거짓을 Animator에 전송하는 함수들 */
    public void WalkingAnimation(bool _flag)
    {
        /* WeaponManager를 통해 Aminator 관리 */
        WeaponManager.currentWeaponAnim.SetBool("Walk", _flag);
        anim.SetBool("Walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        /* WeaponManager를 통해 Aminator 관리 */
        WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
        anim.SetBool("Running", _flag);
    }

    // 경사면에서 미끄러질 때, 달리는 애니메이션을 동작시키지 않도록 새로 정의
    public void JumpingAnimation(bool _flag)
    {
        anim.SetBool("Running", _flag);
    }

    public void CrouchingAnimation(bool _flag)
    {
        anim.SetBool("Crouching", _flag);
    }
    public void FineSightAnimation(bool _flag)
    {
        anim.SetBool("FineSight", _flag);
    }

    /* 총을 발사할 때의 크로스 트리거 애니메이션 함수 */
    public void FireAnimation()
    {
        /* 걷고 있는 중 쏠 경우 */
        if (anim.GetBool("Walking"))
            anim.SetTrigger("Fire_Walk");
        /* 앉아 있는 상태에서 쏠 경우 */
        else if (anim.GetBool("Crouching"))
            anim.SetTrigger("Fire_Crouch");
        /* 정지한 상태에서 쏠 경우 */
        else
            anim.SetTrigger("Fire_Idle");
    }

    public float GetAccuracy(bool isFineSight)
    {
        /* 걷고 있는 중 쏠 경우 */
        if (anim.GetBool("Walking"))
            gunAccuracy = 0.08f;
        /* 앉아 있는 상태에서 쏠 경우 */
        else if (anim.GetBool("Crouching"))
            gunAccuracy = 0.02f;
        /* 정지한 상태에서 쏠 경우 */
        else if (isFineSight)
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.04f;
        return gunAccuracy;
    }
}
