﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CGFXLibrary;

namespace CGFX_Viewer_SharpDX.Forms.General.UserDataForm
{
	public partial class UserDataDictionaryForm : Form
	{
		public List<CGFXFormat.CGFXData> userData_List { get; set; }

		public UserDataDictionaryForm(List<CGFXFormat.CGFXData> userDataDicts)
		{
			InitializeComponent();
			userData_List = userDataDicts;
		}

		Form1 Form;

		private void UserDataDictionaryForm_Load(object sender, EventArgs e)
		{
			Form = (Form1)Application.OpenForms["Form1"];

			if (userData_List.Count != 0)
			{
				List<string> UDList = new List<string>();

				for(int i = 0; i < userData_List.Count; i++)
				{
					if (userData_List[i].String_Data != null)
					{
                        UDList.Add(i + " : " + userData_List[i].String_Data.ToString());
                    }
                    else if (userData_List[i].Int32_Data != null)
                    {
                        UDList.Add(i + " : " + userData_List[i].Int32_Data.ToString());
                    }
                    else if (userData_List[i].RealNumber_Data != null)
                    {
                        UDList.Add(i + " : " + userData_List[i].RealNumber_Data.ToString());
                    }

                    //UDList.Add(i + " : " + userData_List[i].Type.ToString());
                }

				listBox1.Items.AddRange(UDList.ToArray());
			}
		}

		private void AddUserData_Btn_Click(object sender, EventArgs e)
		{

		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = new PropertyGridForms.General.UserDataForm.UserDataEntryPropertyGrid(userData_List[listBox1.SelectedIndex]);
		}
	}
}
