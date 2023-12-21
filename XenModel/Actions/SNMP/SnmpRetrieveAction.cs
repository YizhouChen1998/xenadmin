/* Copyright (c) Cloud Software Group, Inc. 
 * 
 * Redistribution and use in source and binary forms, 
 * with or without modification, are permitted provided 
 * that the following conditions are met: 
 * 
 * *   Redistributions of source code must retain the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer. 
 * *   Redistributions in binary form must reproduce the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer in the documentation and/or other 
 *     materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */

using Newtonsoft.Json;
using System;
using XenAdmin.Core;
using XenAPI;

namespace XenAdmin.Actions.SNMP
{
    public class SnmpRetrieveAction : AsyncAction
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SnmpConfiguration SnmpConfiguration { get; private set; }

        public SnmpRetrieveAction(IXenObject xenObject, bool suppressHistory)
            : base(xenObject.Connection, Messages.SNMP_ACTION_RETRIEVE, Messages.SNMP_ACTION_RETRIEVE, suppressHistory)
        {
            if (xenObject is Pool p)
            {
                Pool = p;
                Host = Helpers.GetCoordinator(p);
            }
            else if (xenObject is Host h)
            {
                Host = h;
            }
            else
            {
                throw new ArgumentException($"{nameof(xenObject)} should be host or pool");
            }
        }

        protected override void Run()
        {
            try
            {
                // If error occurs in either plugin, this method will return directly and SnmpConfiguration will be null
                // Call "get-config" plugin
                var resultJson1 = Host.call_plugin(Connection.Session, Host.opaque_ref, SnmpXapiConfig.XAPI_SNMP_PLUGIN_NAME,
                    SnmpXapiConfig.XAPI_SNMP_GET_CONFIG, null);
                Log.InfoFormat("Run SNMP {0}, return: {1}", SnmpXapiConfig.XAPI_SNMP_GET_CONFIG, resultJson1);
                var resultObj1 = JsonConvert.DeserializeObject<SnmpRes<SnmpConfigurationData>>(resultJson1);
                if (resultObj1 == null || resultObj1.code != 0)
                {
                    return;
                }
                // Call "status" plugin
                var resultJson2 = Host.call_plugin(Connection.Session, Host.opaque_ref, SnmpXapiConfig.XAPI_SNMP_PLUGIN_NAME,
                    SnmpXapiConfig.XAPI_SNMP_GET_STATUS, null);
                Log.InfoFormat("Run SNMP {0}, return: {1}", SnmpXapiConfig.XAPI_SNMP_GET_STATUS, resultJson2);
                var resultObj2 = JsonConvert.DeserializeObject<SnmpRes<GetServiceStatusData>>(resultJson2);
                if (resultObj2 == null || resultObj2.code != 0)
                {
                    return;
                }
                SnmpConfiguration = new SnmpConfiguration();
                var resultData1 = resultObj1.result;
                var resultData2 = resultObj2.result;
                SnmpConfiguration.IsSnmpEnabled = resultData1.common.enabled == "yes";
                SnmpConfiguration.IsLogEnabled = resultData1.common.debug_log == "yes";
                SnmpConfiguration.IsV2CEnabled = resultData1.agent.v2c == "yes";
                SnmpConfiguration.IsV3Enabled = resultData1.agent.v3 == "yes";
                SnmpConfiguration.Community = resultData1.agent.community;
                SnmpConfiguration.UserName = resultData1.agent.user_name;
                SnmpConfiguration.AuthPass = resultData1.agent.authentication_password;
                SnmpConfiguration.AuthProtocol = resultData1.agent.authentication_protocol;
                SnmpConfiguration.PrivacyPass = resultData1.agent.privacy_password;
                SnmpConfiguration.PrivacyProtocol = resultData1.agent.privacy_protocol;
                SnmpConfiguration.ServiceStatus = resultData2.enabled == "enabled" && resultData2.active == "active";
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Run SNMP plugin failed, failed reason: {0}", e.Message);
                SnmpConfiguration = null;
            }
        }
    }
}
