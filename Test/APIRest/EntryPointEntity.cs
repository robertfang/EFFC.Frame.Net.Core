﻿using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Test.APIRest
{
    public class EntryPointEntity
    {
        static Regex _reg_version_ = new Regex(@"(?<=.)v\d+._\d+", RegexOptions.IgnoreCase);
        static Regex _reg_brace_p_ = new Regex(@"\{[A-Za-z0-9_]+\}", RegexOptions.IgnoreCase);
        static string[] _method_verbs_ = new string[] { "get", "post", "put", "patch", "delete" };

        Dictionary<string, List<RouteInvokeEntity>> _d_invoke_ = new Dictionary<string, List<RouteInvokeEntity>>();
        /// <summary>
        /// verb+route资源个数做key映射一个索引,该索引可以用于通过url进行执行列表的查找功能
        /// </summary>
        Dictionary<string, string> _d_route_index_ = new Dictionary<string, string>();
        /// <summary>
        /// 无效的关键字，用于路由搜索
        /// 覆盖范围：logic的name，{p}
        /// </summary>
        List<string> _invalid_keys_ = new List<string>();
        private EntryPointEntity() { }
        public string APIVersion
        {
            get;
            internal protected set;
        }

        public string Name
        {
            get;
            internal protected set;
        }

        public FrameDLRObject RouteDesc
        {
            get;
            internal protected set;
        }
        public object Invoke(string[] sarray, string verb)
        {
            object[] parseParams = null;
            var dt = DateTime.Now;
            var invokelist = FindByRoute(sarray, verb, ref parseParams);
            Console.WriteLine($"路由搜索消耗时间：{(DateTime.Now-dt).TotalMilliseconds}ms"); dt = DateTime.Now;

            if (invokelist == null)
            {
                var strroute = "";
                foreach (var s in sarray)
                {
                    strroute += $"/{s}";
                }
                Console.WriteLine($"未找到[{verb}-{strroute}]的执行方法，请阅读API手册，确定调用的URL是正确的");
                return null;
            }
            var index = 0;
            var preIndex = 0;
            object preResult = null;
            foreach(var rie in invokelist)
            {
                var p = new List<object>();
                p.AddRange(parseParams.Skip(preIndex).Take(rie.ParameterCountWithOutParent).ToArray());
                if (rie.HasParentParameter) p.Add(preResult);
                preResult = rie.Invoke(p.ToArray());
                preIndex += rie.ParameterCountWithOutParent;
                index++;
            }
            Console.WriteLine($"方法执行消耗时间：{(DateTime.Now - dt).TotalMilliseconds}ms"); dt = DateTime.Now;
            return preResult;
        }

        /// <summary>
        /// 根据请求的url资源和verb抓取执行列表
        /// </summary>
        /// <param name="sarray"></param>
        /// <param name="verb"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<RouteInvokeEntity> FindByRoute(string[] sarray,string verb,ref object[] parsedParams)
        {
            var paramValues = new List<object>();
            var key = $"{verb}:{sarray.Length}:";
            for (var i = 0; i < sarray.Length; i++)
            {
                if (_invalid_keys_.Contains(sarray[i]))
                {
                    key += $"{sarray[i]}@{i}|";
                }
                else
                {
                    paramValues.Add(sarray[i]);
                }
            }

            if (!_d_route_index_.ContainsKey(key)) return null;

            parsedParams = paramValues.ToArray();
            return _d_invoke_[_d_route_index_[key]];
        }

        public static EntryPointEntity CreateFrom(Type t,List<Type> pointTypeList)
        {
            if (t.GetTypeInfo().BaseType != typeof(PointLogic)) return null;

            var rtn = new EntryPointEntity();
            var tmp = (PointLogic)Activator.CreateInstance(t, true);
            //版本号
            var ns = t.Namespace;
            if (_reg_version_.IsMatch(ns))
            {
                rtn.APIVersion = _reg_version_.Match(ns).Value.ToLower().Replace("_", "");
            }
            rtn.Name = tmp.Name;
            //建立树形调用链式结构
            rtn.BuildEntryRouteInvokeLink(t, pointTypeList);
            //建立基于RouteAttribute描述的独立入口
            rtn.BuildSingleEntryInvokeLink(t, pointTypeList);
            rtn.RouteDesc = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
            var index = 1;
            foreach(var item in rtn._d_invoke_)
            {
                var key = $"NO.{index}";
                var v = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                v.Desc = item.Value.Last().RouteDesc;
                v.Verb = item.Key.Split(':')[0];
                v.Url = item.Key.Split(':')[1];
                rtn.RouteDesc.SetValue(key, v);

                
                index++;
            }
            //通过{verb+route+keyname@keyname所在位置}做key映射一个索引
            var list = new List<string>();
            foreach (var item in rtn._d_invoke_)
            {
                var verb = item.Key.Split(':')[0];
                var url = item.Key.Split(':')[1];
                var sary = url.Split('/').Where(p => p != "");
                var express = _reg_brace_p_.Replace(url, @"{p}").Substring(1);

                //关键字
                var keynames = express.Split('/');
                list.AddRange(keynames);

                var key = $"{verb}:{sary.Count()}:";
                for (var i = 0; i < keynames.Count(); i++)
                {
                    if (keynames[i] != "{p}")
                    {
                        key += $"{keynames[i]}@{i}|";
                    }
                }
                if (!rtn._d_route_index_.ContainsKey(key))
                {
                    rtn._d_route_index_.Add(key, item.Key);
                }
            }

            rtn._invalid_keys_ = list.Distinct().ToList();

            return rtn;
        }
        /// <summary>
        /// 构建入口路由
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pointTypeList"></param>
        private void BuildEntryRouteInvokeLink(Type t, List<Type> pointTypeList)
        {
            var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(p => _method_verbs_.Contains(p.Name.ToLower())
                                    && (p.DeclaringType == t || p.DeclaringType == typeof(PointLogic)) && p.GetCustomAttribute(typeof(EWRARouteAttribute))==null);
            var subtypes = pointTypeList.Where(p => p.GetTypeInfo().BaseType == t).ToList();
            var instance = (PointLogic)Activator.CreateInstance(t, true);
            var tmpdic = new  Dictionary<string, List<RouteInvokeEntity>>();

            foreach (var item in methods)
            {
                //入口logic不可拥有名称开头为parent_的参数
                if (item.GetParameters().Where(p => p.Name.StartsWith("parent_")).Count() > 0) continue;
                //参数类型只能为string
                if (item.GetParameters().Where(p => p.ParameterType != typeof(string)).Count() > 0) continue;


                var entity = new RouteInvokeEntity();
                entity.InstanceType = t;
                entity.InvokeMethod = item;
                entity.InvokeName = item.Name.ToLower();

                entity.Route = $"/{instance.Name}";
                entity.ParameterCountWithOutParent = 0;
                entity.HasParentParameter = false;
                foreach (var p in entity.InvokeMethod.GetParameters())
                {
                    entity.Route += $"/{{{p.Name}}}";
                    entity.ParameterCountWithOutParent++;
                }
                var attrdesc = entity.InvokeMethod.GetCustomAttribute<RouteDescAttribute>();
                entity.RouteDesc = attrdesc == null ? "" : attrdesc.Desc;



                var key = $"{entity.InvokeName}:{entity.Route}";
                if (_d_invoke_.ContainsKey(key))
                {
                    if (entity.InvokeMethod.DeclaringType == t && _d_invoke_[key][0].InvokeMethod.DeclaringType != t)
                    {
                        _d_invoke_[key][0] = entity;
                        tmpdic[key][0] = entity;
                    }
                }
                else
                {
                    var list = new List<RouteInvokeEntity>();
                    list.Add(entity);
                    _d_invoke_.Add(key, list);
                    tmpdic.Add(key, list);
                }
                
            }
            foreach (var item in tmpdic)
            {
                //搜索子类
                foreach (var subt in subtypes)
                {
                    BuildNextRouteInvokeLink(subt, item.Key, item.Value, pointTypeList);
                }
            }
            tmpdic.Clear();
            //子类独立入口路径建立
            foreach (var subt in subtypes)
            {
                BuildEntryRouteInvokeLink(subt, pointTypeList);
            }
        }
        /// <summary>
        /// 构建next执行链接
        /// </summary>
        /// <param name="t"></param>
        /// <param name="parentRIE"></param>
        /// <param name="pointTypeList"></param>
        private void BuildNextRouteInvokeLink(Type t,string parentKey, List<RouteInvokeEntity> entryRIE, List<Type> pointTypeList)
        {
            var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(p => _method_verbs_.Contains(p.Name.ToLower())
                                    && (p.DeclaringType == t || p.DeclaringType == typeof(PointLogic)) && p.GetCustomAttribute(typeof(EWRARouteAttribute)) == null);
            var subtypes = pointTypeList.Where(p => p.GetTypeInfo().BaseType == t).ToList();

            RouteInvokeEntity parentRIE = entryRIE[entryRIE.Count-1];
            var instance = (PointLogic)Activator.CreateInstance(t, true);

            foreach (var item in methods)
            {
                //parent_开头的参数只能有一个
                if (item.GetParameters().Where(p => p.Name.StartsWith("parent_")).Count() != 1) continue;
                if (parentRIE.InvokeName != "get") continue;
                var parent_p = item.GetParameters().Where(p => p.Name.StartsWith("parent_")).First();
                if (parentRIE.InvokeMethod.ReturnType != parent_p.ParameterType) continue;
                //参数类型只能为string
                if (item.GetParameters().Where(p => p.ParameterType != typeof(string) && !p.Name.StartsWith("parent_")).Count() > 0) continue;

                var list = new List<RouteInvokeEntity>();

                list.AddRange(entryRIE);

                var entity = new RouteInvokeEntity();
                entity.InstanceType = t;
                entity.InvokeMethod = item;
                entity.InvokeName = item.Name.ToLower();
                entity.Route = $"/{instance.Name}";
                entity.ParameterCountWithOutParent = 0;
                entity.HasParentParameter = true;
                foreach (var p in entity.InvokeMethod.GetParameters().Where(p => !p.Name.StartsWith("parent_")))
                {
                    entity.Route += $"/{{{p.Name}}}";
                    entity.ParameterCountWithOutParent++;
                }
                var attrdesc = entity.InvokeMethod.GetCustomAttribute<RouteDescAttribute>();
                entity.RouteDesc = attrdesc == null ? "" : attrdesc.Desc;


                list.Add(entity);
                var key = $"{entity.InvokeName}:{parentKey.Split(':')[1]}{entity.Route}";
                _d_invoke_.Add(key,list);

                //搜索子类
                foreach (var subt in subtypes)
                {
                    BuildNextRouteInvokeLink(subt, key, list, pointTypeList);
                }

            }
        }
        /// <summary>
        /// 基于RouteAttribute描述构建自定义独立API入口
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pointTypeList"></param>
        private void BuildSingleEntryInvokeLink(Type entryType, List<Type> pointTypeList)
        {
            var types = pointTypeList.Where(p => p == entryType || p.GetTypeInfo().IsSubclassOf(entryType));
            foreach(Type item in types)
            {
                var methods = item.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(p => p.GetCustomAttribute<EWRARouteAttribute>() != null);
                foreach(var m in methods)
                {
                    //参数类型只能为string
                    if (m.GetParameters().Where(p => p.ParameterType != typeof(string)).Count() > 0) continue;

                    var attri = m.GetCustomAttribute<EWRARouteAttribute>();
                    if (attri.MethodVerb == "" || !_method_verbs_.Contains( attri.MethodVerb)) continue;

                    var list = new List<RouteInvokeEntity>();

                    var entity = new RouteInvokeEntity();
                    entity.InstanceType = item;
                    entity.InvokeMethod = m;
                    entity.InvokeName = attri.MethodVerb;
                    entity.Route = $"/{attri.Route}";
                    entity.ParameterCountWithOutParent = 0;
                    entity.HasParentParameter = false;
                    foreach (var p in entity.InvokeMethod.GetParameters())
                    {
                        //如果route中已经含有对应的参数定义，则不自动添加
                        if (!attri.Route.Contains($"/{{{p.Name}}}"))
                        {
                            entity.Route += $"/{{{p.Name}}}";
                        }
                        entity.ParameterCountWithOutParent++;
                    }
                    var attrdesc = entity.InvokeMethod.GetCustomAttribute<RouteDescAttribute>();
                    entity.RouteDesc = attrdesc == null ? "" : attrdesc.Desc;
                    

                    list.Add(entity);
                    var key = $"{attri.MethodVerb}:{attri.Route}";
                    if (!_d_invoke_.ContainsKey(key))
                        _d_invoke_.Add(key, list);
                    else
                    {
                        Console.WriteLine($"路由路径:{key}已经存在;相关信息:类名-{item.Name},方法名称-{m.Name},参数-{entity.Route.Replace($"/{entity.Route}","").Replace("/",",")}");
                        continue;
                    }
                }
            }
        }
        /// <summary>
        /// 路由执行流程中的元素
        /// </summary>
        internal class RouteInvokeEntity
        {
            public RouteInvokeEntity()
            {
                
            }
            /// <summary>
            /// 执行的实例的类型
            /// </summary>
            public Type InstanceType
            {
                get;
                internal protected set;
            }
            /// <summary>
            /// 执行的方法
            /// </summary>
            public MethodInfo InvokeMethod
            {
                get;
                internal protected set;
            }
            /// <summary>
            /// 对应的路由路径
            /// </summary>
            public string Route
            {
                get;
                internal protected set;
            }
            /// <summary>
            /// 对应路由描述
            /// </summary>
            public string RouteDesc
            {
                get;
                internal protected set;
            }
            /// <summary>
            /// 执行的名称
            /// </summary>
            public string InvokeName
            {
                get;
                internal protected set;
            }
            /// <summary>
            /// 排除名称为parent_开头的参数后的参数个数
            /// </summary>
            public int ParameterCountWithOutParent
            {
                get;
                internal protected set;
            }
            /// <summary>
            /// 判断是否有名称为parent_开头的参数
            /// </summary>
            public bool HasParentParameter
            {
                get;
                internal protected set;
            }
            /// <summary>
            /// 执行
            /// </summary>
            /// <param name="paramValues"></param>
            /// <returns></returns>
            public object Invoke(object[] paramValues)
            {
                var instance = Activator.CreateInstance(InstanceType);
                //找到正确的参数对应顺序
                var parameterlist = InvokeMethod.GetParameters().Select(p => p.Name.ToLower()).ToList();
                var matchs = _reg_brace_p_.Matches(Route);
                var l = new List<string>();
                foreach (Match m in matchs)
                {
                    var key = m.Value.ToLower().Replace("{", "").Replace("}", "");
                    l.Add(key);
                }
                var newp = new List<object>();
                
                foreach (var item in parameterlist)
                {
                    if (l.Contains(item))
                        newp.Add(paramValues[l.IndexOf(item)]);
                }
                if (HasParentParameter)
                    newp.Add(paramValues.Last());

                return InvokeMethod.Invoke(instance, newp.ToArray());
            }

            public RouteInvokeEntity Clone()
            {
                var rtn = new RouteInvokeEntity();
                rtn.InstanceType = InstanceType;
                rtn.InvokeMethod = InvokeMethod;
                rtn.InvokeName = InvokeName;
                rtn.Route = Route;
                rtn.RouteDesc = RouteDesc;
                return rtn;
            }

        }
    }
}
