using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    /* 무기 중복 교체 실행 방지 변수 */
    public static bool isChangeWeapon;
    /* 모든 무기가 사용할 수 있는 기본 컴포넌트 변수 */
    public static Transform currentWeapon;
    /* 현재 무기의 애니메이션 변수 */
    public static Animator currentWeaponAnim;
    [SerializeField]
    /* 현재 무기의 타입 변수 */
    private string currentWeaponType;

    [SerializeField]
    /* 현재 무기를 변경하는 시간 */
    private float changeWeaponDelayTime;
    [SerializeField]
    /* 다음 무기를 변경하는 시간 */
    private float changeWeaponEndDelayTime;

    /* Untiy 외부 차원의 무기 종류 변수 */
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    /* 각 무기들의 컨트롤러 객체 */
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;

    /* Unity 내부 차원의 접근이 용이한 Dictinary 객체 */
    // 총 무기+
    private Dictionary<string, Gun> gunDictionary;
    // 손 무기
    private Dictionary<string, Hand> handDictionary;

    void Start()
    {
        // Dictonary 초기화
        gunDictionary = new Dictionary<string, Gun>();
        handDictionary = new Dictionary<string, Hand>();

        for (int i = 0; i < guns.Length; i++)
            /* 모든 총기들을 이름으로 Dictionary에 입력 */
            gunDictionary.Add(guns[i].gunName, guns[i]);
        for (int i = 0; i < hands.Length; i++)
            /* 모든 손 무기들을 이름으로 Dictonary에 입력 */
            handDictionary.Add(hands[i].handName, hands[i]);

    }

    // Update is called once per frame
    void Update()
    {
        commandChangeWeapon();
    }

    private void commandChangeWeapon()
    {
        if(!isChangeWeapon)
        {
            // 해당 키를 눌렀을 때 무기 변경
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(changeWeaponCoroutine("HAND", "Hand_Default"));
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(changeWeaponCoroutine("GUN", "SubMachineGun01"));

        }
    }

    // 무기 변경 함수
    private void WeaponChange(string _type, string _name)
    {
        /* 타입에 따라 해당 컨트롤러의 Change 함수 실행 */
        switch(_type)
        {
            case "GUN":
                theGunController.GunChange(gunDictionary[_name]);
                break;
            case "HAND":
                theHandController.HandChange(handDictionary[_name]);
                break;
        }
    }


    private void CancelPreWeaponAction()
    {
        /* 타입에 따라 해당 컨트롤러의 무기 액션 취소 함수 실행 */
        switch (currentWeaponType)
        {
            /* 총기 무기 변경 시, 정조준 취소, 총기 사용 취소 */
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CanelReload();
                GunController.isActivate = false;
                break;
            /* 손 무기 변경 시, 손 무기 사용 취소 */
            case "HAND":
                HandController.isActivate = false;
                break;
        }
    }

    // 총기 변경 코루틴 함수
    public IEnumerator changeWeaponCoroutine(string _type, string _name)
    {
        /* 무기 변경 시작 및 애니메이션 시작 */
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");
        /* 변경 시간동안 대기 */
        yield return new WaitForSeconds(changeWeaponDelayTime);

        /* 이전 무기 실행 정지 */
        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        /* 무기 변경 완료되기 까지 대기 */
        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        /* 현재 타입 변경 */
        currentWeaponType = _type;
        /* 무기 변경 종료 */
        isChangeWeapon = false;
    }

}
