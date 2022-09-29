# EscapeZoo2088

* Unity 2020.1.1f
* 홍혜령, 박상훈, 박해찬

## Scene FlowChart

> 서버에 연결하지 않고 테스트 진행하기 위해 서버 요청/응답이 필요한 모든 함수는 성공으로 간주하고 실행되도록 되어있음

1. LoginScene
   * ID 입력 후 로그인 버튼 클릭하면 **서버로 요청**
   * 로그인 응답 성공 시 LobbyScene 이동
2. LobbyScene
   * 캐릭터(동물) 및 맵 선택 후 준비 버튼 클릭하면 **서버로 요청**
   * 준비 응답 성공 시 매칭 대기 화면으로 넘어가며 팀 구성을 기다림
   * 팀 구성이 완료되어 **서버에서 시작 요청**을 받으면 IngameScene 이동
3. IngameScene
   * 로딩(스토리) > 인게임 > 엔딩 > LobbyScene
   * 로딩(스토리)가 진행될 동안 인게임 로딩 및 입장을 완료하면 **서버로 요청**
   * 모든 플레이어가 동일한 맵순서로 진행해야하므로, 게임시작시 서버에서 맵목록 받아와서 로딩중에 클라에서 맵 생성
   * 모든 플레이어들이 입장 완료될 때까지 로딩 마지막 화면에서 인게임으로 넘어가지 않고 대기
   * 준비가 완료되어 **서버에서 시작 요청**을 받으면 인게임 시작

## Asset

> 샘플 프로젝트에 있던 Script, prefab 등 파일은 **script(prefab)\Legacy** 파일로 옮겨 놓음

### Scene
* DontDestroyOnLoad
   * GlobalData.cs: 게임에 필요한 데이터들 임시 저장
   * SceneLoadManager.cs: 씬 이동, fadeout/in 기능, 로딩 기능
* LoginScene
   * 로그인
* LobbyScene
   * 캐릭터 및 맵 선택
   * 매칭 대기 화면
* IngameScene
  * 컨트롤러: 오브젝트 스포너, 리소스 관리(맵, 오브젝트)
  * 맵: 배경
  * 캐릭터
  * 오브젝트(기믹)
  * 로딩(스토리)
  * 엔딩(결과)
  * UI

## Git Commit Message

* feat: 새로운 기능에 대한 커밋
* fix: 버그 수정에 대한 커밋
* build: 빌드 관련 파일 수정에 대한 커밋
* chore: 그 외 자잘한 수정에 대한 커밋
* ci: CI관련 설정 수정에 대한 커밋
* docs: 문서 수정에 대한 커밋
* style: 코드 스타일 혹은 포맷 등에 관한 커밋
* refactor: 코드 리팩토링에 대한 커밋
* test: 테스트 코드 수정에 대한 커밋
