namespace ETHotfix
{
    /// <summary>
    /// 每个Config的基类
    /// </summary>
    public interface IConfig {
        string ConfigFileName {
            get;
        }
        void Deserialize(byte[] json);
    }
}