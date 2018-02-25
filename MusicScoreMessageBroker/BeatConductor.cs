
using System.Threading;
using UnityEngine;
using UniRx;
using Zenject;
using System;

namespace MusicScoreMessageBroker
{

    [RequireComponent(typeof(AudioSource))]
    public class BeatConductor : MonoBehaviour
    {

        private const int SIXTEEN = 16;
        private const int FOUR = 4;
        private const int QUARTER = 4;
        private const int ZERO = 0;

        /// <summary>
        /// 変更不可・MsPerTick*_tickPerBar*60/1000の値を表示。
        /// </summary>
        public float BPM;

        /// <summary>
        /// 1Tickにかかるms数。
        /// </summary>
        public int MsPerTick = 50;

        /// <summary>
        /// 1小節あたりの分解能。
        /// </summary>
        public TickPerBar TickPerBar = TickPerBar._32;
        
        /// <summary>
        /// 4小節毎に発行する
        /// </summary>
        [Inject(Id = MSBKType.FourBars)]
        private IMessagePublisher _fourBarsPublisher;

        /// <summary>
        /// 1小節毎に発行する
        /// </summary>
        [Inject(Id = MSBKType.Bar)]
        private IMessagePublisher _barPublisher;

        /// <summary>
        /// 4分音符毎に発行する
        /// </summary>
        [Inject(Id = MSBKType.QuarterNote)]
        private IMessagePublisher _quarterNotePublisher;

        /// <summary>
        /// 16分音符毎に発行する
        /// </summary>
        [Inject(Id = MSBKType.SixTeensNote)]
        private IMessagePublisher _sixteenNotePublisher;

        /// <summary>
        /// tick毎に発行する
        /// </summary>
        [Inject(Id = MSBKType.Tick)]
        private IMessagePublisher _tickPublisher;

        private CurrentMusicState _currentMusicState = new CurrentMusicState();
        
        private Subject<Unit> _tickReactive = new Subject<Unit>();

        private bool isDestroyed = false;

        void Start()
        {
            //(音楽の)スコアクラスの初期化。
            _currentMusicState.Initialize();
            //SE用のAudioSourceを格納。
            _currentMusicState.SoundEffect = GetComponent<AudioSource>();
            //始めの1小節は不安定なので減らす。
            _currentMusicState.Bar--;

            //tickReactiveはメインスレッドで実施する。
            _tickReactive.ObserveOnMainThread()
                //始めの1小節は不安定なので発行しない。
                .Skip((int)TickPerBar)
                .Subscribe(_ =>
            {
                _currentMusicState.Tick++;
                if (_currentMusicState.Tick % ((int)TickPerBar) == 0)
                {
                    _currentMusicState.SixteensNote++;
                    if (_currentMusicState.Tick % (int)TickPerBar == 0)
                    {
                        _currentMusicState.Tick = 0;
                        _currentMusicState.SixteensNote = 0;
                        _currentMusicState.QuarterNote++;
                        if (_currentMusicState.QuarterNote % FOUR == 0)
                        {
                            _currentMusicState.Bar++;

                            _currentMusicState.QuarterNote = 0;
                            if (_currentMusicState.Bar % 4 == 0)
                            {
                                _currentMusicState.FourBars++;
                                _fourBarsPublisher.Publish(new FourBarsMessageBroker { CurrentMusicState = _currentMusicState });
                            }
                            _barPublisher.Publish(new BarMessageBroker { CurrentMusicState = _currentMusicState });
                        }
                        _quarterNotePublisher.Publish(new QuarterNoteMessageBroker { CurrentMusicState = _currentMusicState });
                    }
                    _sixteenNotePublisher.Publish(new SixteensNoteMessageBroker { CurrentMusicState = _currentMusicState });
                }
                _tickPublisher.Publish(new TickMessageBroker { CurrentMusicState = _currentMusicState });
            }).AddTo(this);

            //別スレッドで実施。
            Scheduler.ThreadPool.Schedule(() =>
            {
                //BeatConductorクラスが破棄されるまでループ。
                while (!isDestroyed)
                {
                    //指定期間スリープして待機(これでms単位ではあるが待機できる。）
                    //可能ならばμsレベルの待機をしたいが方法が分からない。
                    Thread.Sleep(TimeSpan.FromMilliseconds(MsPerTick));
                    //tickReactiveを発行する。
                    _tickReactive.OnNext(Unit.Default);
                }
            });

        }

        /// <summary>
        /// このクラスが破棄されたら実施する。
        /// </summary>
        private void OnDestroy()
        {
            //このクラスが作成した別スレッドに終了フラグを立てる。
            isDestroyed = true;
        }
    }
}
