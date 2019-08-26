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
