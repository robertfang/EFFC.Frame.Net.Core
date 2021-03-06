using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace EFFC.Frame.Net.Base.Data
{
    public class DoubleStd : StandardData<double>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public DoubleStd()
        {
        }
        /// <summary>
        /// default constructor with default value
        /// </summary>
        /// <param name="o"></param>
        public DoubleStd(string o)
        {
            if (IsDouble(o))
            {
                this.Value = double.Parse(o);
            }
        }
        /// <summary>
        /// default constructor with default value
        /// </summary>
        /// <param name="o"></param>
        public DoubleStd(double o)
        {
            this.Value = o;
        }
        /// <summary>
        /// default constructor with default value
        /// </summary>
        /// <param name="o"></param>
        public DoubleStd(double? o)
        {
            if (o != null)
                this.Value = o.Value;
        }
        /// <summary>
        /// 隐性转化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static implicit operator DoubleStd(double o)
        {
            return DoubleStd.ParseStd(o);
        }
        /// <summary>
        /// 隐性转化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static implicit operator DoubleStd(float o)
        {
            return DoubleStd.ParseStd(o);
        }
        /// <summary>
        /// 隐性转化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static implicit operator DoubleStd(decimal o)
        {
            return DoubleStd.ParseStd(o);
        }
        /// <summary>
        /// 隐性转化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static implicit operator DoubleStd(int o)
        {
            return DoubleStd.ParseStd(o);
        }
        /// <summary>
        /// 隐性转化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static implicit operator double(DoubleStd o)
        {
            return o.Value;
        }
        /// <summary>
        /// 隐性转化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static explicit operator float(DoubleStd o)
        {
            return float.Parse(o.Value.ToString());
        }
        /// <summary>
        /// 隐性转化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static explicit operator int(DoubleStd o)
        {
            return int.Parse(o.Value.ToString());
        }

        /// <summary>
        /// 正号運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static DoubleStd operator +(DoubleStd o1)
        {
            return new DoubleStd(o1.Value);
        }
        /// <summary>
        /// 加運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static DoubleStd operator +(DoubleStd o1, DoubleStd o2)
        {
            return new DoubleStd(o1.Value + o2.Value);
        }
        /// <summary>
        /// 负号運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static DoubleStd operator -(DoubleStd o1)
        {
            return new DoubleStd(-o1.Value);
        }
        /// <summary>
        /// 减運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static DoubleStd operator -(DoubleStd o1, DoubleStd o2)
        {
            return new DoubleStd(o1.Value - o2.Value);
        }
        /// <summary>
        /// 乘法運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static DoubleStd operator *(DoubleStd o1, DoubleStd o2)
        {
            return new DoubleStd(o1.Value * o2.Value);
        }
        /// <summary>
        /// 除法運算
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static DoubleStd operator /(DoubleStd o1, DoubleStd o2)
        {
            return new DoubleStd((double)o1.Value / o2.Value);
        }

        /// <summary>
        /// ++運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static DoubleStd operator ++(DoubleStd o1)
        {
            o1.Value++;
            return o1;
        }

        /// <summary>
        /// --運算
        /// </summary>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static DoubleStd operator --(DoubleStd o1)
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
        public static DoubleStd operator %(DoubleStd o1, DoubleStd o2)
        {
            return new DoubleStd(o1.Value % o2.Value);
        }
        /// <summary>
        /// !=判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator !=(DoubleStd o1, DoubleStd o2)
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
        public static bool operator ==(DoubleStd o1, DoubleStd o2)
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
        public static bool operator >=(DoubleStd o1, DoubleStd o2)
        {
            return o1.Value >= o2.Value;
        }

        /// <summary>
        /// 小於等於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator <=(DoubleStd o1, DoubleStd o2)
        {
            return o1.Value <= o2.Value;
        }

        /// <summary>
        /// 小於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator <(DoubleStd o1, DoubleStd o2)
        {
            return o1.Value < o2.Value;
        }

        /// <summary>
        /// 大於判斷
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator >(DoubleStd o1, DoubleStd o2)
        {
            return o1.Value > o2.Value;
        }
        /// <summary>
        /// 判斷字符串是否可以转换成doubleStd类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static bool TryParse(string s, ref DoubleStd o1)
        {
            double d;
            if (double.TryParse(s, out d))
            {
                o1.Value = d;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判斷字符串是否可以转换成doubleStd类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ns"></param>
        /// <param name="formatprovider"></param>
        /// <param name="o1"></param>
        /// <returns></returns>
        public static bool TryParse(string s, NumberStyles ns, IFormatProvider formatprovider,ref  DoubleStd o1)
        {
            double d;
            if (o1 == null) o1 = new DoubleStd();

            if (double.TryParse(s, ns, formatprovider,out d))
            {
                o1.Value = d;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 将Object转化成DoubleStd数据
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DoubleStd ParseStd(object o)
        {
            if (IsDouble(o))
            {
                DoubleStd rtn = new DoubleStd();
                rtn.Value = double.Parse(o.ToString());
                return rtn;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// double判斷
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsDouble(object o)
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
                double t1;
                return double.TryParse(o.ToString(), out t1);
            }
        }
        /// <summary>
        /// 判断是否为double，如果不是double则返回指定的默认值，否则返回原值
        /// </summary>
        /// <param name="o"></param>
        /// <param name="defaultvalue">默认为0</param>
        /// <returns></returns>
        public static double IsNotDoubleThen(object o, double defaultvalue = 0)
        {
            return IsDouble(o) ? ParseStd(o).Value : defaultvalue;
        }
        public override bool Equals(object obj)
        {
            if(obj is double)
            {
                return this == (double)obj;
            }else if(obj is DoubleStd)
            {
                return this == (DoubleStd)obj;
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

        #region IComparable 成員

        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(DoubleStd))
            {
                return this.Value.CompareTo(((DoubleStd)obj).Value);
            }
            else if(obj is double)
            {
                return this.Value.CompareTo((double)obj);
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region IFormattable 成員

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.Value.ToString(format, formatProvider);
        }

        #endregion

        #region IComparable<DoubleStd> 成員

        public int CompareTo(DoubleStd other)
        {
            return this.Value.CompareTo(other.Value);
        }

        #endregion
    }
}
