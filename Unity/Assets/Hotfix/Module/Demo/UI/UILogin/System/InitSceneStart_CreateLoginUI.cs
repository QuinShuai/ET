using ETModel;

namespace ETHotfix {
    [Event(EventIdType.InitSceneStart)]
    public class InitSceneStart_CreateLoginUI : AEvent {
        public override void Run() {
            var t = UILoginFactory.Create().GetAwaiter();
            t.OnCompleted(() => {
                Game.Scene.GetComponent<UIComponent>().Add(t.GetResult());
            });
        }
    }
}
