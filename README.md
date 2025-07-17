# 🎣 Fishing Blast

블럭 퍼즐을 플레이하면서 물고기를 수집하는 게임

---

## 🧠 아키텍처 개요

- **의존성 주입 (DI):** [VContainer](https://github.com/hadashiA/VContainer) 사용
- **비동기 처리:** [UniTask](https://github.com/Cysharp/UniTask) 사용
- **모듈 분리:** 씬 기반 `LifetimeScope` 모듈 분리
- **게임 플로우:** 타이틀 → 플레이 → 종료/재시작 순환 구조
- **책임 분리 원칙:** Logic / View 계층 분리

---

## 🔁 전체 게임 플로우

```text
1. Init 씬: 플레이어 데이터 로딩 후 Title 씬 이동
2. Title 씬: Start 버튼 → Play 씬 이동
3. Play 씬:
   - 블럭 3개 생성
   - 유저가 블럭을 드래그해 보드에 배치
   - 줄이 맞으면 제거 + 점수
   - 획득한 점수에 비례한 레어도의 물고기 획득 (도감에서 획득한 물고기 확인 가능)
   - 블럭 모두 배치 시 다음 턴
   - 배치 불가능하면 게임 종료 → Title 씬
```

---

## 🧩 VContainer 구조

### AppLifetimeScope

* 앱 전체 공통 서비스 등록
* `SceneLoader`, `IAdsService`, `GameInitializer`

### TitleLifetimeScope

* 타이틀 화면 관련 서비스
* `TitleFlowController`

### PlayLifetimeScope

* 게임 플레이 관련 구성 요소 등록
* `BlockManager`, `BlockPresenter`, `BlockBoard`, `PlayFlowController`

---

## 🧠 주요 시스템 설명

### GameInitializer

* 게임 전역 초기화 (광고, 데이터 로딩 등)
* 모든 FlowController의 초기화 조건이 됨

### SceneLoader

* 씬 전환을 담당 (VContainer 포함)

---

## 🎮 Play 씬 핵심 구조

### 📌 PlayFlowController

* 게임 루프 관리
* 현재 블럭 드래그 가능 여부, 애니메이션 대기 여부 관리
* 게임 오버 판정

### 📌 BlockGenerator

* 배치 가능한 무작위 블럭 3개 생성
* 블럭 패턴과 가중치 기반 필터링

### 📌 BlockManager

* 현재 블럭 3개 상태 저장
* 블럭 제거 여부 판단, 사용 완료 여부 판단

### 📌 BlockBoard

* 보드 상태 관리 (배치 가능 여부, 매칭 체크, 게임 오버 판단)
* `CanPlace`, `Place`, `CheckLineClear`, `IsGameOver` 등

### 📌 BlockDragController

* 유저 드래그 입력 관리
* 드래그 중 프리뷰 표시 요청

### 📌 BlockPresenter

* 블럭 모델을 기반으로 View (BlockView) 생성
* `CreateAndShowBlocks`, `DestroyAllBlocks`

### 📌 BlockView

* 드래그 가능, 미리보기 표시 기능 포함
* 블럭 단위 연출, 클릭 이벤트 처리 등

---

## 🧱 클래스 간 관계

```text
[PlayFlowController]
      │
      ▼
[BlockManager] ─────→ [BlockBoard]
      │                    │
      ▼                    ▼
[BlockPresenter]    [BoardRenderer]
      │
      ▼
[BlockView] ←──── [BlockDragController]
```

---

## 🙋‍♀️ 기타 가이드

* 기능 추가 시 각 모듈 책임을 명확히 구분
* 로직은 가능한 한 View와 분리하여 테스트 가능하게 유지
* 새 씬 추가 시 `LifetimeScope` 반드시 정의

---

