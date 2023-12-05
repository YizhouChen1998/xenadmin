using System.Windows.Forms;
using DataGridView = System.Windows.Forms.DataGridView;

namespace XenAdmin.SettingsPanels
{
    partial class SnmpEditPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnmpEditPage));
            this.SnmpTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DescLabel = new System.Windows.Forms.Label();
            this.SnmpServiceStatusPanel = new System.Windows.Forms.TableLayoutPanel();
            this.EnableSnmpCheckBox = new System.Windows.Forms.CheckBox();
            this.ServiceStatusPicture = new System.Windows.Forms.PictureBox();
            this.ServiceStatusLabel = new System.Windows.Forms.Label();
            this.DebugLogCheckBox = new System.Windows.Forms.CheckBox();
            this.SwitchConfigLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SwitchConfigListBox = new System.Windows.Forms.ListBox();
            this.AddTrapButton = new System.Windows.Forms.Button();
            this.TestTrapButton = new System.Windows.Forms.Button();
            this.DeleteTrapButton = new System.Windows.Forms.Button();
            this.FlexSettingGroupBox = new System.Windows.Forms.GroupBox();
            this.BasicConfigLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SupportV2cCheckBox = new System.Windows.Forms.CheckBox();
            this.SnmpV2cPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CommunityLabel = new System.Windows.Forms.Label();
            this.CommunityTextBox = new System.Windows.Forms.TextBox();
            this.SupportV3CheckBox = new System.Windows.Forms.CheckBox();
            this.SnmpV3Panel = new System.Windows.Forms.TableLayoutPanel();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.AuthenticationPasswordLabel = new System.Windows.Forms.Label();
            this.AuthenticationPasswordLabelTextBox = new System.Windows.Forms.TextBox();
            this.AuthenticationProtocolLabel = new System.Windows.Forms.Label();
            this.AuthenticationProtocolComboBox = new System.Windows.Forms.ComboBox();
            this.PrivacyPasswordLabel = new System.Windows.Forms.Label();
            this.PrivacyPasswordTextBox = new System.Windows.Forms.TextBox();
            this.PrivacyProtocolLabel = new System.Windows.Forms.Label();
            this.PrivacyProtocolComboBox = new System.Windows.Forms.ComboBox();
            this.RetrieveSnmpPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RetrieveSnmpPicture = new System.Windows.Forms.PictureBox();
            this.RetrieveSnmpLabel = new System.Windows.Forms.Label();
            this.GeneralConfigureGroupBox = new System.Windows.Forms.GroupBox();
            this.GeneralConfigTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SnmpTableLayoutPanel.SuspendLayout();
            this.SnmpServiceStatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServiceStatusPicture)).BeginInit();
            this.SwitchConfigLayoutPanel.SuspendLayout();
            this.FlexSettingGroupBox.SuspendLayout();
            this.BasicConfigLayoutPanel.SuspendLayout();
            this.SnmpV2cPanel.SuspendLayout();
            this.SnmpV3Panel.SuspendLayout();
            this.RetrieveSnmpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RetrieveSnmpPicture)).BeginInit();
            this.GeneralConfigureGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SnmpTableLayoutPanel
            // 
            resources.ApplyResources(this.SnmpTableLayoutPanel, "SnmpTableLayoutPanel");
            this.SnmpTableLayoutPanel.Controls.Add(this.DescLabel, 0, 0);
            this.SnmpTableLayoutPanel.Controls.Add(this.SnmpServiceStatusPanel, 0, 1);
            this.SnmpTableLayoutPanel.Controls.Add(this.DebugLogCheckBox, 0, 2);
            this.SnmpTableLayoutPanel.Controls.Add(this.SwitchConfigLayoutPanel, 0, 3);
            this.SnmpTableLayoutPanel.Controls.Add(this.FlexSettingGroupBox, 0, 4);
            this.SnmpTableLayoutPanel.Controls.Add(this.RetrieveSnmpPanel, 0, 5);
            this.SnmpTableLayoutPanel.Name = "SnmpTableLayoutPanel";
            // 
            // DescLabel
            // 
            resources.ApplyResources(this.DescLabel, "DescLabel");
            this.DescLabel.Name = "DescLabel";
            // 
            // SnmpServiceStatusPanel
            // 
            resources.ApplyResources(this.SnmpServiceStatusPanel, "SnmpServiceStatusPanel");
            this.SnmpServiceStatusPanel.Controls.Add(this.EnableSnmpCheckBox, 0, 0);
            this.SnmpServiceStatusPanel.Controls.Add(this.ServiceStatusPicture, 1, 0);
            this.SnmpServiceStatusPanel.Controls.Add(this.ServiceStatusLabel, 2, 0);
            this.SnmpServiceStatusPanel.Name = "SnmpServiceStatusPanel";
            // 
            // EnableSnmpCheckBox
            // 
            resources.ApplyResources(this.EnableSnmpCheckBox, "EnableSnmpCheckBox");
            this.EnableSnmpCheckBox.Name = "EnableSnmpCheckBox";
            this.EnableSnmpCheckBox.UseVisualStyleBackColor = true;
            this.EnableSnmpCheckBox.CheckedChanged += new System.EventHandler(this.EnableSNMPCheckBox_CheckedChanged);
            // 
            // ServiceStatusPicture
            // 
            resources.ApplyResources(this.ServiceStatusPicture, "ServiceStatusPicture");
            this.ServiceStatusPicture.Image = global::XenAdmin.Properties.Resources._000_Alert2_h32bit_16;
            this.ServiceStatusPicture.Name = "ServiceStatusPicture";
            this.ServiceStatusPicture.TabStop = false;
            // 
            // ServiceStatusLabel
            // 
            resources.ApplyResources(this.ServiceStatusLabel, "ServiceStatusLabel");
            this.ServiceStatusLabel.Name = "ServiceStatusLabel";
            // 
            // DebugLogCheckBox
            // 
            resources.ApplyResources(this.DebugLogCheckBox, "DebugLogCheckBox");
            this.DebugLogCheckBox.Name = "DebugLogCheckBox";
            this.DebugLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // SwitchConfigLayoutPanel
            // 
            resources.ApplyResources(this.SwitchConfigLayoutPanel, "SwitchConfigLayoutPanel");
            this.SwitchConfigLayoutPanel.Controls.Add(this.SwitchConfigListBox, 0, 0);
            this.SwitchConfigLayoutPanel.Controls.Add(this.AddTrapButton, 1, 0);
            this.SwitchConfigLayoutPanel.Controls.Add(this.TestTrapButton, 1, 1);
            this.SwitchConfigLayoutPanel.Controls.Add(this.DeleteTrapButton, 1, 2);
            this.SwitchConfigLayoutPanel.Name = "SwitchConfigLayoutPanel";
            // 
            // SwitchConfigListBox
            // 
            resources.ApplyResources(this.SwitchConfigListBox, "SwitchConfigListBox");
            this.SwitchConfigListBox.Items.AddRange(new object[] {
            resources.GetString("SwitchConfigListBox.Items")});
            this.SwitchConfigListBox.Name = "SwitchConfigListBox";
            this.SwitchConfigLayoutPanel.SetRowSpan(this.SwitchConfigListBox, 3);
            this.SwitchConfigListBox.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // AddTrapButton
            // 
            resources.ApplyResources(this.AddTrapButton, "AddTrapButton");
            this.AddTrapButton.Name = "AddTrapButton";
            this.AddTrapButton.Click += new System.EventHandler(this.AddTrapButton_Click);
            // 
            // TestTrapButton
            // 
            resources.ApplyResources(this.TestTrapButton, "TestTrapButton");
            this.TestTrapButton.Name = "TestTrapButton";
            // 
            // DeleteTrapButton
            // 
            resources.ApplyResources(this.DeleteTrapButton, "DeleteTrapButton");
            this.DeleteTrapButton.Name = "DeleteTrapButton";
            this.DeleteTrapButton.Click += new System.EventHandler(this.DeleteTrapButton_Click);
            // 
            // FlexSettingGroupBox
            // 
            resources.ApplyResources(this.FlexSettingGroupBox, "FlexSettingGroupBox");
            this.FlexSettingGroupBox.Controls.Add(this.BasicConfigLayoutPanel);
            this.FlexSettingGroupBox.Name = "FlexSettingGroupBox";
            this.FlexSettingGroupBox.TabStop = false;
            // 
            // BasicConfigLayoutPanel
            // 
            resources.ApplyResources(this.BasicConfigLayoutPanel, "BasicConfigLayoutPanel");
            this.BasicConfigLayoutPanel.Controls.Add(this.SupportV2cCheckBox, 0, 0);
            this.BasicConfigLayoutPanel.Controls.Add(this.SnmpV2cPanel, 0, 1);
            this.BasicConfigLayoutPanel.Controls.Add(this.SupportV3CheckBox, 0, 2);
            this.BasicConfigLayoutPanel.Controls.Add(this.SnmpV3Panel, 0, 3);
            this.BasicConfigLayoutPanel.Name = "BasicConfigLayoutPanel";
            // 
            // SupportV2cCheckBox
            // 
            resources.ApplyResources(this.SupportV2cCheckBox, "SupportV2cCheckBox");
            this.SupportV2cCheckBox.Checked = true;
            this.SupportV2cCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SupportV2cCheckBox.Name = "SupportV2cCheckBox";
            this.SupportV2cCheckBox.UseVisualStyleBackColor = true;
            this.SupportV2cCheckBox.CheckedChanged += new System.EventHandler(this.SupportV2CheckBox_CheckedChanged);
            // 
            // SnmpV2cPanel
            // 
            resources.ApplyResources(this.SnmpV2cPanel, "SnmpV2cPanel");
            this.SnmpV2cPanel.Controls.Add(this.CommunityLabel, 0, 0);
            this.SnmpV2cPanel.Controls.Add(this.CommunityTextBox, 1, 0);
            this.SnmpV2cPanel.Name = "SnmpV2cPanel";
            // 
            // CommunityLabel
            // 
            resources.ApplyResources(this.CommunityLabel, "CommunityLabel");
            this.CommunityLabel.Name = "CommunityLabel";
            // 
            // CommunityTextBox
            // 
            resources.ApplyResources(this.CommunityTextBox, "CommunityTextBox");
            this.CommunityTextBox.Name = "CommunityTextBox";
            // 
            // SupportV3CheckBox
            // 
            resources.ApplyResources(this.SupportV3CheckBox, "SupportV3CheckBox");
            this.SupportV3CheckBox.Name = "SupportV3CheckBox";
            this.SupportV3CheckBox.UseVisualStyleBackColor = true;
            this.SupportV3CheckBox.CheckedChanged += new System.EventHandler(this.SupportV3CheckBox_CheckedChanged);
            // 
            // SnmpV3Panel
            // 
            resources.ApplyResources(this.SnmpV3Panel, "SnmpV3Panel");
            this.SnmpV3Panel.Controls.Add(this.UserNameLabel, 0, 0);
            this.SnmpV3Panel.Controls.Add(this.UserNameTextBox, 1, 0);
            this.SnmpV3Panel.Controls.Add(this.AuthenticationPasswordLabel, 0, 1);
            this.SnmpV3Panel.Controls.Add(this.AuthenticationPasswordLabelTextBox, 1, 1);
            this.SnmpV3Panel.Controls.Add(this.AuthenticationProtocolLabel, 0, 2);
            this.SnmpV3Panel.Controls.Add(this.AuthenticationProtocolComboBox, 1, 2);
            this.SnmpV3Panel.Controls.Add(this.PrivacyPasswordLabel, 0, 3);
            this.SnmpV3Panel.Controls.Add(this.PrivacyPasswordTextBox, 1, 3);
            this.SnmpV3Panel.Controls.Add(this.PrivacyProtocolLabel, 0, 4);
            this.SnmpV3Panel.Controls.Add(this.PrivacyProtocolComboBox, 1, 4);
            this.SnmpV3Panel.Name = "SnmpV3Panel";
            // 
            // UserNameLabel
            // 
            resources.ApplyResources(this.UserNameLabel, "UserNameLabel");
            this.UserNameLabel.Name = "UserNameLabel";
            // 
            // UserNameTextBox
            // 
            resources.ApplyResources(this.UserNameTextBox, "UserNameTextBox");
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.TextChanged += new System.EventHandler(this.V3Block_Changed);
            // 
            // AuthenticationPasswordLabel
            // 
            resources.ApplyResources(this.AuthenticationPasswordLabel, "AuthenticationPasswordLabel");
            this.AuthenticationPasswordLabel.Name = "AuthenticationPasswordLabel";
            // 
            // AuthenticationPasswordLabelTextBox
            // 
            resources.ApplyResources(this.AuthenticationPasswordLabelTextBox, "AuthenticationPasswordLabelTextBox");
            this.AuthenticationPasswordLabelTextBox.Name = "AuthenticationPasswordLabelTextBox";
            this.AuthenticationPasswordLabelTextBox.TextChanged += new System.EventHandler(this.EncryptTextBox_TextChanged);
            // 
            // AuthenticationProtocolLabel
            // 
            resources.ApplyResources(this.AuthenticationProtocolLabel, "AuthenticationProtocolLabel");
            this.AuthenticationProtocolLabel.Name = "AuthenticationProtocolLabel";
            // 
            // AuthenticationProtocolComboBox
            // 
            resources.ApplyResources(this.AuthenticationProtocolComboBox, "AuthenticationProtocolComboBox");
            this.AuthenticationProtocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthenticationProtocolComboBox.FormattingEnabled = true;
            this.AuthenticationProtocolComboBox.Items.AddRange(new object[] {
            resources.GetString("AuthenticationProtocolComboBox.Items"),
            resources.GetString("AuthenticationProtocolComboBox.Items1")});
            this.AuthenticationProtocolComboBox.Name = "AuthenticationProtocolComboBox";
            this.AuthenticationProtocolComboBox.SelectedIndexChanged += new System.EventHandler(this.V3Block_Changed);
            // 
            // PrivacyPasswordLabel
            // 
            resources.ApplyResources(this.PrivacyPasswordLabel, "PrivacyPasswordLabel");
            this.PrivacyPasswordLabel.Name = "PrivacyPasswordLabel";
            // 
            // PrivacyPasswordTextBox
            // 
            resources.ApplyResources(this.PrivacyPasswordTextBox, "PrivacyPasswordTextBox");
            this.PrivacyPasswordTextBox.Name = "PrivacyPasswordTextBox";
            this.PrivacyPasswordTextBox.TextChanged += new System.EventHandler(this.EncryptTextBox_TextChanged);
            // 
            // PrivacyProtocolLabel
            // 
            resources.ApplyResources(this.PrivacyProtocolLabel, "PrivacyProtocolLabel");
            this.PrivacyProtocolLabel.Name = "PrivacyProtocolLabel";
            // 
            // PrivacyProtocolComboBox
            // 
            resources.ApplyResources(this.PrivacyProtocolComboBox, "PrivacyProtocolComboBox");
            this.PrivacyProtocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrivacyProtocolComboBox.FormattingEnabled = true;
            this.PrivacyProtocolComboBox.Items.AddRange(new object[] {
            resources.GetString("PrivacyProtocolComboBox.Items"),
            resources.GetString("PrivacyProtocolComboBox.Items1")});
            this.PrivacyProtocolComboBox.Name = "PrivacyProtocolComboBox";
            this.PrivacyProtocolComboBox.SelectedIndexChanged += new System.EventHandler(this.V3Block_Changed);
            // 
            // RetrieveSnmpPanel
            // 
            resources.ApplyResources(this.RetrieveSnmpPanel, "RetrieveSnmpPanel");
            this.RetrieveSnmpPanel.Controls.Add(this.RetrieveSnmpPicture, 0, 0);
            this.RetrieveSnmpPanel.Controls.Add(this.RetrieveSnmpLabel, 1, 0);
            this.RetrieveSnmpPanel.Name = "RetrieveSnmpPanel";
            // 
            // RetrieveSnmpPicture
            // 
            resources.ApplyResources(this.RetrieveSnmpPicture, "RetrieveSnmpPicture");
            this.RetrieveSnmpPicture.Name = "RetrieveSnmpPicture";
            this.RetrieveSnmpPicture.TabStop = false;
            // 
            // RetrieveSnmpLabel
            // 
            resources.ApplyResources(this.RetrieveSnmpLabel, "RetrieveSnmpLabel");
            this.RetrieveSnmpLabel.AutoEllipsis = true;
            this.RetrieveSnmpLabel.Name = "RetrieveSnmpLabel";
            // 
            // GeneralConfigureGroupBox
            // 
            resources.ApplyResources(this.GeneralConfigureGroupBox, "GeneralConfigureGroupBox");
            this.GeneralConfigureGroupBox.Controls.Add(this.GeneralConfigTableLayoutPanel);
            this.GeneralConfigureGroupBox.Name = "GeneralConfigureGroupBox";
            this.GeneralConfigureGroupBox.TabStop = false;
            // 
            // GeneralConfigTableLayoutPanel
            // 
            resources.ApplyResources(this.GeneralConfigTableLayoutPanel, "GeneralConfigTableLayoutPanel");
            this.GeneralConfigTableLayoutPanel.Name = "GeneralConfigTableLayoutPanel";
            // 
            // SnmpEditPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.SnmpTableLayoutPanel);
            this.Name = "SnmpEditPage";
            this.SnmpTableLayoutPanel.ResumeLayout(false);
            this.SnmpTableLayoutPanel.PerformLayout();
            this.SnmpServiceStatusPanel.ResumeLayout(false);
            this.SnmpServiceStatusPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServiceStatusPicture)).EndInit();
            this.SwitchConfigLayoutPanel.ResumeLayout(false);
            this.FlexSettingGroupBox.ResumeLayout(false);
            this.FlexSettingGroupBox.PerformLayout();
            this.BasicConfigLayoutPanel.ResumeLayout(false);
            this.BasicConfigLayoutPanel.PerformLayout();
            this.SnmpV2cPanel.ResumeLayout(false);
            this.SnmpV2cPanel.PerformLayout();
            this.SnmpV3Panel.ResumeLayout(false);
            this.SnmpV3Panel.PerformLayout();
            this.RetrieveSnmpPanel.ResumeLayout(false);
            this.RetrieveSnmpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RetrieveSnmpPicture)).EndInit();
            this.GeneralConfigureGroupBox.ResumeLayout(false);
            this.GeneralConfigureGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TableLayoutPanel SnmpTableLayoutPanel;
        private GroupBox GeneralConfigureGroupBox;
        private TableLayoutPanel GeneralConfigTableLayoutPanel;
        private Label DescLabel;
        private TableLayoutPanel SnmpServiceStatusPanel;
        private CheckBox EnableSnmpCheckBox;
        private PictureBox ServiceStatusPicture;
        private Label ServiceStatusLabel;
        private CheckBox DebugLogCheckBox;
        private TableLayoutPanel SwitchConfigLayoutPanel;
        private ListBox SwitchConfigListBox;
        private Button AddTrapButton;
        private Button TestTrapButton;
        private Button DeleteTrapButton;
        private GroupBox FlexSettingGroupBox;
        private TableLayoutPanel BasicConfigLayoutPanel;
        private CheckBox SupportV2cCheckBox;
        private TableLayoutPanel SnmpV2cPanel;
        private Label CommunityLabel;
        private TextBox CommunityTextBox;
        private CheckBox SupportV3CheckBox;
        private TableLayoutPanel SnmpV3Panel;
        private Label UserNameLabel;
        private TextBox UserNameTextBox;
        private Label AuthenticationPasswordLabel;
        private TextBox AuthenticationPasswordLabelTextBox;
        private Label AuthenticationProtocolLabel;
        private ComboBox AuthenticationProtocolComboBox;
        private Label PrivacyPasswordLabel;
        private TextBox PrivacyPasswordTextBox;
        private Label PrivacyProtocolLabel;
        private ComboBox PrivacyProtocolComboBox;
        private TableLayoutPanel RetrieveSnmpPanel;
        private PictureBox RetrieveSnmpPicture;
        private Label RetrieveSnmpLabel;
    }
}