namespace ProtocolShared.Proto.Base
{
    public enum ResCode
    {
        Failed = 0,
        Successed = 1,
        Exception = 2,
        EmptyBody = 3,
        NullResponse = 4,
        JsonParseFailed = 5,
        DuplicationRequest = 6,
        ExpiredAccessToken = 7,
        ExpiredRefrashToken = 8,
        ExceededRetryCount = 9,
        ValidateFailedAccessToken = 10,
        ValidateFailedRefreshToken = 11,

        // Http Status 값
        OK = 200, // 성공
        Created = 201, // 생성됨
        Accepted = 202, // 허용됨 (요청은 접수하였지만, 처리가 완료되지 않았다.)
        Non_AuthoritativeInformation = 203, //신뢰 할수 없는 정보
        NoContent = 204, // 처리는 성공하였지만 클라이언트에게 돌려줄 콘텐츠가 없다.
        ResetContent = 205, // 콘텐츠 재설정
        PartialContent = 206, // 콘텐츠의 일부만을 보낸다.
        Multi_Status = 207, // 처리 결과의 스테이터스가 여러개 이다.

        MultipleChoices = 300, // 선택 항목이 여러 개 있다.
        MovedPermanently = 301, // 지정한 리소스가 새로운 URI로 이동하였다.
        Found = 302, // 요청한 리소스를 다른 URI에서 찾았다.
        SeeOther = 303, // 다른 위치로 요청 하라
        NotModified = 304, // 마지막 요청 이후 요청한 페이지는 수정되지 않았다.
        UseProxy = 305, // 지정한 리소스에 엑세스 하려면 프록시를 통해야 한다.
        TemporaryRedirect = 307, // 임시로 리다이렉션 요청이 필요하다.

        BadRequest = 400, // 요청의 구문이 잘못되었다.
        Unauthorized = 401, // 지정한 리소스에 대한 액세스 권한이 없다.
        PaymentRequired = 402, // 지정한 리소스를 액세스하기 위해서는 결제가 필요하다.
        Forbidden = 403, // 지정한 리소스에 대한 액세스가 금지되었다.
        NotFound = 404, // 지정한 리소스를 찾을 수 없다.
        MethodNotAllowed = 405, // 요청한 URI가 지정한 메소드를 지원하지 않는다.
        NotAcceptable = 406, // 클라이언트가 Accept-* 헤더에 지정한 항목에 관해 처리할 수 없다.
        ProxyAuthenticationRequired = 407, // 클라이언트는 프록시 서버에 인증이 필요하다.
        RequestTimeout = 408, // 요청을 기다리다 서버에서 타임아웃하였다.
        Conflict = 409, // 서버가 요청을 수행하는 중에 충돌이 발생하였다.
        Gone = 410, // 지정한 리소스가 이전에는 존재하였지만, 현재는 존재하지 않는다.
        LengthRequired = 411, // 요청 헤더에 Content-Length를 지정해야 한다.
        PreconditionFailed = 412, // If-Match와 같은 조건부 요청에서 지정한 사전 조건이 서버와 맞지 않는다.
        RequestEntityTooLarge = 413, // 요청 메시지가 너무 크다.
        Request_URITooLarge = 414, // 요청 URI가 너무 길다.
        UnsupportedMediaType = 415, // 클라이언트가 지정한 미디어 타입을 서버가 지원하지 않는다.
        RangeNotSatisfiable = 416, // 클라이언트가 지정한 리소스의 범위가 서버의 리소스 사이즈와 맞지 않는다.
        ExpectationFailed = 417, // 클라이언트가 지정한 Expect 헤더를 서버가 이해할 수 없다.
        UnprocessableEntity = 422, // (WebDAV) 클라이언트가 송신한 XML이 구문은 맞지만, 의미상 오류가 있다.
        Locked = 423, // (WebDAV) 지정한 리소스는 잠겨있다.
        FailedDependency = 424, // (WebDAV) 다른 작업의 실패로 인해 본 요청도 실패하였다.
        UpgradedRequired = 426, // 클라이언트의 프로토콜의 업그레이드가 필요하다.
        PreconditionRequired = 428, // If-Match와 같은 사전조건을 지정하는 헤더가 필요하다.
        TooManyRequests = 429, // 클라이언트가 주어진 시간 동안 너무 많은 요청을 보냈다.
        RequestHeaderFieldsTooLarge = 431, // 헤더의 길이가 너무 크다.
        ConnectionClosedWithoutResponse = 444, // (NGINX) 응답을 보내지 않고 연결을 종료하였다.
        UnavailableForLegalReasons = 451, // 법적으로 문제가 있는 리소스를 요청하였다.

        InternalServerError = 500, // 서버에 에러가 발생하였다.
        NotImplemented = 501, // 요청한 URI의 메소드에 대해 서버가 구현하고 있지 않다.
        BadGateway = 502, // 게이트웨이 또는 프록시 역할을 하는 서버가 그 뒷단의 서버로부터 잘못된 응답을 받았다.
        ServiceUnavailable = 503, // 현재 서버에서 서비스를 제공할 수 없다.
        GatewayTimeout = 504, // 게이트웨이 또는 프록시 역할을 하는 서버가 그 뒷단의 서버로부터 응답을 기다리다 타임아웃이 발생하였다.
        HTTPVersionNotSupported = 505, // 클라이언트가 요청에 사용한 HTTP 버전을 서버가 지원하지 않는다.
        InsufficientStorage = 507, // (WebDAV) 서버에 저장 공간 부족으로 처리에 실패하였다.

        NeedCreateUser = 10001, // 유저를 생성해야 함.
        DuplicationName = 10002, // 유저를 생성해야 함.
        NotFoundItem = 10003, // 아이템을 찾을수 없음.
    }
}
