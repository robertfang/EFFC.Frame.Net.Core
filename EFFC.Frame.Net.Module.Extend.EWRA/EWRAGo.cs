﻿using EFFC.Frame.Net.Module.Extend.EWRA.DataCollections;
using EFFC.Frame.Net.Module.Extend.EWRA.Parameters;
using EFFC.Frame.Net.Module.Web.Modules;
using EFFC.Frame.Net.Module.Web.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using EFFC.Frame.Net.Base.Data;
using EFFC.Frame.Net.Base.Parameter;
using EFFC.Frame.Net.Base.Module.Proxy;
using EFFC.Frame.Net.Module.Extend.EWRA.Logic;
using EFFC.Frame.Net.Global;
using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Module.Extend.EWRA.Constants;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data.Base;
using Frame.Net.Base.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace EFFC.Frame.Net.Module.Extend.EWRA
{
    public class EWRAGo : WebBaseRestAPI<EWRAParameter, EWRAData>
    {
        static string restroothome = "~/";
        public override string Name => "EWRA";

        public override string Description => "EFFC Web Rest API";

        public override void Dispose()
        {
            
        }
        protected override void OnUsed(ProxyManager ma, dynamic options)
        {
            if (options != null)
            {
                if (ComFunc.nvl(options.RestAPIRootHome) != "")
                {
                    restroothome = options.RestAPIRootHome;
                }
            }

            GlobalCommon.Logger.WriteLog(LoggerLevel.INFO,
                        string.Format("EWRAGo的RootHome设定为{0},如要调整，请在ProxyManager.UseProxy中的options参数设定RestAPIRootHome的值", restroothome));
            //构建路由表
            ma.UseProxy<EWRABusiProxy>("busi", options);
        }

        protected override void InvokeAction(EWRAParameter p, EWRAData d)
        {
            var dt = DateTime.Now;
            object obj = d;
            if (p.MethodName.ToUpper() == "OPTIONS")
            {
                //
                d.Result = "";
                d.StatusCode = RestStatusCode.OK;
            }
            else
            {
                GlobalCommon.Proxys["busi"].CallModule(ref obj, new object[] { p });
            }
            
            GlobalCommon.Logger.WriteLog(LoggerLevel.DEBUG, $"excute:{p.MethodName} \"{p.RequestRoute}\" InvokeAction cast time:{(DateTime.Now - dt).TotalMilliseconds}ms "); dt = DateTime.Now;
        }

        protected override void OnError(Exception ex, ParameterStd p, DataCollection d)
        {
            var ep = (EWRAParameter)p;
            var ed = (EWRAData)d;
            ed.StatusCode = Constants.RestStatusCode.INVALID_REQUEST;
            var isdebug = p[DomainKey.CONFIG, "DebugMode"] == null ? false : (bool)p[DomainKey.CONFIG, "DebugMode"];

            string errorCode = "E-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string errlog = "";
            if (ex is HostJsException)
            {
                var jex = (HostJsException)ex;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is HostJsException)
                    {
                        var ijex = (HostJsException)ex.InnerException;
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\n出错代码行数{3}\n出错代码列数{4}\n出错代码位置{5}\nInnerException:{6}\n{7}\n出错代码行数{8}\n出错代码列数{9}\n出错代码位置{10}", errorCode, ex.Message, ex.StackTrace,
                            jex.Line, jex.Column, jex.SourceCode.Replace("\"", "'"),
                            ex.InnerException.Message, ex.InnerException.StackTrace,
                            ijex.Line, ijex.Column, ijex.SourceCode.Replace("\"", "'"));
                    }
                    else
                    {
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\n出错代码行数{3}\n出错代码列数{4}\n出错代码位置{5}\nInnerException:{6}\n{7}", errorCode, ex.Message, ex.StackTrace, jex.Line, jex.Column, jex.SourceCode, ex.InnerException.Message, ex.InnerException.StackTrace);
                    }
                }
                else
                {
                    errlog = string.Format("错误编号：{0}，\n{1}\n{2}\n出错代码行数{3}\n出错代码列数{4}\n出错代码位置{5}", errorCode, ex.Message, ex.StackTrace,
                        jex.Line, jex.Column, jex.SourceCode.Replace("\"", "'"));
                }
            }
            else
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is HostJsException)
                    {
                        var ijex = (HostJsException)ex.InnerException;
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\nInnerException:{3}\n{4}\n\n出错代码行数{5}\n出错代码列数{6}\n出错代码位置{7}", errorCode, ex.Message, ex.StackTrace,
                            ex.InnerException.Message, ex.InnerException.StackTrace,
                            ijex.Line, ijex.Column, ijex.SourceCode.Replace("\"", "'"));
                    }
                    else
                    {
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\nInnerException:{3}\n{4}", errorCode, ex.Message, ex.StackTrace, ex.InnerException.Message, ex.InnerException.StackTrace);
                    }
                }
                else
                {
                    errlog = string.Format("错误编号：{0}，\n{1}\n{2}", errorCode, ex.Message, ex.StackTrace);
                }
            }

            GlobalCommon.Logger.WriteLog(LoggerLevel.ERROR, errlog);

            var contentbody = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
            var errormsg = "";
            if (isdebug)
            {
                errormsg = string.Format("出错了，{0}", errlog); ;
            }
            else
            {
                errormsg = string.Format("系统错误!!!（错误编号：{0}）", errorCode);
            }

            contentbody.error = errormsg;
            var jsonmsg = ((FrameDLRObject)contentbody).ToJSONString();
            var msgbytelength = Encoding.UTF8.GetByteCount(jsonmsg);

            SetHeaders(ep, ed);
            ep.CurrentHttpContext.Response.StatusCode = (int)ed.StatusCode;
            ep.CurrentHttpContext.Response.ContentType = ResponseHeader_ContentType.Map(ComFunc.Enum2String<RestContentType>(ed.ContentType).ToLower()) + ";charset=utf-8";
            ep.CurrentHttpContext.Response.Headers.Add("Content-Length", msgbytelength + "");
            ep.CurrentHttpContext.Response.WriteAsync(jsonmsg);
            ep.CurrentHttpContext.Response.Body.FlushAsync();
        }

        protected override void SetResponseContent(EWRAParameter p, EWRAData d)
        {
            var contentbody = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
            SetHeaders(p, d);
            var arr = new string[] { "put", "post", "patch", "options" };
            var msgbytelength = 0;
            var resultmsg = "";
            if (d.ContentType == RestContentType.JSON)
            {
                if(d.StatusCode == RestStatusCode.NONE)
                {
                    if (p.MethodName == "get")
                    {
                        if (d.Result != null)
                        {
                            contentbody.result = d.Result;
                            d.StatusCode = RestStatusCode.OK;
                        }
                        else
                        {
                            contentbody.error = "资源未找到或非法请求";
                            d.StatusCode = RestStatusCode.NOT_FOUND;
                        }
                    } else if (p.MethodName == "delete")
                    {
                        if (d.Result != null && d.Result is bool)
                        {
                            if ((bool)d.Result)
                                d.StatusCode = RestStatusCode.NO_CONTENT;
                            else
                            {
                                contentbody.error = "操作失败";
                                d.StatusCode = RestStatusCode.NOT_FOUND;
                            }
                        }
                        else
                        {
                            contentbody.error = "资源未找到或非法请求";
                            d.StatusCode = RestStatusCode.NOT_FOUND;
                        }
                    }
                    else if (arr.Contains(p.MethodName))
                    {
                        if (d.Result != null)
                        {
                            contentbody.result = d.Result;
                            d.StatusCode = RestStatusCode.CREATED;
                        }
                        else
                        {
                            contentbody.error = "资源未找到或非法请求";
                            d.StatusCode = RestStatusCode.NOT_FOUND;
                        }
                    }
                    else
                    {
                        contentbody.error = "非法请求";
                        d.StatusCode = RestStatusCode.FORBIDDEN;
                    }
                }
                else
                {
                    if((int)d.StatusCode >= 400)
                    {
                        contentbody.error = d.Error;
                    }
                    else
                    {
                        contentbody.result = d.Result;
                    }
                    
                }

                resultmsg = ((FrameDLRObject)contentbody).ToJSONString();
                msgbytelength = Encoding.UTF8.GetByteCount(resultmsg);
                p.CurrentHttpContext.Response.StatusCode = (int)d.StatusCode;
                p.CurrentHttpContext.Response.ContentType = ResponseHeader_ContentType.json + ";charset=utf-8";
                p.CurrentHttpContext.Response.Headers.Add("Content-Length", msgbytelength + "");
                p.CurrentHttpContext.Response.WriteAsync(resultmsg);
            }
            else
            {
                resultmsg = ComFunc.nvl(d.Result);
                msgbytelength = Encoding.UTF8.GetByteCount(resultmsg);
                p.CurrentHttpContext.Response.StatusCode = (int)d.StatusCode;
                p.CurrentHttpContext.Response.ContentType = ResponseHeader_ContentType.txt + ";charset=utf-8";
                p.CurrentHttpContext.Response.Headers.Add("Content-Length", msgbytelength + "");
                p.CurrentHttpContext.Response.WriteAsync(resultmsg);
            }
            p.CurrentHttpContext.Response.Body.FlushAsync();
        }

        protected override void BeforeProcess(EWRAParameter p, EWRAData d)
        {
            base.BeforeProcess(p, d);
            LoadConfig(p, d);
            //抓取验证token
            p.AuthorizedToken = ComFunc.nvl(p[DomainKey.HEADER, "Authorization"]).Replace("Bearer ","");
            //滤掉RootHome
            if(restroothome != "~/")
            {
                p.RequestRoute = (p.RequestRoute + "/").Replace("//","/").ToLower().Replace(restroothome.ToLower().Replace("~", ""), "");
            }
            
        }

        protected virtual void LoadConfig(EWRAParameter p, EWRAData d)
        {
            foreach (var item in MyConfig.GetConfigurationList("ConnectionStrings"))
            {
                p[DomainKey.CONFIG, item.Key] = ComFunc.nvl(item.Value);
            }
            p.DBConnectionString = ComFunc.nvl(p[DomainKey.CONFIG, "DefaultConnection"]);
            bool bvalue = true;
            foreach (var item in MyConfig.GetConfigurationList("EFFC"))
            {
                if (bool.TryParse(ComFunc.nvl(item.Value), out bvalue))
                {
                    p[DomainKey.CONFIG, item.Key] = bool.Parse(ComFunc.nvl(item.Value));
                }
                else if (DateTimeStd.IsDateTime(item.Value))
                {
                    p[DomainKey.CONFIG, item.Key] = DateTimeStd.ParseStd(item.Value).Value;
                }
                else
                {
                    p[DomainKey.CONFIG, item.Key] = ComFunc.nvl(item.Value);
                }
            }
        }

        private void SetHeaders(EWRAParameter p, EWRAData d)
        {

            p.CurrentHttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //CORS的Options验证处理
            p.CurrentHttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PATCH,PUT,DELETE,OPTIONS");
            p.CurrentHttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Content-Length, Authorization, Accept,X-Requested-With,Authorization");
            p.CurrentHttpContext.Response.Headers.Add("X-Frame-Options", "DENY");
            p.CurrentHttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            p.CurrentHttpContext.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            if (p.CurrentHttpContext.Request.IsHttps)
            {
                p.CurrentHttpContext.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            }
        }
    }
}
