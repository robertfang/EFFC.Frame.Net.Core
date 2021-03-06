﻿using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data.Base;
using EFFC.Frame.Net.Unit.DataObjectDefine;
using EFFC.Frame.Net.Unit.DataObjectDefine.Datas;
using EFFC.Frame.Net.Unit.DataObjectDefine.Parameters;
using EFFC.Frame.Net.Unit.DataObjectDefine.Unit;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFFC.Frame.Net.Module.Business.Logic
{
    public partial class BaseLogic<PType, DType>
    {
        private DOHelper _dod;
        /// <summary>
        /// dod操作相关
        /// </summary>
        public virtual DOHelper DOD
        {
            get
            {
                if (_dod == null)
                    _dod = new DOHelper(this);

                return _dod;
            }
        }
        /// <summary>
        /// dod操作相关-动态调用方法
        /// </summary>
        public dynamic DO
        {
            get
            {
                if (_dod == null)
                    _dod = new DOHelper(this);

                return _dod;
            }
        }

        public class DOHelper : MyDynamicMetaProvider
        {
            BaseLogic<PType, DType> _logic;
            public DOHelper(BaseLogic<PType, DType> logic)
            {
                _logic = logic;
            }
            /// <summary>
            /// DOD调用，param的第一个默认为instanceid
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="propertyname"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public DOCollection DO<T>(string propertyname, params KeyValuePair<string, object>[] param) where T : DODBaseUnit
            {
                var pp = GetP();
                pp.PropertyName = propertyname;
                if (param != null)
                {
                    foreach (var val in param)
                    {
                        pp.SetValue(val.Key, val.Value);
                    }
                }
                pp.ExtentionObj.instanceid = param.Length > 0 ? param[0].Value : "";
                var result = DODUnitProxy.CallDOD<T>(pp);

                return result;
            }

            private DODParameter GetP()
            {
                DODParameter pp = new DODParameter();

                foreach (var val in _logic.CallContext_Parameter.Domain(DomainKey.INPUT_PARAMETER))
                {
                    pp.SetValue(val.Key, val.Value);
                }
                foreach (var val in _logic.CallContext_Parameter.Domain(DomainKey.CONFIG))
                {
                    pp.SetValue(DomainKey.CONFIG, val.Key, val.Value);
                }

                pp.SetValue(ParameterKey.RESOURCE_MANAGER, _logic.CallContext_ResourceManage);
                pp.SetValue(ParameterKey.TOKEN, _logic.CallContext_CurrentToken);
                return pp;
            }


            protected override object SetMetaValue(string key, object value)
            {
                return this;
            }

            protected override object GetMetaValue(string key)
            {
                var p = GetP();
                return DODUnitProxy.GetDOD(key, p);
            }

            protected override object InvokeMe(string methodInfo, params object[] args)
            {
                var p = GetP();
                var du = DODUnitProxy.GetDOD(methodInfo, p);
                if (args.Length > 0)
                {
                    du.InstanceID = ComFunc.nvl(args[0]);
                }
                else
                {
                    du.InstanceID = "";
                }

                return du;
            }
        }
    }
}
