﻿using EFFC.Frame.Net.Base.Data;
using EFFC.Frame.Net.Base.Data.Base;
using EFFC.Frame.Net.Base.Exceptions;
using EFFC.Frame.Net.Base.Interfaces.Core;
using EFFC.Frame.Net.Base.Parameter;
using EFFC.Frame.Net.Global;
using EFFC.Frame.Net.Unit.DB.Datas;
using EFFC.Frame.Net.Unit.DB.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EFFC.Frame.Net.Unit.DB.Unit
{
    /// <summary>
    /// 翻页查询操作Unit
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryByPageUnit<T> : IUnit where T : IDBUnit<UnitParameter>
    {
        public DataCollection DoOperate(ParameterStd p)
        {
            string flag = p.GetValue<string>("_unit_action_flag_");
            //预执行
            T t = (T)Activator.CreateInstance(typeof(T), true);
            UnitParameter up = (UnitParameter)p;
            UnitDataCollection rtn = new UnitDataCollection();
            if (up.Dao is ADBAccess)
            {
                var sqlobj = t.GetSqlFunc(flag)(up);
                ADBAccess dba = (ADBAccess)up.Dao;
                string regstr = "";
                string regexpress = @"(?isx)
                                (')                                                           #开始标记“<tag...>”
                                (?>                                                                  #分组构造，用来限定量词“*”修饰范围
                                \1  (?<Open>)                                                 #命名捕获组，遇到开始标记，入栈，Open计数加1
                                |\1  (?<-Open>)                                                   #狭义平衡组，遇到结束标记，出栈，Open计数减1
                                |[^']*                                                   #右侧不为开始或结束标记的任意字符
                                )
                                (?(Open)(?!))                                                        #判断是否还有'OPEN'，有则说明不配对，什么都不匹配
                                \1                                                                #结束标记“</tag>”
                     ";
                string tmpsql = "";
                regstr = @"(?<=" + (dba.ParameterFlagChar == "$" ? "\\$" : dba.ParameterFlagChar) + @")[A-Za-z0-9_]+\d*";

                Regex re = new Regex(regstr);
                Regex re2 = new Regex(regexpress);
                try
                {
                    if (!(sqlobj is FrameDLRObject))
                    {
                        throw new TypeRequiredException("需要指定的动态数据对象：FrameDLRObject");
                    }

                    string presql = sqlobj.presql;
                    DBAPageP dbc = new DBAPageP();
                    //dba.BeginTransaction();
                    if (!string.IsNullOrEmpty(presql))
                    {
                        tmpsql = presql.Replace("''", "#sp#");
                        foreach (Match m in re2.Matches(tmpsql))
                        {
                            tmpsql = tmpsql.Replace(m.Value, "#sp#");
                        }

                        foreach (System.Text.RegularExpressions.Match m in re.Matches(tmpsql))
                        {
                            dbc.SQL_Parameters.Add(m.ToString(), up.GetValue(m.ToString()));
                        }
                        try
                        {
                            dba.ExecuteNoQuery(presql, dbc.SQL_Parameters);
                        }
                        catch (Exception ex)
                        {
                            FrameDLRObject dp = FrameDLRObject.CreateInstance(Base.Constants.FrameDLRFlags.SensitiveCase);
                            foreach (var item in dbc.SQL_Parameters)
                            {
                                if (item.Value.ParameterValue is DateTime)
                                {
                                    dp.SetValue(item.Key, DateTimeStd.IsDateTimeThen(item.Value.ParameterValue, "yyyy-MM-dd HH:mm:ss"));
                                }
                                else if (item.Value.ParameterValue is DBNull)
                                {
                                    dp.SetValue(item.Key, null);
                                }
                                else
                                {
                                    dp.SetValue(item.Key, item.Value.ParameterValue);
                                }
                            }
                            GlobalCommon.Logger.WriteLog(Base.Constants.LoggerLevel.ERROR, $"QueryByPage PreSql={presql};\nParameters={dp.ToJSONString()}");

                            throw ex;
                        }
                    }
                    //执行翻页查询
                    string sql = sqlobj.sql;
                    string orderby = sqlobj.orderby;
                    if (!string.IsNullOrEmpty(sql))
                    {
                        tmpsql = sql.Replace("''", "#sp#");
                        foreach (Match m in re2.Matches(tmpsql))
                        {
                            tmpsql = tmpsql.Replace(m.Value, "#sp#");
                        }

                        dbc.SQL_Parameters.Clear();
                        foreach (System.Text.RegularExpressions.Match m in re.Matches(tmpsql))
                        {
                            if (up.GetValue(m.ToString()) is byte[])
                            {
                                dbc.SQL_Parameters.Add(m.ToString(), up.GetValue(m.ToString()), System.Data.DbType.Binary);
                            }
                            else
                            {
                                dbc.SQL_Parameters.Add(m.ToString(), up.GetValue(m.ToString()));
                            }
                        }
                        try
                        {
                            dbc.SQL = sql;
                            dbc.OrderBy = orderby;
                            dbc.Count_of_OnePage = up.Count_Of_OnePage;
                            dbc.CurrentPage = up.CurrentPage;
                            dba.StartPageByCondition(dbc);
                            rtn.QueryTable = dba.GoToPage(up.ToPage);
                            rtn.QueryDatas = new DataSetStd();
                            rtn.QueryDatas.Tables.Add(rtn.QueryTable);
                            rtn.Count_Of_OnePage = up.Count_Of_OnePage;
                            rtn.CurrentPage = dba.CurrentPage;
                            rtn.TotalPage = dba.TotalPage;
                            rtn.TotalRow = dba.TotalRow;
                        }
                        catch (Exception ex)
                        {
                            FrameDLRObject dp = FrameDLRObject.CreateInstance(Base.Constants.FrameDLRFlags.SensitiveCase);
                            foreach (var item in dbc.SQL_Parameters)
                            {
                                if (item.Value.ParameterValue is DateTime)
                                {
                                    dp.SetValue(item.Key, DateTimeStd.IsDateTimeThen(item.Value.ParameterValue, "yyyy-MM-dd HH:mm:ss"));
                                }
                                else if (item.Value.ParameterValue is DBNull)
                                {
                                    dp.SetValue(item.Key, null);
                                }
                                else
                                {
                                    dp.SetValue(item.Key, item.Value.ParameterValue);
                                }
                            }
                            GlobalCommon.Logger.WriteLog(Base.Constants.LoggerLevel.ERROR, $"QueryByPage sql={sql};\n order by={orderby};\nParameters={dp.ToJSONString()}");

                            throw ex;
                        }
                    }
                    //收尾处理
                    string aftersql = sqlobj.aftersql;
                    if (!string.IsNullOrEmpty(aftersql))
                    {
                        tmpsql = aftersql.Replace("''", "#sp#");
                        foreach (Match m in re2.Matches(tmpsql))
                        {
                            tmpsql = tmpsql.Replace(m.Value, "#sp#");
                        }

                        dbc.SQL_Parameters.Clear();
                        foreach (System.Text.RegularExpressions.Match m in re.Matches(tmpsql))
                        {
                            dbc.SQL_Parameters.Add(m.ToString(), up.GetValue(m.ToString()));
                        }
                        try
                        {
                            dba.ExecuteNoQuery(aftersql, dbc.SQL_Parameters);
                        }catch(Exception ex)
                        {
                            FrameDLRObject dp = FrameDLRObject.CreateInstance(Base.Constants.FrameDLRFlags.SensitiveCase);
                            foreach (var item in dbc.SQL_Parameters)
                            {
                                if (item.Value.ParameterValue is DateTime)
                                {
                                    dp.SetValue(item.Key, DateTimeStd.IsDateTimeThen(item.Value.ParameterValue, "yyyy-MM-dd HH:mm:ss"));
                                }
                                else if (item.Value.ParameterValue is DBNull)
                                {
                                    dp.SetValue(item.Key, null);
                                }
                                else
                                {
                                    dp.SetValue(item.Key, item.Value.ParameterValue);
                                }
                            }
                            GlobalCommon.Logger.WriteLog(Base.Constants.LoggerLevel.ERROR, $"QueryByPage AfterSql={aftersql};\nParameters={dp.ToJSONString()}");

                            throw ex;
                        }
                    }

                    //dba.CommitTransaction();

                }
                catch
                {
                    //if (dba != null)
                    //    dba.RollbackTransaction();
                    throw;
                }

            }
            return rtn;
        }
    }
}
