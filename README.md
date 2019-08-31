## Inspector 창 관련
	
	> Unity 내 Inspector 창에서 오른쪽 위 자물쇠 버튼을 누르면

	다른 오브젝트를 눌러도 Inspector 창이 변하지 않음
	
		> 다른 창도 마찬가지

## Particle System 컴포넌트

	> 게임 내의 광원 이펙트를 표현하기 위한 시스템

	> 광원의 속성 값을 정의해줄 수 있음

	> Looping : 광원 효과의 반복 설정

	> Play On Awake : 플레이를 시작할 때 광원 효과 시작

	> Start Lifetime : 광원이 생성되는 주기 시간

	> Duration : 광원 발사 주기
		
	> Start Speed : 광원이 퍼지는 속도

	Emission 속성

		> Rate over Time : 광원의 발사 횟수를 정의

		> Bursts : 광원 발사 개수등 을 설정 가능
	
	Shape 속성

		> Shape : 광원의 퍼지는 모습을 설정
			
		> Radius : 광원이 퍼지는 반경 설정 가능 ( 구 내에서 랜덤하게 생성 )

	Color over Lifetime

		> 광원의 색을 점점 옅어지게 할 수 있음

	Size over Lifetime

		> 광원의 크기를 시간이 지남에 따라 설정 가능

	Texture Sheet Animation

		> Mode 속성을 통해(Sprite)  광원에 Texture를 설정할 수 있음

		> 총 발사광의 현실성을 위해 흐트러짐을 표현 가능 ( 맞는 텍스쳐가 있을 때 )
		
		> Mode를 Sprite로 설정한 뒤, Texture를 설정하고, Renderer에서 Meterial 값을 

		Sprites-Default로 설정하면 흐트러짐을 표시할 수 있음

	Renderer

		> 광원의 퍼지는 모양을 정의할 수 있음

	* Particle System 오브젝트를 결합해 특이한 광원의 형태를 표현할 수도 있음

## Audio Source 컴포넌트
	
	> 해당 오브젝트에 소리를 넣을 설정할 수 있는 컴포넌트

	> Play On Awake : 플레이를 시작할 때 소리 실행 시작

## Particle System

	> Gravity Modifier : 광원에 중력을 설정 가능

	Trail

		> 광원의 이동하는 부분을 따라 잔상이 남게 함

		> Renderer 속성에서 Trail Material을 정의해줘야 함

		> Ratio : 모든 광원에 잔상이 남거나 남지 않게 정의 가능

		> Lifetime : 광원 잔상이 남는 시간을 정의

		> Width over Trail : 잔상의 가로폭 길이를 정의

## C# Script 부분

## [HideInInspector]

	> Unity의 Inspector 창에서 해당 변수가 표시되지 않도록 막을 수 있음

## Instantiate 함수

	> Prefab 오브젝트, 또는 Scene상 오브젝트를 복사해 특정 위치, 각도로 생성할 수 있음

	> RaycastHit를 통해 총알이 피격된 오브젝트의 표면 방향으로 피격 이펙트가 발생하도록 설정할 수 있음

	> Instantiate(hitEffectPrefab(피격 효과 오브젝트), hitInfo.point(피격된 위치), Quaternion.LookRotation(hitInfo.normal)(피격된 위치에서의 피격된 오브젝트의 표면방향));

## Destroy 함수
	
	> 생성되어 있는 오브젝트를 특정 시간 이후에 삭제할 수 있는 함수

	> Destroy(clone(오브젝트 객체), 2f) -> clone 오브젝트를 2초 뒤에 삭제

## Canvas

	> Unity 내에서 모든 UI를 사용할 수 있도록 해주는 오브젝트

	> Render Mode : 화면 동기화 관련

		> Screen Space - Overlay를 통해 카메라 화면에 UI를 덮어 씌울 수 (오버레이)있음

		> World Space - 절대 좌표에 UI를 위치시킬 수 있음

	> Render Camera : 특정 카메라에 Canvas 오브젝트를 동기화 할 수 있음
	
	> Plane Distance : Render Camera를 통해 동기화 된 UI 화면과 카메라 간의 거리를 조정 가능

		* Render Camera를 통해 UI를 정의할 경우, 오브젝트들에 의해 UI가 가려질 수 있음

## Image Texture
	
	> Texture Type : 텍스터 타입을 설정

		> 타입을 설정하지 않을 시, 다른 오브젝트 컴포넌트에서 사용할 수 없음

	> Default : 오브젝트의 기본적인 속성( 크기나 형식 등)을 설정
	
		> Max Size : 이미지의 최대 크기를 설정

		> 이미지의 실제 크기보다 작게 설정할 경우, 이미지가 잘리게 됨

## Text

	> UI 내에서 글자를 표시해주는 오브젝트

	> Raycast Target : Mouse Raycast와 관련해 반응하도록 하는 속성

	> Outline 컴포넌트

		> 텍스트 테두리에 새로운 선을 정의할 수 있음

## C# Script 부분

	* UI 관련 스크립트를 작성하고 싶은 경우, Unity 내부 UI 라이브러리를 가져와야 함

	* using UnityEngine.UI;

## CrossHair

	> 총알의 피격 위치를 제대로 정의하기 위한 크로스 헤어 스크립트

	> 크로스 헤어 상태에 따른 정확도와 애니메이션을 변경할 수 있도록 구현함
