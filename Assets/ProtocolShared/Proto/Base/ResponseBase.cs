#nullable enable

using Newtonsoft.Json;
using System;
using System.Net;

namespace ProtocolShared.Proto.Base
{
    [System.Serializable]
    public class EmptyRequest
    {
    }

    [System.Serializable]
    public class ResponseBase
    {
        public ResCode ResCode { get; set; } = ResCode.Failed;
    }

    [System.Serializable]
    public class ResponseData<T> : ResponseBase
    {
        public T? Data { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
