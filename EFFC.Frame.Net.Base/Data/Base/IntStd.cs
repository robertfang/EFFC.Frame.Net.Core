using System;

namespace EFFC.Frame.Net.Base.Data
{
    public class IntStd : StandardData<int>,IFormattable
    {

        public IntStd()
        {
        }

        public IntStd(int o)
        {
            this.Value = o;
        }

        public IntStd(int? o)
        {
            if (o != null)
                this.Value = o.Value;
        }

        public static implicit operator IntStd(double o)
        {
            return IntStd.ParseStd(o);
        }
        public static implicit operator IntStd(float o)
        {
            return IntStd.ParseStd(o);
        }
        public static implicit operator IntStd(decimal o)
        {
            return IntStd.ParseStd(o);
        }
        public static implicit operator IntStd(int o)
        {
            return IntStd.ParseStd(o);
        }
        public static explicit operator double(IntStd o)
        {
            return o.Value;
        }
        public static explicit operator float(IntStd o)
        {
            return float.Parse(o.Value.ToString());
        }
        public static implicit operator int(IntStd o)
        {
            return int.Parse(o.Value.ToString());
        }

        /// <summary>
        /// 正号運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static IntStd operator +(IntStd o1)
        {
            return new IntStd(o1.Value);
        }
        /// <summary>
        /// 加運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static IntStd operator +(IntStd o1,IntStd o2)
        {
            return new IntStd(o1.Value + o2.Value);
        }
        public static IntStd operator +(IntStd o1, int o2)
        {
            return new IntStd(o1.Value + o2);
        }
        public static IntStd operator +(int o1, IntStd o2)
        {
            return new IntStd(o1 + o2);
        }
        public static string operator +(IntStd o1, string o2)
        {
            return o1.Value + o2;
        }
        /// <summary>
        /// 负号運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static IntStd operator -(IntStd o1)
        {
            return new IntStd(-o1.Value);
        }
        /// <summary>
        /// 减運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static IntStd operator -(IntStd o1, IntStd o2)
        {
            return new IntStd(o1.Value - o2.Value);
        }
        public static IntStd operator -(IntStd o1, int o2)
        {
            return new IntStd(o1.Value - o2);
        }
        public static IntStd operator -(int o1, IntStd o2)
        {
            return new IntStd(o1 - o2.Value);
        }
        /// <summary>
        /// 乘法運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static IntStd operator *(IntStd o1, IntStd o2)
        {
            return new IntStd(o1.Value * o2.Value);
        }
        public static IntStd operator *(IntStd o1, int o2)
        {
            return new IntStd(o1.Value * o2);
        }
        public static IntStd operator *(int o1, IntStd o2)
        {
            return new IntStd(o1 * o2);
        }
        /// <summary>
        /// 除法運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static DoubleStd operator /(IntStd o1, IntStd o2)
        {
            return new DoubleStd((double)o1.Value / o2.Value);
        }
        public static DoubleStd operator /(IntStd o1, int o2)
        {
            return new DoubleStd(o1.Value / o2);
        }
        public static DoubleStd operator /(int o1, IntStd o2)
        {
            return new DoubleStd(o1 / o2);
        }
        /// <summary>
        /// ++運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static IntStd operator ++(IntStd o1)
        {
            o1.Value++;
            return o1;
        }

        /// <summary>
        /// --運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static IntStd operator --(IntStd o1)
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
        public static IntStd operator %(IntStd o1, IntStd o2)
        {
            return new IntStd(o1.Value % o2.Value);
        }
        public static IntStd operator %(IntStd o1, int o2)
        {
            return new IntStd(o1.Value % o2);
        }
        public static IntStd operator %(int o1, IntStd o2)
        {
            return new IntStd(o1 % o2);
        }
        /// <summary>
        /// !=判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator !=(IntStd o1, IntStd o2)
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
        public static bool operator ==(IntStd o1, IntStd o2)
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
        public static bool operator >=(IntStd o1, IntStd o2)
        {
            return o1.Value >= o2.Value;
        }

        /// <summary>
        /// 小於等於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator <=(IntStd o1, IntStd o2)
        {
            return o1.Value <= o2.Value;
        }

        /// <summary>
        /// 小於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator <(IntStd o1, IntStd o2)
        {
            return o1.Value < o2.Value;
        }

        /// <summary>
        /// 大於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator >(IntStd o1, IntStd o2)
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
        /// 将Object转化成IntStd数据
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IntStd ParseStd(object o)
        {
            if (IsInt(o))
            {
                IntStd rtn = new IntStd();
                rtn.Value = int.Parse(o.ToString());
                return rtn;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Int判斷
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsInt(object o)
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
                int t1;
                return int.TryParse(o.ToString(), out t1);
            }
        }
        /// <summary>
        /// 判断是否为int，如果不是int则返回指定的默认值，否则返回原值
        /// </summary>
        /// <param name="o"></param>
        /// <param name="defaultvalue">默认为0</param>
        /// <returns></returns>
        public static int IsNotIntThen(object o,int defaultvalue=0)
        {
            return IsInt(o) ? ParseStd(o).Value : defaultvalue;
        }
        public override bool Equals(object obj)
        {
            if (obj is int)
            {
                return this == (int)obj;
            }
            else if (obj is IntStd)
            {
                return this == (IntStd)obj;
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
