# 예르시아 (Yersia)

![Yersi4조 전광판](https://github.com/Sangyeop-Lim/GIF/blob/main/Yersi4%EC%A1%B0_%EC%A0%84%EA%B4%91%ED%8C%90.gif?raw=true)

"고대의 마법과 잊혀진 전설이 깨어나는 순간,  
당신의 모험은 이제 막 시작된다."

---

## 📑 목차
📌 [게임 정보](#-게임-정보)  
🧙‍♂️ [프로젝트 개요 및 목표](#-프로젝트-개요-및-목표)  
📖 [세계관](#-세계관)  
🎬 [게임 플레이 영상](#-게임-플레이-영상)  
🎮 [게임 특징](#-게임-특징)  
🗺️ [게임 흐름](#-게임-흐름)  
🕹️ [조작법](#-조작법)  
🖼 [게임 미리보기](#-게임-미리보기)  
📂 [프로젝트 구조](#-프로젝트-구조)  
🔮 [기술적 도전 과제](#-기술적-도전-과제)  
⚙️ [사용된 기술 스택](#-사용된-기술-스택)  
📝 [사용자 개선 사항](#-사용자-개선-사항)  
🏆 [프로젝트 결과 및 성과](#-프로젝트-결과-및-성과)  
👥 [만든 사람들](#-만든-사람들)  

---

## 📌 게임 정보

| 항목      | 내용                         |
|-----------|------------------------------|
| 게임명    | 예르시아 (Yersia)            |
| 장르      | 2D 턴제 RPG                   |
| 개발환경  | Unity 2022.3.17f              |
| 제작 기간 | 2025.06.20 ~ 2025.08.12       |

---

## 🧙‍♂️ 프로젝트 개요 및 목표

- 2000년대 추억의 MMORPG ‘아르피아’ 감성을 트렌드에 맞게 부활시킨 따뜻하고도 미스터리한 마법학교 배경  
- 남녀노소 누구나 즐길 수 있으며, 팬들에게 향수를 자극하는 것이 목표  
- 미스터리한 학교 생활(탐험), 원소 마법 습득(스킬 트리), NPC와 대화 시스템 중심  
- Isometric 2D 쿼터뷰, 싱글 플레이  
- 과거의 향수를 간직한 유저에게는 익숙함을, 새로운 유저에게는 신선함 제공  

---

## 📖 세계관  

예르시아 마법학교는 수 세기 동안 전설적인 마법사를 배출해온 명문 학교입니다.  
고대 봉인의 균열로 몬스터들이 나타나면서 학교와 주변 세계는 혼란에 빠집니다.  
마법사들은 ‘서클’이라 불리는 네 가지 속성별 마법 체계를 따르며 각각의 선생님에게서 전문적인 스킬을 배웁니다.  

- **서클 종류**  
  - 테럼 (자연)  
  - 글래큐어 (얼음)  
  - 이그누바 (화염)  
  - 불칸 (물리)  

주인공은 속성에 제한되지 않아 여러 서클의 선생님에게서 자유롭게 스킬을 배울 수 있습니다.  

---

## 🎬 게임 플레이 영상

[![게임 플레이 영상](https://img.youtube.com/vi/nTDZwvOsvkY/0.jpg)](https://www.youtube.com/watch?v=nTDZwvOsvkY)

---

## 🎮 게임 특징  

- **통합 월드 씬** – 허브와 필드를 한 장면에서 구현  
- **턴제 전투** – 전략적인 스킬과 아이템 활용  
- **NPC 상호작용** – 퀘스트 및 대화  
- **펫 시스템** – 전투 보조  
- **스킬 트리** – 노드 기반, 이전 스킬 습득 시 다음 해금  
- **스킬 강화** – 레벨업을 통한 효과 증대  

---

## 🗺️ 게임 흐름  

- **StartScene** – 타이틀 화면, 새 게임/불러오기, 기본 조작법 도움말  
- **IntroScene** – 세계관 소개 및 오프닝 연출  
- **WorldScene**  
  - 마법학교 허브: NPC 대화, 퀘스트 수주, 스킬 습득  
  - 필드 탐험: 지하 던전  
  - 전투 진입: BattleScene 전환  
- **BattleScene** – 턴제 전투, 보상 및 경험치 획득 후 WorldScene 복귀  

---

## 🕹️ 조작법  

![가이드](https://github.com/Sangyeop-Lim/GIF/blob/main/Guide.gif?raw=true)

| 키             | 기능                        |  
|----------------|-----------------------------|  
| W / A / S / D  | 캐릭터 이동                 |  
| 마우스 우클릭  | 캐릭터 이동 (WASD와 병행)   |  
| F              | 상호작용 / 대화             |  
| Esc            | 설정창 열기                 |  
| 마우스 좌클릭  | 선택                         |  

---

## 🖼 게임 미리보기  

**🎥 파티 시스템**  
![파티 시스템](https://github.com/Sangyeop-Lim/GIF/blob/main/Party.gif?raw=true)  

**🎥 상점 UI**  
![상점 UI](https://github.com/Sangyeop-Lim/GIF/blob/main/ShopUI.gif?raw=true)  

**🎥 스킬 학습 UI**  
![스킬 학습 UI](https://github.com/Sangyeop-Lim/GIF/blob/main/learnUI.gif?raw=true)  

**🎥 전투 시스템**  
![전투 시스템](https://github.com/Sangyeop-Lim/GIF/blob/main/Battle.gif?raw=true)  

---

## 📂 프로젝트 구조

```plaintext
Assets/
├── Animations/        # 애니메이션
├── Prefabs/           # NPC, 오브젝트, UI 프리팹
├── Resources/         # 공용 리소스
├── Scenes/            # Start, Intro, World, Battle
├── Scripts/           # 게임 로직 스크립트
├── Sounds/            # 배경음악 / 효과음
├── TextMesh Pro/      # UI 텍스트
```

---

## 🔮 기술적 도전 과제

- **URP Light 2D** – 2D 환경에서 실시간 조명과 분위기 연출  
- **SceneManager + 멀티 씬** – 씬 이동 시 데이터 초기화 없이 실시간 불러오기  
- **Cinemachine + Confiner2D** – 부드러운 카메라 추적과 이동영역 제한  
- **Animation Event** – 애니메이션과 스크립트 동기화, 스킬 이펙트 자연스러운 적용  

---

## ⚙️ 사용된 기술 스택

- **엔진 / 언어**: Unity 2022.3.17f1 (URP 2D Renderer), C#  
- **패키지 & 시스템**: New Input System, Cinemachine, TextMeshPro, URP 2D Light, DOTween, ProBuilder  
- **아키텍처 & 설계 패턴**: SRP, FSM, ScriptableObject Data-Driven, Observer & Event System, Object Pooling  
- **협업 & CI**: GitHub, Notion, Google Sheets  

---

## 📝 사용자 개선 사항

| 문제점 / 피드백 | 개선 방안 |
|-----------------|-----------|
| 전투와 대화 속도가 느림 | 캐릭터 기본 행동 속도 및 대사 출력 속도 조정 |
| 체력 회복 수단 부족 | 체력 회복 NPC 및 아이템 상점 추가 |
| 포탈 반복 사용 불편 | 제자리에서 포탈 상호작용 가능하도록 수정 |
| 인트로 BGM 및 스킵 기능 | 스타트, 인트로 씬에 전용 BGM과 스킵 기능 추가 |
| 전투 중 행동 게이지 정지 | 기능 점검 및 오류 수정 |

---

## 🏆 프로젝트 결과 및 성과

**결과**
- 일반 클래스로 데이터 저장 및 공유  
- 멀티 씬 활용으로 실시간 데이터 조작 가능  
- 이벤트 시스템 기반 옵저버 패턴 적용  
- JSON 파일 활용으로 텍스트 편집 용이  

**성과**
- 유저 테스트 후 개선 사항 반영  
- 다양한 에셋 활용 경험 및 Unity 최적화 숙달  
- 협업 과정에서 역할 분담, 일정 관리, 코드/리소스 병합 경험  

---

## 👥 만든 사람들  
테럼 : 백진환  
블로그 주소 : https://info8196.tistory.com/  
Github 주소 : https://github.com/JH696  

글래큐어 : 임상엽  
블로그 주소 : https://lim0210.tistory.com/  
Github 주소 : https://github.com/Sangyeop-Lim  
  
이그누바 : 손양복  
블로그 주소 : https://97926.tistory.com/  
Github 주소 : https://github.com/YBdhhh  

불칸 : 이선량  
블로그 주소 : https://05cm.tistory.com/  
Github 주소 : https://github.com/AgathaYi  
