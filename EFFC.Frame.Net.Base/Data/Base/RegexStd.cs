using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace EFFC.Frame.Net.Base.Data
{
    /// <summary>
    /// 正則表達式標注化對象-利用線程防止匹配時間過長
    /// </summary>
    public class RegexStd:StandardData<Regex>
    {
        private int _timeout = 100;
        /// <summary>
        /// 設定匹配超時，單位為毫秒
        /// </summary>
        public int TimeOut
        {
            get
            {
                return _timeout;
            }
            set
            {
                this._timeout = value;
            }
        }

        public RegexStd(string partten, RegexOptions ro)
        {
            this.Value = new Regex(partten, ro);
        }
        public RegexStd(string partten)
        {
            this.Value = new Regex(partten);
        }
        public RegexStd(Regex r)
        {
            this.Value = r;
        }
        public RegexStd()
        {
        }

        public static RegexStd ParseStd(Regex r)
        {
            return new RegexStd(r);
        }
        public static RegexStd ParseStd(string partten)
        {
            return new RegexStd(partten);
        }
        string _input = "";
        MatchCollection _mc = null;
        bool _isMatch = false;
        bool _isEnd = false;
        Thread t1 = null;

        public Match Match(string input)
        {
            if (IsMatch(input))
            {
                return this.Value.Match(input);
            }
            else
            {
                return null;
            }
        }

        public string[] Split(string input)
        {
            return this.Value.Split(input);
        }

        public string Replace(string input, string repstr)
        {
            if (IsMatch(input))
            {
                return this.Value.Replace(input, repstr);
            }
            else
            {
                return "";
            }
        }

        public string Replace(string input, MatchEvaluator evaluator)
        {
            if (IsMatch(input))
            {
                return this.Value.Replace(input, evaluator);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 匹配出input中符合規則的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MatchCollection Matches(string input)
        {
            _mc = null;
            _isEnd = false;
            this._input = input;
            TimerCallback tcb = this.TimerStart;
            ThreadStart ts = new ThreadStart(this._Matches);
            t1 = new Thread(ts);
            t1.Name = "RegexStdMarthes";
            t1.Start();
           // Timer ter = new Timer(tcb, null, _timeout + 10, _timeout + 10);
            Thread.Sleep(_timeout);

            TimerStart(null);
            return _mc;
        }
        protected void _Matches()
        {
            MatchCollection mc = this.Value.Matches(_input);
            foreach (Match m in mc)
            {
                string a = m.Value;
            }
           
            _mc = mc;
        }
        /// <summary>
        /// 是否能匹配出input中符合規則的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsMatch(string input)
        {
            _isMatch = false;
            _isEnd = false;
            try
            {
                this._input = input;
                TimerCallback tcb = this.TimerStart;
                ThreadStart ts = new ThreadStart(this._IsMatch);
                t1 = new Thread(ts);
                t1.Name = "RegexStdIsMatch";
                t1.Start();
                //Timer ter = new Timer(tcb, null, _timeout + 10, _timeout + 10);
                Thread.Sleep(_timeout);

                TimerStart(null);
                return _isMatch;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void _IsMatch()
        {
            MatchCollection mc = this.Value.Matches(_input);
            foreach (Match m in mc)
            {
                string a = m.Value;
            }
            if (this.Value.IsMatch(_input))
            {
                _isMatch = true;
            }
            else
            {
                _isMatch = false;
            }
        }
        
        

        protected void TimerStart(object o)
        {
            if (t1!= null && t1.IsAlive)
            {
                _isEnd = true;

            }
        }
    }
}
