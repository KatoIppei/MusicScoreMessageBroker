using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MusicScoreMessageBroker
{
    
    /// <summary>
    /// 現在の楽譜の位置情報
    /// </summary>
    public class CurrentMusicState
    {
        /// <summary>
        /// 4小節につき1つ増える。
        /// </summary>
        public int FourBars;
        /// <summary>
        /// 1小節につき1つ増える。
        /// </summary>
        public int Bar;
        /// <summary>
        /// 4分音符につき１つ増える。1小節進むと0に戻る。♪
        /// </summary>
        public int QuarterNote;
        /// <summary>
        /// 16分音符につき1つ増える。1小節進むと0に戻る。♬
        /// </summary>
        public int SixteensNote;
        /// <summary>
        /// 4分音符で480。分解能。4分音符進むと0に戻る。
        /// </summary>
        public int Tick;
        /// <summary>
        /// コードだよー。本当はEnumの方が好み。
        /// </summary>
        public string Code;

        /// <summary>
        /// SEを鳴らす用の汎用AudioSource
        /// </summary>
        public AudioSource SoundEffect;
        /// <summary>
        /// 値に-1を代入して初期化する。
        /// </summary>
        public void Initialize()
        {
            FourBars = -1;
            Bar = -1;
            QuarterNote = -1;
            SixteensNote = -1;
            Tick = -1;
            Code = "C";
        }
    }


    public enum MSBKType
    {
        FourBars = 81000,
        Bar = 81001,
        QuarterNote = 81002,
        SixTeensNote = 81003,
        Tick = 81004,
        Code = 81005
    }

    public enum TickPerBar
    {
        _16 = 16,
        _32 = 32,
        _48 = 48,
        _64 = 64,
        _96 = 96,
        _128 = 128
    }
}