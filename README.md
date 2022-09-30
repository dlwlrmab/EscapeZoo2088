# EscapeZoo2088

* Unity 2020.1.1f
* 홍혜령, 박상훈, 박해찬

## Scene FlowChart

> 서버에 연결하지 않고 테스트 진행하기 위해 서버 요청/응답이 필요한 모든 함수는 성공으로 간주하고 실행되도록 되어있음

1. LoginScene
   * ID 입력 후 로그인 버튼 클릭하면 서버로 요청
   * 로그인 응답 성공 시 LobbyScene 이동
2. LobbyScene
   * 캐릭터(동물) 및 맵 선택 후 준비 버튼 클릭하면 서버로 요청
   * 준비 응답 성공 시 매칭 대기 화면으로 넘어가며 팀 구성을 기다림
   * 팀 구성이 완료되어 서버에서 시작 요청을 받으면 IngameScene 이동
3. IngameScene
   * **로딩(스토리) > [로딩(라운드) > 인게임] > 엔딩 > LobbyScene**
   * 입장 후 바로 서버에서 인게임에 필요한 데이터들을 보냄
   * 로딩(스토리)가 진행될 동안 인게임 로딩 및 입장을 완료하면 서버로 요청. 이때, 모두 입장 완료될 때까지 로딩(스토리)에서 넘어가지 않고 대기
   * 모든 플레이어가 입장을 완료하면 서버에서 시작 요청을 보내고 응답을 받아 로딩(라운드) 시작
   * 로딩(라운드)는 서버 요청 없이 자동으로 넘어가며 로딩(라운드)가 끝나면 인게임 시작. 이때, 로딩(라운드) > 인게임 구간은 라운드 수만큼 반복
   * 라운드를 클리어할 때마다 서버로 요청을 보냄
   * 서버에서는 다음 라운드가 있다면 RoundStart를 보내고 모든 라운드가 끝났다면 GameResult를 보냄
   * GameResult를 받았다면 엔딩 화면과 함께 로비로 다시 이동

## Asset

> 샘플 프로젝트에 있던 Script, prefab 등 파일은 **script(prefab)\Legacy** 파일로 옮겨 놓음

### Scripts
* Common
   * GlobalData: 게임에 필요한 데이터 저장
   * SceneLoadManager: 씬 이동, fadeout/in 기능, 로딩 기능
   * Rotate: 적용된 오브젝트 회전시키는 기능
* LobbyScene
   * LoginScene: 로그인
   * LobbyScene: 캐릭터 및 맵 선택, 매칭 대기 화면
* IngameScene
  * IngameScene: 인게임 전체 흐름 관리
  * Controller
     * IngamePacketHandler: 서버 요청 및 응답
     * IngameMapController: 맵 및 라운드 생성, 라운드 로드, 플레이어 스폰 위치, 라운드 설명 등 맵 및 라운드 관리
     * IngamePlayerController: 플레이어 생성, 라운드 준비(스폰), 라운드 시작 등 모든 플레이어 관리
     * IngameLoadingController: 스토리 로딩 화면, 라운드 로딩 화면
     * IngameEndingController: 엔딩(결과) 화면, 로비 이동
  * Player
     * Player: IngamePlayerController에서 사용되는 Base 스크립트, 모든 캐릭터가 공통 사용
     * PlayerMe: 내 플레이어를 관리, 키보드 이동
     * PlayerOhter: 다른 플레이어를 관리
  * Round
     * Round: IngameMapController에서 사용되는 Base 스크립트, 플레이어 스폰 위치나 라운드 설명 등을 저장
     * Round0~9: 각 라운드에서 사용될 스크립트, 실제 기믹 구현
  * 추가 구현 예정
     * UI
     * 팀 대전

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
