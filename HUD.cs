using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 필요한 컴포넌트들
    [SerializeField]
    /* 플레이어가 사용하는 건 컨트롤러 객체 */
    private GunController theGunController;
    /* 현재 스크립트에서 사용할 건 객체 */
    private Gun currentGun;

    [SerializeField]
    /* HUD 객체 */
    private GameObject goBulletHUD;

    [SerializeField]
    /* 총알 개수 반영 텍스트 */
    private Text[] textBullet;

    // Update is called once per frame
    void Update()
    {
        checkBullet();
    }

    private void checkBullet()
    {
        currentGun = theGunController.GetGun();
        textBullet[0].text = currentGun.carryBulletCount.ToString();
        textBullet[1].text = currentGun.reloadBulletCount.ToString();
        textBullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
