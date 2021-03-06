using System;

namespace EFFC.Frame.Net.Base.Data
{
    public class Int64Std : StandardData<long>,IFormattable
    {

        public Int64Std()
        {
        }

        public Int64Std(long o)
        {
            this.Value = o;
        }

        public Int64Std(long? o)
        {
            if (o != null)
                this.Value = o.Value;
        }

        public static implicit operator Int64Std(double o)
        {
            return Int64Std.ParseStd(o);
        }
        public static implicit operator Int64Std(float o)
        {
            return Int64Std.ParseStd(o);
        }
        public static implicit operator Int64Std(decimal o)
        {
            return Int64Std.ParseStd(o);
        }
        public static implicit operator Int64Std(int o)
        {
            return Int64Std.ParseStd(o);
        }
        public static implicit operator Int64Std(long o)
        {
            return Int64Std.ParseStd(o);
        }
        public static explicit operator double(Int64Std o)
        {
            return o.Value;
        }
        public static explicit operator float(Int64Std o)
        {
            return float.Parse(o.Value.ToString());
        }
        public static implicit operator long(Int64Std o)
        {
            return long.Parse(o.Value.ToString());
        }

        /// <summary>
        /// 正号運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static Int64Std operator +(Int64Std o1)
        {
            return new Int64Std(o1.Value);
        }
        /// <summary>
        /// 加運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Int64Std operator +(Int64Std o1,Int64Std o2)
        {
            return new Int64Std(o1.Value + o2.Value);
        }
        public static Int64Std operator +(Int64Std o1, long o2)
        {
            return new Int64Std(o1.Value + o2);
        }
        public static Int64Std operator +(long o1, Int64Std o2)
        {
            return new Int64Std(o1 + o2);
        }
        public static string operator +(Int64Std o1, string o2)
        {
            return o1.Value + o2;
        }
        /// <summary>
        /// 负号運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static Int64Std operator -(Int64Std o1)
        {
            return new Int64Std(-o1.Value);
        }
        /// <summary>
        /// 减運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Int64Std operator -(Int64Std o1, Int64Std o2)
        {
            return new Int64Std(o1.Value - o2.Value);
        }
        public static Int64Std operator -(Int64Std o1, long o2)
        {
            return new Int64Std(o1.Value - o2);
        }
        public static Int64Std operator -(long o1, Int64Std o2)
        {
            return new Int64Std(o1 - o2.Value);
        }
        /// <summary>
        /// 乘法運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Int64Std operator *(Int64Std o1, Int64Std o2)
        {
            return new Int64Std(o1.Value * o2.Value);
        }
        public static Int64Std operator *(Int64Std o1, long o2)
        {
            return new Int64Std(o1.Value * o2);
        }
        public static Int64Std operator *(long o1, Int64Std o2)
        {
            return new Int64Std(o1 * o2);
        }
        /// <summary>
        /// 除法運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static DoubleStd operator /(Int64Std o1, Int64Std o2)
        {
            return new DoubleStd((double)o1.Value / o2.Value);
        }
        public static DoubleStd operator /(Int64Std o1, long o2)
        {
            return new DoubleStd(o1.Value / o2);
        }
        public static DoubleStd operator /(long o1, Int64Std o2)
        {
            return new DoubleStd(o1 / o2);
        }
        /// <summary>
        /// ++運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static Int64Std operator ++(Int64Std o1)
        {
            o1.Value++;
            return o1;
        }

        /// <summary>
        /// --運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static Int64Std operator --(Int64Std o1)
        {
            o1.Value--;
            return o1;
        }

        /// <summary>
        /// %運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Int64Std operator %(Int64Std o1, Int64Std o2)
        {
            return new Int64Std(o1.Value % o2.Value);
        }
        public static Int64Std operator %(Int64Std o1, long o2)
        {
            return new Int64Std(o1.Value % o2);
        }
        public static Int64Std operator %(long o1, Int64Std o2)
        {
            return new Int64Std(o1 % o2);
        }
        /// <summary>
        /// !=判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator !=(Int64Std o1, Int64Std o2)
        {
            object o = o1;
            object oo = o2;
            if (o == null || oo == null)
            {
                return o != oo;
            }
            else
            {
                return o1.Value != o2.Value;
            }
        }

        /// <summary>
        /// ==判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator ==(Int64Std o1, Int64Std o2)
        {
            object o = o1;
            object oo = o2;
            if (o == null || oo == null)
            {
                return o == oo;
            }
            else
            {
                return o1.Value == o2.Value;
            }
        }

        /// <summary>
        /// >=判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator >=(Int64Std o1, Int64Std o2)
        {
            return o1.Value >= o2.Value;
        }

        /// <summary>
        /// 小於等於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator <=(Int64Std o1, Int64Std o2)
        {
            return o1.Value <= o2.Value;
        }

        /// <summary>
        /// 小於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator <(Int64Std o1, Int64Std o2)
        {
            return o1.Value < o2.Value;
        }

        /// <summary>
        /// 大於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator >(Int64Std o1, Int64Std o2)
        {
            return o1.Value > o2.Value;
        }


        #region IFormattable 成員

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value.ToString(format, formatProvider);
        }

        #endregion
        /// <summary>
        /// 将Object转化成Int64Std数据
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Int64Std ParseStd(object o)
        {
            if (IsInt64(o))
            {
                Int64Std rtn = new Int64Std();
                rtn.Value = long.Parse(o.ToString());
                return rtn;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// long判斷
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsInt64(object o)
        {
            if (o == null || o == DBNull.Value)
            {
                return false;
            }
            else if (o.ToString().Length <= 0)
            {
                return false;
            }
            else
            {
                long t1;
                return long.TryParse(o.ToString(), out t1);
            }
        }
        /// <summary>
        /// 判断是否为long，如果不是long则返回指定的默认值，否则返回原值
        /// </summary>
        /// <param name="o"></param>
        /// <param name="defaultvalue">默认为0</param>
        /// <returns></returns>
        public static long IsNotInt64Then(object o,long defaultvalue=0)
        {
            return IsInt64(o) ? ParseStd(o).Value : defaultvalue;
        }
        public override bool Equals(object obj)
        {
            if (obj is long)
            {
                return this == (long)obj;
            }
            else if (obj is Int64Std)
            {
                return this == (Int64Std)obj;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
