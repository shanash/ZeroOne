namespace FluffyDuck.Util
{
    public interface IUserData
    {
        /// <summary>
        /// 사용자 데이터를 로드한 후 초기화 과정이 필요한 경우 호출
        /// </summary>
        void InitLoadData();
    }
}
