using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlLibrary
{
    public partial class AnimationTrackBar: UserControl
    {
        public AnimationTrackBar()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;

            AnimStop_Btn.Enabled = false;

            CANM_AnimTrackBar.Value = int.Parse(CurrentAnimPosValue_TXT.Text.ToString());
            CANM_AnimTrackBar.Maximum = int.Parse(MaxAnimRangeValue_TXT.Text.ToString());

            //1 frame = 60ms
            //60ms = 0.03m
            CANM_AnimationTimer.Interval = 60;
        }

        public bool IsEnabled { get; set; } = true;
        public bool IsLoop { get; set; } = false;

        /// <summary>
        /// 現在のアニメーション位置
        /// </summary>
        public int BarCurrentValue
        {
            get
            {
                return int.Parse(CurrentAnimPosValue_TXT.Text);
            }
            set
            {
                CurrentAnimPosValue_TXT.Text = value.ToString();
            }
        }

        /// <summary>
        /// 最大フレーム数
        /// </summary>
        public int BarMaxValue
        {
            get
            {
                return int.Parse(MaxAnimRangeValue_TXT.Text);
            }
            set
            {
                MaxAnimRangeValue_TXT.Text = value.ToString();
            }
        }

        private void AnimationTrackBar_Load(object sender, EventArgs e)
        {
            //CANM_AnimTrackBar.Value = int.Parse(CurrentAnimPosValue_TXT.Text.ToString());
            //CANM_AnimTrackBar.Maximum = int.Parse(MaxAnimRangeValue_TXT.Text.ToString());
        }

        private void MaxAnimRangeValue_TXT_TextChanged(object sender, EventArgs e)
        {
            int value = int.Parse(MaxAnimRangeValue_TXT.Text.ToString());
            if (value < 0) MessageBox.Show("Error : -1");
            CANM_AnimTrackBar.Maximum = int.Parse(MaxAnimRangeValue_TXT.Text.ToString());
        }

        private void CurrentAnimPosValue_TXT_TextChanged(object sender, EventArgs e)
        {
            int value = int.Parse(CurrentAnimPosValue_TXT.Text.ToString());
            CANM_AnimTrackBar.Value = value;
        }

        private void AnimStop_Btn_Click(object sender, EventArgs e)
        {
            AnimStop_Btn.Enabled = false;
            AnimPlay_Btn.Enabled = true;

            CANM_AnimationTimer.Stop();
        }

        private void AnimPlay_Btn_Click(object sender, EventArgs e)
        {
            AnimPlay_Btn.Enabled = false;
            AnimStop_Btn.Enabled = true;

            CANM_AnimationTimer.Start();
        }

        private void CANM_AnimTrackBar_Scroll(object sender, EventArgs e)
        {
            CurrentAnimPosValue_TXT.Text = CANM_AnimTrackBar.Value.ToString();
        }

        private void CANM_AnimTrackBar_ValueChanged(object sender, EventArgs e)
        {
            CurrentAnimPosValue_TXT.Text = CANM_AnimTrackBar.Value.ToString();
        }

        private void CANM_AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (CANM_AnimationTimer.Enabled == true)
            {
                if (IsLoop == true)
                {
                    if (CANM_AnimTrackBar.Value < int.Parse(MaxAnimRangeValue_TXT.Text))
                    {
                        CANM_AnimTrackBar.Value += 1;
                    }
                    else
                    {
                        CANM_AnimTrackBar.Value = 0;
                    }
                }
                else if (IsLoop == false)
                {
                    if (CANM_AnimTrackBar.Value < int.Parse(MaxAnimRangeValue_TXT.Text))
                    {
                        CANM_AnimTrackBar.Value += 1;
                    }
                    else
                    {
                        AnimStop_Btn.Enabled = false;
                        AnimPlay_Btn.Enabled = true;

                        //Reset
                        CANM_AnimTrackBar.Value = 0;

                        CANM_AnimationTimer.Stop();
                    }
                }

            }
            else if(CANM_AnimationTimer.Enabled == false)
            {
                //DisableAnimation
            }
        }
    }
}
