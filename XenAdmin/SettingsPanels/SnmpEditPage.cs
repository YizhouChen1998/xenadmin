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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XenAdmin.Actions;
using XenAdmin.Actions.SNMP;
using XenAdmin.Core;
using XenAdmin.Properties;
using XenAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using CheckBox = System.Windows.Forms.CheckBox;

namespace XenAdmin.SettingsPanels
{
    public partial class SnmpEditPage : UserControl, IEditPage
    {
        public override string Text => Messages.SNMP;
        public string SubText => Messages.SNMP_EDIT_PAGE_TEXT;
        public Image Image => Images.StaticImages._000_Network_h32bit_16;
        private ToolTip _invalidParamToolTip = new ToolTip
        {
            IsBalloon = true,
            ToolTipIcon = ToolTipIcon.Warning
        };
        private (Control Control, string Title, string Text) _tuple;
        private static readonly Regex RegexCommon = new Regex(@"^[a-zA-Z0-9-.\#@=:_]{6,32}$");
        private static readonly Regex RegexEncryptTextBox = new Regex(@"^([a-zA-Z0-9-.\#@=:_]{8,32}|[*]{8})$");
        private bool _encryptTextBoxFlag;
        private readonly List<int> _trapIndexList = new List<int>();
        private IXenObject _clone;
        private SnmpConfiguration _snmpConfiguration;
        private readonly SnmpConfiguration _snmpCurrentConfiguration = new SnmpConfiguration();

        public SnmpEditPage()
        {
            InitializeComponent();
            this.Load += SnmpEditPage_Load;
        }

        public bool HasChanged
        {
            get
            {
                UpdateCurrentSnmpConfiguration();
                return !_snmpCurrentConfiguration.Equals(_snmpConfiguration);
            }
        }

        public bool ValidToSave
        {
            get
            {
                _tuple = (null, null, null);
                if (!EnableSnmpCheckBox.Checked)
                {
                    return true;
                }
                if (!SupportV2cCheckBox.Checked && !SupportV3CheckBox.Checked)
                {
                    _tuple = (SupportV2cCheckBox, Messages.SNMP_ALLOW_CHOOSE_TITLE, Messages.SNMP_ALLOW_CHOOSE_TITLE);
                    return false;
                }
                if (SupportV2cCheckBox.Checked)
                {
                    var communityStr = CommunityTextBox.Text;
                    if (string.IsNullOrEmpty(communityStr) || !RegexCommon.Match(communityStr.Trim()).Success)
                    {
                        _tuple = (CommunityTextBox, Messages.SNMP_ALLOW_COMMUNITY_TITLE, Messages.SNMP_ALLOW_COMMUNITY_TEXT);
                        return false;
                    }
                }
                if (SupportV3CheckBox.Checked)
                {
                    var usernameStr = UserNameTextBox.Text;
                    var authPassStr = AuthenticationPasswordLabelTextBox.Text;
                    var privacyPassStr = PrivacyPasswordTextBox.Text;
                    if (string.IsNullOrEmpty(usernameStr) || !RegexCommon.Match(usernameStr.Trim()).Success)
                    {
                        _tuple = (UserNameTextBox, Messages.SNMP_ALLOW_USER_TITLE, Messages.SNMP_ALLOW_COMMUNITY_TEXT);
                        return false;
                    }

                    if (string.IsNullOrEmpty(authPassStr) || !RegexEncryptTextBox.Match(authPassStr.Trim()).Success)
                    {
                        _tuple = (AuthenticationPasswordLabelTextBox, Messages.SNMP_ALLOW_AUTH_TITLE, Messages.SNMP_ALLOW_AUTH_TEXT);
                        return false;
                    }

                    if (string.IsNullOrEmpty(privacyPassStr) || !RegexEncryptTextBox.Match(privacyPassStr.Trim()).Success)
                    {
                        _tuple = (PrivacyPasswordTextBox, Messages.SNMP_ALLOW_PRIVACY_TITLE, Messages.SNMP_ALLOW_AUTH_TEXT);
                        return false;
                    }
                }
                return true;
            }
        }

        public void Cleanup()
        {
            _invalidParamToolTip.Dispose();
        }

