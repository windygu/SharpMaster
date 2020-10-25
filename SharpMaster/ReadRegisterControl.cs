﻿
using System;
using System.Windows.Forms;
using SharpModbus;

namespace SharpMaster
{
	public partial class ReadRegisterControl : UserControl, IoControl
	{
		private readonly ControlContext context;
		
		public ReadRegisterControl(ControlContext context, SerializableMap settings)
		{
			this.context = context;
			
			InitializeComponent();
			
			numericUpDownSlaveAddress.Value = settings.GetNumber("slaveAddress", 0);
			numericUpDownRegisterAddress.Value = settings.GetNumber("startAddress", 0);
			comboBoxFunctionCode.Text = settings.GetString("functionCode", "3 Holding");
			if (comboBoxFunctionCode.SelectedIndex < 0)
				comboBoxFunctionCode.SelectedIndex = 0;
		}
		
		public SerializableMap GetSettings()
		{
			var settings = new SerializableMap();
			settings.AddAny("slaveAddress", numericUpDownSlaveAddress.Value);
			settings.AddAny("startAddress", numericUpDownRegisterAddress.Value);
			settings.AddAny("functionCode", comboBoxFunctionCode.Text);
			return settings;
		}
		
		public void Enable(bool enabled)
		{
			buttonRead.Enabled = enabled;        	
        }

        public void Perform()
        {
            buttonRead.PerformClick();
        }

        void ButtonReadClick(object sender, EventArgs e)
		{
			var slaveAddress = (byte)numericUpDownSlaveAddress.Value;
			var startAddress = (ushort)numericUpDownRegisterAddress.Value;
			var functionCode = comboBoxFunctionCode.SelectedIndex;
			context.Io((master) => {
                var value = functionCode == 0 ? 
				master.ReadHoldingRegister(slaveAddress, startAddress) : 
				master.ReadInputRegister(slaveAddress, startAddress);
				context.Ui(() => {
					labelRegisterValue.Text = value.ToString();
				});
			});
		}
	}
}
