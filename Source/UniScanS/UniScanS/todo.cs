// < 완료 목록 >

//-BUG: 프로그램 종료시 MIL Buffer Free 오류
//-TODO: 모델 신규 생성부터 검사까지 동작되는지 확인 필요
//-TODO: CAM2 동시 동작 테스트
//-TODO: 모니터링이 켜져 있는 상태에서 검사 PC종료 후 재기동시 소켓 연결 여부
//-TODO: 검사PC가 켜져 있는 상태에서 모니터링 종료 후 재기동시 소켓 연결 여부
//-TODO: Change User
//-TODO: IUserChanged 인터페이스 구현
//-TODO: SheetCheckerParam 창의 Teach Tab -> Teach Tab / Margin Tab 분리
//-TODO: SheetCheckerParam 창의 Teach Tab을 권한에 따라 표시하도록 수정
//-TODO: [검사] 이미지 확대창 구현 -> 동작 확인 필요
//-TODO: Status 변경시점에 바로 Alive 패킷 날릴 수 있도록. Status Listener 구현
//-TODO: [모니터링 모델창] Cam1 / Cam2 이미지 2개 표시
//-BUG: [모니터링] 이미지 클릭시 다운?
//-BUG: 모델 선택시 티칭/검사 여부 메시지 박스 제거
//-TODO: [모니터링] Cam1 / Cam2 카운트 구분
//-TODO: [모니터링] 카메라 이미지 클릭시 카메라 화면 표시
//-TODO: CAM1 / CAM2는 Title바를 변경
//-BUG: 신규 모델 등록 후, 모델 티칭 여부 확인용 프로토콜 추가 필요.
//-BUG: 원격 티칭 종료시 Disable 해제 안됨

// < Curtis Lee>



// < 정해민 >
//TODO: 자동 네트웍 로그온
//TODO: Release 실행 설치
//TODO: [모니터링] Pause상태에서 목록 선택시 검사결과 이미지와 불량 목록 표시
//TODO: UserManager 연동
//-TODO: [검사] Sheet 목록 다중 선택시 불량 분포 표시 기능 확인
//-TODO: [검사] Sheet 목록 단일 선택시 불량 위치 표시 기능 확인

//TODO: Warning 제거
//TODO: 조명 컨트롤
//TODO: 한글화 작업

//TODO: 파라미터 Threshold 조정바를 컬러 보면서 범위 설정할 수 있도록 ( Range Slider )

// 6/13 (화)
//-TODO: 모니터링 메인에 Type 정보 표시
//-TODO: 모델 리스트 소문자 입력 시 자동 서치 기능 안됨  -> 대문자로 통일
//-TODO: 모델 검색 창 자동 serach 기능 구현 x
//-TODO: 그랩 리스트 이미지 가지고 있기
//-TODO: type 변경할 시 "Auto Teach 다시하세요" 창 띄우기
//TODO: SheetResultGrid에서 Index > 10 때 이미지 뷰어 버튼 표시안되도록
//TODO: SheetResultGrid에서 선택 변경 시 ImageViewer 닫기
//TODO: Start시 ImageViewer 버튼 표시 안되고 Dialog 닫고, Stop이나 Pause일경우에만 표시함

//TODO: Type Preset 만들기(아마 타입_두께별) - 버튼명 Load Template, Save Template
//TODO: 모델 : 폭 조절하기? - 김민정 선임기준으로


// < 송현석 >
//TODO: 간이 메뉴얼 작성(5/6)
//TODO: 오퍼레이터 메뉴얼 작성