# 예르시아 (Yersia)

![Yersi4조 전광판](https://github.com/Sangyeop-Lim/GIF/blob/main/Yersi4%EC%A1%B0_%EC%A0%84%EA%B4%91%ED%8C%90.gif?raw=true)

"고대의 마법과 잊혀진 전설이 깨어나는 순간,  
당신의 모험은 이제 막 시작된다."

---

## 📑 목차  
- [📌 게임 정보](#-게임-정보)  
- [📖 세계관](#-세계관)  
- [🎬 게임 플레이 영상](#-게임-플레이-영상)  
- [🖼 게임 미리보기](#-게임-미리보기)  
- [🗺️ 게임 흐름](#-게임-흐름)  
- [🎯 게임 특징](#-게임-특징)  
- [🕹️ 조작법](#-조작법)  
- [📂 프로젝트 구조](#-프로젝트-구조)  
- [👥 만든 사람들](#-만든-사람들)  

---

## 📌 게임 정보

| 항목      | 내용                         |
|-----------|------------------------------|
| 게임명    | 예르시아 (Yersia)            |
| 장르      | 2D 턴제 RPG                   |
| 개발환경  | Unity 2022.3.17f              |
| 제작 기간 | 2025.06.20 ~ 2025.08.12       |

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

대부분 마법사는 특정 서클에 재능과 친화력을 갖고 있지만,  
주인공은 속성에 제한되지 않아 여러 서클의 선생님에게서 자유롭게 스킬을 배울 수 있습니다.  

---

## 🎬 게임 플레이 영상

[![게임 플레이 영상](https://img.youtube.com/vi/nTDZwvOsvkY/0.jpg)](https://www.youtube.com/watch?v=nTDZwvOsvkY)

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

## 🗺️ 게임 흐름  
- **StartScene** – 타이틀 화면, 새 게임/불러오기, 기본 조작법 도움말  
- **IntroScene** – 세계관 소개 및 오프닝 연출  
- **WorldScene**  
  - 마법학교 허브: NPC 대화, 퀘스트 수주, 스킬 습득  
  - 필드 탐험: 지하 던전  
  - 전투 진입: BattleScene 전환  
- **BattleScene** – 턴제 전투, 보상 및 경험치 획득 후 WorldScene 복귀  

---

## 🎯 게임 특징  

- **통합 월드 씬** – 허브와 필드를 한 장면에서 구현  
- **턴제 전투** – 전략적인 스킬과 아이템 활용  
- **NPC 상호작용** – 퀘스트 및 대화  
- **펫 시스템** – 전투 보조  
- **스킬 트리** – 노드 기반, 이전 스킬 습득 시 다음 해금  
- **스킬 강화** – 레벨업을 통한 효과 증대  

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
