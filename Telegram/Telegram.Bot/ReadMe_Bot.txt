1.bot 생성
	BotFather를 찾아 대화 상대 추가
	'/start' 입력
	'/newbot' 입력하여 절차에 따라 새로운 bot 생성 / 생성 후 나오는 HTTP API를 저장해 놓는다   ex)12345678:abcdefghijklmn

2.bot 사용
	1.핸드폰 텔레그램에서 bot에게 말을건다.
	2.https://api.telegram.org/bot12345678:abcdefghijklmn/getUpdates
		위에 주소( 'https://api.telegram.org/bot' + HTTP API + '/getUpdates')로 접속하면 CHAT ID를 찾을 수 있다. 크롬 권장
	3.https://api.telegram.org/bot12345678:abcdefghijklmn/sendMessage?chat_id=173075344&text=Hello
		위에 주소( 'https://api.telegram.org/bot' + HTTP API + '/sendMessage?chat_id=' + CHAT ID + '&text=Hello') 로 메시지가 전송 되는지 테스트

	4.클래스 생성 
	  Telegram.TelegramBot(HTTP API, CHAT ID);



