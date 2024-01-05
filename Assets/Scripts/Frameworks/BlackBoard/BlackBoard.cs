using System.Collections.Generic;

namespace FluffyDuck.Util
{
    /// <summary>
    /// 본 클래스는 휘발성 데이터를 임시 저장하기 위한 클래스.
    /// 씬 이동시 등 임시로 데이터를 저장하고 삭제하기 위한 공간
    /// 게임이 종료되면 모든 데이터는 초기화됨.
    /// 메모리에 데이터를 저장하는 방식이기 때문에 사용처에 맞도록 사용해야 함
    /// </summary>
    public class BlackBoard : Singleton<BlackBoard>
    {
        Dictionary<string, object> Key_Values;

        BlackBoard() { }

        protected override void Initialize()
        {
            Key_Values = new Dictionary<string, object>();
        }

        protected override void OnDispose()
        {
            throw new System.NotImplementedException();
        }

        public void SetBlackBoard(BLACK_BOARD_KEY key, object v)
        {
            SetBlackBoard(key.ToString(), v);
        }

        public void SetBlackBoard(string key, object v)
        {
            if (Key_Values.ContainsKey(key))
            {
                Key_Values[key] = v;
            }
            else
            {
                Key_Values.Add(key, v);
            }
        }

        public object GetBlackBoardData(BLACK_BOARD_KEY key)
        {
            return GetBlackBoardData(key.ToString());
        }
        public object GetBlackBoardData(string key)
        {
            if (Key_Values.ContainsKey(key))
            {
                return Key_Values[key];
            }
            return null;
        }

        public void GetBlackBoardData<T>(BLACK_BOARD_KEY key, out T result)
        {
            result = (T)GetBlackBoardData(key.ToString());
        }

        public void GetBlackBoardData<T>(string key, out T result)
        {
            result = (T)Key_Values[key];
        }

        public T GetBlackBoardData<T>(BLACK_BOARD_KEY key)
        {
            return GetBlackBoardData<T>(key.ToString());
        }
        public T GetBlackBoardData<T>(string key)
        {
            T result = (T)Key_Values[key];
            return result;
        }
        public T GetBlackBoardData<T>(BLACK_BOARD_KEY key, object default_value)
        {
            return (T)GetBlackBoardData<T>(key.ToString(), default_value);
        }
        public T GetBlackBoardData<T>(string key, object default_value)
        {
            if (Key_Values.ContainsKey(key))
            {
                return (T)Key_Values[key];
            }
            else
            {
                return (T)default_value;
            }
        }

        public void RemoveBlackBoardData(BLACK_BOARD_KEY key)
        {
            RemoveBlackBoardData(key.ToString());
        }
        public void RemoveBlackBoardData(string key)
        {
            Key_Values.Remove(key);
        }
        public void ClearAllDatas()
        {
            Key_Values.Clear();
        }
    }
}

