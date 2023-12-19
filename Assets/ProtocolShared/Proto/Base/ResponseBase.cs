#nullable enable

namespace ProtocolShared.Proto.Base
{
    [System.Serializable]
    public class ResponseBase
    {
        public ResCode resCode { get; set; } = ResCode.Failed;
    }

    [System.Serializable]
    public class ResponseData<T> : ResponseBase
    {
        public T? data { get; set; }
    }
}
