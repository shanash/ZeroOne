#nullable enable

using Newtonsoft.Json;
using System;
using System.Net;

namespace ProtocolShared.Proto.Base
{
    [Serializable]
    public class EmptyRequest
    {
    }

    //public enum RESPONSE_TYPE
    //{
    //    FAILED = 0,
    //    SUCCESS = 1,
    //    /// <summary>예외 사항 발생</summary>
    //    EXCEPTION = 2,
    //    EMPTY_BODY = 3,
    //    NULL_RESPONSE = 4,
    //    JSON_PARSE_FAILED = 5,
    //    DUPLICATION_REQUEST = 6,
    //    EXPIRED_ACCESS_TOKEN = 7,
    //    EXPIRE_REFRESH_TOKEN = 8,
    //    EXCEEDED_RETRY_COUNT = 9,
    //    VALIDATE_FAILED_ACCESS_TOKEN = 10,
    //    VALIDATE_FAILED_REFRESH_TOKEN = 11,
    //}

    [Serializable]
    public class ResponseBase
    {
        public RESPONSE_TYPE ResType { get; set; } = RESPONSE_TYPE.FAILED;
    }

    [Serializable]
    public class ResponseData<T> : ResponseBase
    {
        public T? Data { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
