using System.Collections.Generic;
using System.Linq;

namespace ETHotfix {
public class ConfigData {

    public TalkConfig TalkConfig { get; }
    public SectionConfig SectionConfig { get; }
    public UnitConfig UnitConfig { get; }
    public DateConfig DateConfig { get; }
    public TitleConfig TitleConfig { get; }
    public AnimationConfig AnimationConfig { get; }
    public NewsConfig NewsConfig { get; }
    public CgConfig CgConfig { get; }
    public TipsConfig TipsConfig { get; }
    public ConstantConfig ConstantConfig { get; }

    private Dictionary<string, IConfig> _configs;
    public List<IConfig> Configs => _configs.Values.ToList();

    public ConfigData() {
        _configs = new Dictionary<string, IConfig>();

        TalkConfig = new TalkConfig();
        _configs.Add(TalkConfig.ConfigFileName.ToLower(), TalkConfig);

        SectionConfig = new SectionConfig();
        _configs.Add(SectionConfig.ConfigFileName.ToLower(), SectionConfig);

        UnitConfig = new UnitConfig();
        _configs.Add(UnitConfig.ConfigFileName.ToLower(), UnitConfig);

        DateConfig = new DateConfig();
        _configs.Add(DateConfig.ConfigFileName.ToLower(), DateConfig);

        TitleConfig = new TitleConfig();
        _configs.Add(TitleConfig.ConfigFileName.ToLower(), TitleConfig);

        AnimationConfig = new AnimationConfig();
        _configs.Add(AnimationConfig.ConfigFileName.ToLower(), AnimationConfig);

        NewsConfig = new NewsConfig();
        _configs.Add(NewsConfig.ConfigFileName.ToLower(), NewsConfig);

        CgConfig = new CgConfig();
        _configs.Add(CgConfig.ConfigFileName.ToLower(), CgConfig);

        TipsConfig = new TipsConfig();
        _configs.Add(TipsConfig.ConfigFileName.ToLower(), TipsConfig);

        ConstantConfig = new ConstantConfig();
        _configs.Add(ConstantConfig.ConfigFileName.ToLower(), ConstantConfig);
    }

    public bool Parse(byte[] bytes, string configFileName) {
        if (_configs.ContainsKey(configFileName)) {
            var iConfig = _configs[configFileName];
            iConfig.Deserialize(bytes);
            return true;
        }
        return false;
    }
}
}
