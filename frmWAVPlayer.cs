using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WAVPlayer
{
    public partial class frmWAVPlayer : Form
    {
        SoundPlayer player = new SoundPlayer();

        public frmWAVPlayer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 當使用者按下 瀏覽 按鈕時 開啟檔案對話框讓讀者選擇WAV檔案 並將選擇的檔案路徑顯示在txtFilePath文字框中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // 過濾條件設定為WAV檔案
            this.ofdWAVFile.Filter = "WAV Files(*.wav)|*.wav";
            // 打開檔案對話方塊
            if (this.ofdWAVFile.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.ofdWAVFile.FileName;

                btnPlay.Enabled = true;
                btnLoop.Enabled = true;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPath.Text) || !File.Exists(txtPath.Text))
            {
                MessageBox.Show("請先選擇有效的 WAV 檔案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                player.SoundLocation = txtPath.Text; // 指定音效所在路徑檔名
                player.Load(); // 載入音效檔資料
                player.Play(); // 播放音效
                //player.PlaySync();
                //MessageBox.Show("音效播放完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show("無法播放音效檔，請確認檔案路徑是否正確");
            }
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            // 檢查路徑是否為空，且檔案是否存在
            if (string.IsNullOrEmpty(txtPath.Text) || !File.Exists(txtPath.Text))
            {
                MessageBox.Show("請先選擇有效的 WAV 檔案再進行重複播放！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                player.SoundLocation = txtPath.Text;
                player.PlayLooping(); // 開始循環播放
            }
            catch (Exception ex)
            {
                MessageBox.Show("重複播放失敗：" + ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // 檢查是否根本還沒選檔案
            if (string.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show("目前沒有載入任何音效，無需停止。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                player.Stop(); // 停止播放
                               // MessageBox.Show("音效已停止"); // 這行看你需求，通常停止按鈕不需要特別跳視窗，直接停掉就好
            }
            catch (Exception ex)
            {
                MessageBox.Show("停止時發生錯誤：" + ex.Message);
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void frmWAVPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("確定要關閉應用程式嗎？", "關閉確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true; // 取消關閉
            }
            else
            {
                player.Stop(); // 關閉視窗時順便停止音樂
                player.Dispose(); // 釋放資源
            }
        }
    }
}