        public void ShowLocalValidationMessages()
        {
            if (_tuple.Control != null)
            {
                _invalidParamToolTip.Dispose();
                _invalidParamToolTip = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipIcon = ToolTipIcon.Warning,
                };
                _invalidParamToolTip.ToolTipTitle = _tuple.Title;
                HelpersGUI.ShowBalloonMessage(_tuple.Control, _invalidParamToolTip, _tuple.Text);
            }
        }

        public void HideLocalValidationMessages()
        {
            if (_tuple.Control != null) _invalidParamToolTip.Hide(_tuple.Control);
            _invalidParamToolTip.RemoveAll();
            _invalidParamToolTip.ToolTipTitle = null;
        }

        public AsyncAction SaveSettings()
        {
            return new SnmpUpdateAction(_clone, _snmpCurrentConfiguration, true);
        }

        public void SetXenObjects(IXenObject orig, IXenObject clone)
        {
            _clone = clone;
            UpdateAllComponents(false);
            var action = new SnmpRetrieveAction(_clone, true);
            action.Completed += ActionCompleted;
            action.RunAsync();
        }

        private void ActionCompleted(ActionBase sender)
        {
            _snmpConfiguration = null;
            if (sender.Succeeded && sender is SnmpRetrieveAction a && a.SnmpConfiguration != null) _snmpConfiguration = a.SnmpConfiguration;
            Program.Invoke(Parent, UpdateRetrieveStatus);
        }

        private void UpdateRetrieveStatus()
        {
            if (_snmpConfiguration != null)
            {
                _encryptTextBoxFlag = true;
                EnableSnmpCheckBox.Enabled = true;
                AddTrapButton.Enabled = true;
                RetrieveSnmpPanel.Visible = false;
                ServiceStatusPicture.Visible = ServiceStatusLabel.Visible =
                    !_snmpConfiguration.ServiceStatus && _snmpConfiguration.IsSnmpEnabled;
                EnableSnmpCheckBox.Checked = _snmpConfiguration.IsSnmpEnabled;
                DebugLogCheckBox.Checked = _snmpConfiguration.IsLogEnabled;
                SupportV2cCheckBox.Checked = _snmpConfiguration.IsV2CEnabled;
                SupportV3CheckBox.Checked = _snmpConfiguration.IsV3Enabled;
                CommunityTextBox.Text = _snmpConfiguration.Community;
                UserNameTextBox.Text = _snmpConfiguration.UserName;
                AuthenticationPasswordLabelTextBox.Text = _snmpConfiguration.AuthPass;
                AuthenticationProtocolComboBox.SelectedItem = _snmpConfiguration.AuthProtocol;
                PrivacyPasswordTextBox.Text = _snmpConfiguration.PrivacyPass;
                PrivacyProtocolComboBox.SelectedItem = _snmpConfiguration.PrivacyProtocol;
                if (EnableSnmpCheckBox.Checked)
                {
                    SnmpV2cPanel.Enabled = SupportV2cCheckBox.Checked;
                    SnmpV3Panel.Enabled = SupportV3CheckBox.Checked;
                }
                _encryptTextBoxFlag = false;
            }
            else
            {
                RetrieveSnmpLabel.Text = Messages.SNMP_RETRIEVE_FAILED;
                RetrieveSnmpPicture.Image = Images.StaticImages._000_error_h32bit_16;
            }

        }

        private void UpdateCurrentSnmpConfiguration()
        {
            _snmpCurrentConfiguration.IsSnmpEnabled = EnableSnmpCheckBox.Checked;
            _snmpCurrentConfiguration.IsLogEnabled = DebugLogCheckBox.Checked;
            _snmpCurrentConfiguration.IsV2CEnabled = SupportV2cCheckBox.Checked;
            _snmpCurrentConfiguration.IsV3Enabled = SupportV3CheckBox.Checked;
            _snmpCurrentConfiguration.Community = CommunityTextBox.Text;
            _snmpCurrentConfiguration.UserName = UserNameTextBox.Text;
            _snmpCurrentConfiguration.AuthPass = AuthenticationPasswordLabelTextBox.Text;
            _snmpCurrentConfiguration.AuthProtocol = AuthenticationProtocolComboBox.Text;
            _snmpCurrentConfiguration.PrivacyPass = PrivacyPasswordTextBox.Text;
            _snmpCurrentConfiguration.PrivacyProtocol = PrivacyProtocolComboBox.Text;
        }

