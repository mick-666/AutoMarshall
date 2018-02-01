using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMarshal.Properties;

namespace AutoMarshal
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.Text = Settings.Default.settingFormTitle;
            this.ReadSettings();
        }

        //читаем настройки app.config
        private void ReadSettings()
        {
            tbBaseURI.Text = Settings.Default.BaseURI;
            tbVehiclesURI.Text = Settings.Default.VehiclesUriTemplate;
            tbImageURI.Text = Settings.Default.ImageUriTemplate;
            updownPeriod.Value = Settings.Default.DaysBefore;
            updownRowLimit.Value = Settings.Default.VehiclesLimit;
        }

        //сохраняем изменения
        private void WriteSettings()
        {
            Settings.Default.BaseURI = tbBaseURI.Text.Trim();
            Settings.Default.VehiclesUriTemplate = tbVehiclesURI.Text.Trim();
            Settings.Default.ImageUriTemplate = tbImageURI.Text.Trim();
            Settings.Default.DaysBefore = (int)updownPeriod.Value;
            Settings.Default.VehiclesLimit = (int)updownRowLimit.Value;
            Settings.Default.Save();
        }

        public static DialogResult ShowSettingsForm()
        {
            SettingsForm settingsForm = new SettingsForm();
            DialogResult result = settingsForm.ShowDialog();
            if (result == DialogResult.OK) settingsForm.WriteSettings();
            return result;
        }

    }
}
