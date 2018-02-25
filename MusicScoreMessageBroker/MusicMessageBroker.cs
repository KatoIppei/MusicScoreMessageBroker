using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace MusicScoreMessageBroker
{
    /// <summary>
    /// 現在の楽譜の位置情報を追加するためのAbstractクラス
    /// </summary>
    abstract public class TimeStatus : MessageBroker
    {
        public CurrentMusicState CurrentMusicState;
    }

    /// <summary>
    /// 1Tickに1回発行する
    /// </summary>
    public class TickMessageBroker : TimeStatus { }

    /// <summary>
    /// 4小節に1回発行する
    /// </summary>
    public class FourBarsMessageBroker : TimeStatus { }

    /// <summary>
    /// 1小節に1回発行する
    /// </summary>
    public class BarMessageBroker : TimeStatus { }

    /// <summary>
    /// 4分音符に1回発行する
    /// </summary>
    public class QuarterNoteMessageBroker : TimeStatus { }

    /// <summary>
    /// 16分音符に1回発行する
    /// </summary>
    public class SixteensNoteMessageBroker : TimeStatus { }

}
