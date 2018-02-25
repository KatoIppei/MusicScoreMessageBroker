using UnityEngine;
using UniRx;
using Zenject;
using MusicScoreMessageBroker;

public class ReceiverText : MonoBehaviour
{
    //InjectからquarterNotePublisherをReceiveするIMessageReceiverを取得する。
    [Inject(Id = MSBKType.QuarterNote)]
    IMessageReceiver _quarterNoteReceiver;

    //SEを鳴らしたい場合は、AudioClipを宣言する。
    //AudioClip kickSound;

    private void Start()
    {
        //宣言したAudioClipにはっつける。
        //kickSound = Resources.Load<AudioClip>("Sounds/kick");

        //4分音符毎に発行する。
        _quarterNoteReceiver.Receive<QuarterNoteMessageBroker>()
            .Subscribe(_ =>
            {
                Debug.Log(_.CurrentMusicState);
                //BeatConductorのAudioSourceで鳴らす。
                //_.CurrentMusicState.SoundEffect.PlayOneShot(kickSound);
            }).AddTo(this);
    }
}
