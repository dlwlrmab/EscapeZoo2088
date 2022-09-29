# EscapeZoo2088

* Unity 2020.1.1f
* 홍혜령, 박상훈, 박해찬

## Scene FlowChart

1. LoginScene
   * ID, PW 입력 후 로그인 버튼 클릭하면 **서버로 요청**
   * 로그인 응답 성공 시 LobbyScene 이동
2. LobbyScene
   * 캐릭터(동물) 및 맵 선택 후 준비 버튼 클릭하면 **서버로 요청**
   * 준비 응답 성공 시 팀(5명) 매칭을 기다리며, 팀 구성이 완료되면 호스트가 시작 버튼 클릭하여 **서버로 요청**
   * 시작 응답 성공 시 IngameScene 이동
3. IngameScene
   * 로딩(스토리) > 인게임 > 엔딩
   * 엔딩까지 완료되면 LobbyScene으로 돌아감

## Asset

> 샘플 프로젝트에 있던 Script, prefab 등 파일은 **script(prefab)\Legacy** 파일로 옮겨 놓음

> 서버에 연결하지않고 테스트 진행하기위해 서버 요청/응답이 필요한 모든 함수는 성공으로 간주하고 바로 실행되도록 되어있음

### Script

* GlobalData.cs: 게임에 필요한 데이터들 임시 저장
* SceneLoadManager.cs(Singleton): 씬 이동, fadeout/in 효과
* LoginScene.cs: 로그인
* LobbyScene.cs: 캐릭터 및 맵 선택, 매치메이킹
* IngameScene.cs: 로딩(스토리), 맵, 엔딩

### prefab
* player.prefab: 플레이어 프리팹

### Scene
* IngameScene 안의 Map의 자식오브젝트로 설정(프리팹 생성 시 자식 오브젝트 설정)

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