        private void UpdateAllComponents(bool status)
        {
            EnableSnmpCheckBox.Enabled = DebugLogCheckBox.Enabled = AddTrapButton.Enabled = TestTrapButton.Enabled = DeleteTrapButton.Enabled = SupportV2cCheckBox.Enabled = SnmpV2cPanel.Enabled = SupportV3CheckBox.Enabled = SnmpV3Panel.Enabled = status;
        }

        private void EnableSNMPCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // agent
            DebugLogCheckBox.Enabled = EnableSnmpCheckBox.Checked;
            SupportV2cCheckBox.Enabled = EnableSnmpCheckBox.Checked;
            SnmpV2cPanel.Enabled = EnableSnmpCheckBox.Checked && SupportV2cCheckBox.Checked;
            SupportV3CheckBox.Enabled = EnableSnmpCheckBox.Checked;
            SnmpV3Panel.Enabled = EnableSnmpCheckBox.Checked && SupportV3CheckBox.Checked;
            // traps
            foreach (TableLayoutPanel panel in FlexSettingGroupBox.Controls)
            {
                int index = FlexSettingGroupBox.Controls.IndexOf(panel);
                if (index != 0)
                {
                    int currentTrapIndex = Convert.ToInt32(panel.Name[panel.Name.Length - 1].ToString());
                    Label nmsAddressLabel = (Label)panel.Controls.Find($"NMSAddressLabel{currentTrapIndex}", true).FirstOrDefault();
                    TextBox nmsAddressTextBox = (TextBox)panel.Controls.Find($"NMSAddressTextBox{currentTrapIndex}", true).FirstOrDefault();
                    Label nmsPortLabel = (Label)panel.Controls.Find($"NMSPortLabel{currentTrapIndex}", true).FirstOrDefault();
                    TextBox nmsPortTextBox = (TextBox)panel.Controls.Find($"NMSPortTextBox{currentTrapIndex}", true).FirstOrDefault();
                    CheckBox trapSupportV2CCheckBox = (CheckBox)panel.Controls.Find($"TrapSupportV2CCheckBox{currentTrapIndex}", true).FirstOrDefault();
                    TableLayoutPanel trapSnmpV2CPanel = (TableLayoutPanel)panel.Controls.Find($"TrapSnmpV2CPanel{currentTrapIndex}", true).FirstOrDefault();
                    CheckBox trapSupportV3CheckBox = (CheckBox)panel.Controls.Find($"TrapSupportV3CheckBox{currentTrapIndex}", true).FirstOrDefault();
                    TableLayoutPanel trapSnmpV3Panel = (TableLayoutPanel)panel.Controls.Find($"TrapSnmpV3Panel{currentTrapIndex}", true).FirstOrDefault();
                    if (nmsAddressLabel != null) nmsAddressLabel.Enabled = EnableSnmpCheckBox.Checked;
                    if (nmsAddressTextBox != null) nmsAddressTextBox.Enabled = EnableSnmpCheckBox.Checked;
                    if (nmsPortLabel != null) nmsPortLabel.Enabled = EnableSnmpCheckBox.Checked;
                    if (nmsPortTextBox != null) nmsPortTextBox.Enabled = EnableSnmpCheckBox.Checked;
                    if (trapSupportV2CCheckBox != null) trapSupportV2CCheckBox.Enabled = EnableSnmpCheckBox.Checked;
                    if (trapSnmpV2CPanel != null && trapSupportV2CCheckBox != null) trapSnmpV2CPanel.Enabled = EnableSnmpCheckBox.Checked && trapSupportV2CCheckBox.Checked;
                    if (trapSupportV3CheckBox != null) trapSupportV3CheckBox.Enabled = EnableSnmpCheckBox.Checked;
                    if (trapSnmpV3Panel != null && trapSupportV3CheckBox != null) trapSnmpV3Panel.Enabled = EnableSnmpCheckBox.Checked && trapSupportV3CheckBox.Checked;
                };
            }
        }

        private void EncryptTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_encryptTextBoxFlag) return;
            var textBox = (TextBox)sender;
            if (textBox.Text.Contains("*"))
            {
                textBox.Text = textBox.Text.Replace("*", "");
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            }
            if (AuthenticationPasswordLabelTextBox.Text.Contains("*"))
            {
                AuthenticationPasswordLabelTextBox.Text = "";
            }
            if (PrivacyPasswordTextBox.Text.Contains("*"))
            {
                PrivacyPasswordTextBox.Text = "";
            }
        }

        public void SnmpEditPage_Load(object sender, EventArgs e)
        {
            SwitchConfigListBox.Items[0].Selected = true;
            AuthenticationProtocolComboBox.SelectedIndex = 0;
            PrivacyProtocolComboBox.SelectedIndex = 0;
        }

        private void V3Block_Changed(object sender, EventArgs e)
        {
            if (_encryptTextBoxFlag) return;
            if (AuthenticationPasswordLabelTextBox.Text.Contains("*") || PrivacyPasswordTextBox.Text.Contains("*"))
            {
                AuthenticationPasswordLabelTextBox.Text = "";
                PrivacyPasswordTextBox.Text = "";
            }
        }

        private void SupportV2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SnmpV2cPanel.Enabled = SupportV2cCheckBox.Checked;
        }

        private void SupportV3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SnmpV3Panel.Enabled = SupportV3CheckBox.Checked;
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SwitchConfigListBox.SelectedIndices.Count == 0)
            {
                TestTrapButton.Enabled = DeleteTrapButton.Enabled = false;
                return;
            }
            int selectedIndex = SwitchConfigListBox.SelectedIndices[0];
            // change button status
            TestTrapButton.Enabled = DeleteTrapButton.Enabled = selectedIndex != 0;
            // change setting GroupBox text
            FlexSettingGroupBox.Text = selectedIndex == 0 ? "XenServer SNMP agent settings" : "NMS trap receiver settings";
            // change to corresponding setting panel
            foreach (TableLayoutPanel panel in FlexSettingGroupBox.Controls)
            {
                int index = FlexSettingGroupBox.Controls.IndexOf(panel);
                panel.Visible = selectedIndex == index;
            }
        }

        private void AddTrapButton_Click(object sender, EventArgs e)
        {
            int trapIndex = 1;
            if (_trapIndexList.Count > 0)
            {
                int index = 0;
                while (index < _trapIndexList.Count && _trapIndexList.Contains(trapIndex))
                {
                    trapIndex++;
                    index++;
                }

                _trapIndexList.Add(trapIndex);
            }
            else
            {
                _trapIndexList.Add(trapIndex);
            }
            SwitchConfigListBox.Items.Add(new ListViewItem("New Trap Receiver", 1));
            AddTrapButton.Enabled = SwitchConfigListBox.Items.Count == 1;
            InitNewTrapConfigPanel(trapIndex);
        }

        private void DeleteTrapButton_Click(object sender, EventArgs e)
        {
            if (SwitchConfigListBox.SelectedIndices.Count == 0)
            {
                return;
            }
            int selectedIndex = SwitchConfigListBox.SelectedIndices[0];
            SwitchConfigListBox.Items.RemoveAt(selectedIndex);
            _trapIndexList.RemoveAt(selectedIndex - 1);
            AddTrapButton.Enabled = SwitchConfigListBox.Items.Count == 1;
            FlexSettingGroupBox.Controls.RemoveAt(selectedIndex);
            SwitchConfigListBox.Items[selectedIndex - 1].Selected = true;
        }

        private void Textbox_IpChange(object sender, EventArgs e)
        {
            TextBox triggerTextBox = (TextBox)sender;
            int currentTrapIndex = Convert.ToInt32(triggerTextBox.Name[triggerTextBox.Name.Length - 1].ToString());
            TableLayoutPanel panel = (TableLayoutPanel)FlexSettingGroupBox.Controls[$"TrapConfigLayoutPanel{currentTrapIndex}"];
            TextBox addressTextBox = (TextBox)panel.Controls[$"NMSAddressTextBox{currentTrapIndex}"];
            TextBox portTextBox = (TextBox)panel.Controls[$"NMSPortTextBox{currentTrapIndex}"];
            string address = addressTextBox.Text;
            string port = portTextBox.Text;
            int selectedIndex = _trapIndexList.IndexOf(currentTrapIndex) + 1;
            if (string.IsNullOrWhiteSpace(address) && string.IsNullOrWhiteSpace(port))
            {
                SwitchConfigListBox.Items[selectedIndex].Text = "New trap";
            }
            else
            {
                SwitchConfigListBox.Items[selectedIndex].Text = $"{address}:{port}";
            }
        }

        private void TrapV2CCheckbox_CheckedChanged(CheckBox trapV2CheckBox, CheckBox trapV3CheckBox, TableLayoutPanel trapSnmpV2Panel)
        {
            if (trapV2CheckBox.Checked)
            {
                trapV3CheckBox.Checked = false;
            }
            trapSnmpV2Panel.Enabled = trapV2CheckBox.Checked;
        }

        private void TrapV3Checkbox_CheckedChanged(CheckBox trapV2CheckBox, CheckBox trapV3CheckBox, TableLayoutPanel trapSnmpV3Panel)
        {
            if (trapV3CheckBox.Checked)
            {
                trapV2CheckBox.Checked = false;
            }
            trapSnmpV3Panel.Enabled = trapV3CheckBox.Checked;
        }

        private void InitNewTrapConfigPanel(int trapIndex)
        {
            #region initialize components
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnmpEditPage));
            TableLayoutPanel trapConfigLayoutPanel = new TableLayoutPanel();
            Label nmsAddressLabel = new Label();
            TextBox nmsAddressTextBox = new TextBox();
            Label nmsPortLabel = new Label();
            TextBox nmsPortTextBox = new TextBox();
            CheckBox trapSupportV2CCheckBox = new CheckBox();
            TableLayoutPanel trapSnmpV2CPanel = new TableLayoutPanel();
            Label trapCommunityLabel = new Label();
            TextBox trapCommunityTextBox = new TextBox();
            CheckBox trapSupportV3CheckBox = new CheckBox();
            TableLayoutPanel trapSnmpV3Panel = new TableLayoutPanel();
            Label trapUserNameLabel = new Label();
            TextBox trapUserNameTextBox = new TextBox();
            Label trapAuthenticationPasswordLabel = new Label();
            TextBox trapAuthenticationPasswordTextBox = new TextBox();
            Label trapAuthenticationProtocolLabel = new Label();
            ComboBox trapAuthenticationProtocolComboBox = new ComboBox();
            Label trapPrivacyPasswordLabel = new Label();
            TextBox trapPrivacyPasswordTextBox = new TextBox();
            Label trapPrivacyProtocolLabel = new Label();
            ComboBox trapPrivacyProtocolComboBox = new ComboBox();
            #endregion
            #region components style
            trapConfigLayoutPanel.Name = $"TrapConfigLayoutPanel{trapIndex}";
            trapConfigLayoutPanel.Dock = DockStyle.Fill;
            trapConfigLayoutPanel.AutoSize = true ;
            for (int i = 0; i < 5; i++)
            {
                trapConfigLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            trapConfigLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            trapConfigLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67F));
            trapConfigLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            trapConfigLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));

            nmsAddressLabel.Name = $"NMSAddressLabel{trapIndex}";
            nmsAddressLabel.Text = "&NMS Address:";
            nmsAddressLabel.Anchor = AnchorStyles.Left;
            nmsAddressLabel.AutoSize = true;
            nmsAddressLabel.Enabled = EnableSnmpCheckBox.Checked;

            nmsAddressTextBox.Name = $"NMSAddressTextBox{trapIndex}";
            nmsAddressTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            nmsAddressTextBox.Enabled = EnableSnmpCheckBox.Checked;
            nmsAddressTextBox.TextChanged += Textbox_IpChange;

            nmsPortLabel.Name = $"NMSPortLabel{trapIndex}";
            nmsPortLabel.Text = "&NMS Port:";
            nmsPortLabel.Anchor = AnchorStyles.Left;
            nmsPortLabel.AutoSize = true;
            nmsPortLabel.Enabled = EnableSnmpCheckBox.Checked;

            nmsPortTextBox.Name = $"NMSPortTextBox{trapIndex}";
            nmsPortTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            nmsPortTextBox.Margin = new Padding( 3, 3, 6, 3);
            nmsPortTextBox.Enabled = EnableSnmpCheckBox.Checked;
            nmsPortTextBox.Text = "162";
            nmsPortTextBox.TextChanged += Textbox_IpChange;

            trapSupportV2CCheckBox.Name = $"TrapSupportV2cCheckBox{trapIndex}";
            trapSupportV2CCheckBox.AutoSize = true;
            trapSupportV2CCheckBox.Text = "Support SNMPv&2c";
            trapSupportV2CCheckBox.Enabled = EnableSnmpCheckBox.Checked;
            trapSupportV2CCheckBox.Checked = true;
            trapSupportV2CCheckBox.CheckedChanged += ( object sender, EventArgs e) => TrapV2CCheckbox_CheckedChanged( trapSupportV2CCheckBox, trapSupportV3CheckBox, trapSnmpV2CPanel);
            trapConfigLayoutPanel.SetColumnSpan(trapSupportV2CCheckBox, 4);

            trapSnmpV2CPanel.Name = $"TrapSnmpV2cPanel{trapIndex}";
            trapSnmpV2CPanel.Dock = DockStyle.Top;
            trapSnmpV2CPanel.AutoSize = true;
            trapSnmpV2CPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            trapSnmpV2CPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            trapSnmpV2CPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            trapSnmpV2CPanel.Enabled = EnableSnmpCheckBox.Checked && trapSupportV2CCheckBox.Checked;
            trapConfigLayoutPanel.SetColumnSpan(trapSnmpV2CPanel, 4);

            trapCommunityLabel.Name = $"TrapCommunityLabel{trapIndex}";
            trapCommunityLabel.Text = "&Community:";
            trapCommunityLabel.Anchor = AnchorStyles.Left;
            trapCommunityLabel.AutoSize = true;

            trapCommunityTextBox.Name = $"TrapCommunityTextBox{trapIndex}";
            trapCommunityTextBox.Text = "public";
            trapCommunityTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            trapSupportV3CheckBox.Name = $"TrapSupportV3CheckBox{trapIndex}";
            trapSupportV3CheckBox.AutoSize = true;
            trapSupportV3CheckBox.Text = "Support SNMPv&3";
            trapSupportV3CheckBox.Enabled = EnableSnmpCheckBox.Checked;
            trapSupportV3CheckBox.CheckedChanged += (object sender, EventArgs e) => TrapV3Checkbox_CheckedChanged(trapSupportV2CCheckBox, trapSupportV3CheckBox, trapSnmpV3Panel); ;
            trapConfigLayoutPanel.SetColumnSpan(trapSupportV3CheckBox, 4);

            trapSnmpV3Panel.Name = $"TrapSnmpV3Panel{trapIndex}";
            trapSnmpV3Panel.Dock = DockStyle.Top;
            trapSnmpV3Panel.AutoSize = true;
            for (int i = 0; i < 5; i++)
            {
                trapSnmpV3Panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            trapSnmpV3Panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            trapSnmpV3Panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            trapSnmpV3Panel.Enabled = EnableSnmpCheckBox.Checked && trapSupportV3CheckBox.Checked;
            trapConfigLayoutPanel.SetColumnSpan(trapSnmpV3Panel, 4);

            trapUserNameLabel.Name = $"TrapUserNameLabel{trapIndex}";
            trapUserNameLabel.Text = "&Username:";
            trapUserNameLabel.Anchor = AnchorStyles.Left;
            trapUserNameLabel.AutoSize = true;

            trapUserNameTextBox.Name = $"TrapUserNameTextBox{trapIndex}";
            trapUserNameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            trapAuthenticationPasswordLabel.Name = $"TrapAuthenticationPasswordLabel{trapIndex}";
            trapAuthenticationPasswordLabel.Text = "Authentication &Password:";
            trapAuthenticationPasswordLabel.Anchor = AnchorStyles.Left;
            trapAuthenticationPasswordLabel.AutoSize = true;

            trapAuthenticationPasswordTextBox.Name = $"TrapAuthenticationPasswordTextBox{trapIndex}";
            trapAuthenticationPasswordTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            trapAuthenticationProtocolLabel.Name = $"TrapAuthenticationProtocolLabel{trapIndex}";
            trapAuthenticationProtocolLabel.Text = "Au&thentication Protocol:";
            trapAuthenticationProtocolLabel.Anchor = AnchorStyles.Left;
            trapAuthenticationProtocolLabel.AutoSize = true;

            trapAuthenticationProtocolComboBox.Name = $"TrapAuthenticationProtocolComboBox{trapIndex}";
            trapAuthenticationProtocolComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            trapAuthenticationProtocolComboBox.FormattingEnabled = true;
            trapAuthenticationProtocolComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            trapAuthenticationProtocolComboBox.Items.AddRange(new object[] {
                resources.GetString("AuthenticationProtocolComboBox.Items"),
                resources.GetString("AuthenticationProtocolComboBox.Items1")});
            trapAuthenticationProtocolComboBox.SelectedIndex = 0;

            trapPrivacyPasswordLabel.Name = $"TrapPrivacyPasswordLabel{trapIndex}";
            trapPrivacyPasswordLabel.Text = "Pr&ivacy Password:";
            trapPrivacyPasswordLabel.Anchor = AnchorStyles.Left;
            trapPrivacyPasswordLabel.AutoSize = true;

            trapPrivacyPasswordTextBox.Name = $"TrapPrivacyPasswordTextBox{trapIndex}";
            trapPrivacyPasswordTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            trapPrivacyProtocolLabel.Name = $"TrapPrivacyProtocolLabel{trapIndex}";
            trapPrivacyProtocolLabel.Text = "Priva&cy Protocol:";
            trapPrivacyProtocolLabel.Anchor = AnchorStyles.Left;
            trapPrivacyProtocolLabel.AutoSize = true;

            trapPrivacyProtocolComboBox.Name = $"TrapPrivacyProtocolComboBox{trapIndex}";
            trapPrivacyProtocolComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            trapPrivacyProtocolComboBox.FormattingEnabled = true;
            trapPrivacyProtocolComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            trapPrivacyProtocolComboBox.Items.AddRange(new object[] {
                resources.GetString("PrivacyProtocolComboBox.Items"),
                resources.GetString("PrivacyProtocolComboBox.Items1")});
            trapPrivacyProtocolComboBox.SelectedIndex = 0;
            #endregion
            #region add components to parent control
            trapConfigLayoutPanel.Controls.Add(nmsAddressLabel, 0, 0);
            trapConfigLayoutPanel.Controls.Add(nmsAddressTextBox, 1, 0);
            trapConfigLayoutPanel.Controls.Add(nmsPortLabel, 2, 0);
            trapConfigLayoutPanel.Controls.Add(nmsPortTextBox, 3, 0);
            trapConfigLayoutPanel.Controls.Add(trapSupportV2CCheckBox, 0, 1);
            trapSnmpV2CPanel.Controls.Add(trapCommunityLabel, 0, 0);
            trapSnmpV2CPanel.Controls.Add(trapCommunityTextBox, 1, 0);
            trapConfigLayoutPanel.Controls.Add(trapSnmpV2CPanel, 0, 2);
            trapConfigLayoutPanel.Controls.Add(trapSupportV3CheckBox, 0, 3);
            trapSnmpV3Panel.Controls.Add(trapUserNameLabel, 0, 0);
            trapSnmpV3Panel.Controls.Add(trapUserNameTextBox, 1, 0);
            trapSnmpV3Panel.Controls.Add(trapAuthenticationPasswordLabel, 0, 1);
            trapSnmpV3Panel.Controls.Add(trapAuthenticationPasswordTextBox, 1, 1);
            trapSnmpV3Panel.Controls.Add(trapAuthenticationProtocolLabel, 0, 2);
            trapSnmpV3Panel.Controls.Add(trapAuthenticationProtocolComboBox, 1, 2);
            trapSnmpV3Panel.Controls.Add(trapPrivacyPasswordLabel, 0, 3);
            trapSnmpV3Panel.Controls.Add(trapPrivacyPasswordTextBox, 1, 3);
            trapSnmpV3Panel.Controls.Add(trapPrivacyProtocolLabel, 0, 4);
            trapSnmpV3Panel.Controls.Add(trapPrivacyProtocolComboBox, 1, 4);
            trapConfigLayoutPanel.Controls.Add(trapSnmpV3Panel, 0, 4);
            #endregion
            trapConfigLayoutPanel.Visible = false;
            FlexSettingGroupBox.Controls.Add(trapConfigLayoutPanel);
        }
    }
}
