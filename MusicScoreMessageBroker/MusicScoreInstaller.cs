using UnityEngine;
using Zenject;
using UniRx;

namespace MusicScoreMessageBroker{

    public class MusicScoreInstaller : MonoInstaller<MusicScoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMessagePublisher>().WithId(MSBKType.Tick).To<TickMessageBroker>().AsSingle();
            Container.Bind<IMessageReceiver>().WithId(MSBKType.Tick).To<TickMessageBroker>().AsSingle();
            Container.Bind<IMessagePublisher>().WithId(MSBKType.FourBars).To<FourBarsMessageBroker>().AsSingle();
            Container.Bind<IMessageReceiver>().WithId(MSBKType.FourBars).To<FourBarsMessageBroker>().AsSingle();
            Container.Bind<IMessagePublisher>().WithId(MSBKType.Bar).To<BarMessageBroker>().AsSingle();
            Container.Bind<IMessageReceiver>().WithId(MSBKType.Bar).To<BarMessageBroker>().AsSingle();
            Container.Bind<IMessagePublisher>().WithId(MSBKType.QuarterNote).To<QuarterNoteMessageBroker>().AsSingle();
            Container.Bind<IMessageReceiver>().WithId(MSBKType.QuarterNote).To<QuarterNoteMessageBroker>().AsSingle();
            Container.Bind<IMessagePublisher>().WithId(MSBKType.SixTeensNote).To<SixteensNoteMessageBroker>().AsSingle();
            Container.Bind<IMessageReceiver>().WithId(MSBKType.SixTeensNote).To<SixteensNoteMessageBroker>().AsSingle();
        }
    }
}