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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WAVPlayer
{
    public partial class frmWAVPlayer : Form
    {
        SoundPlayer player = new SoundPlayer();
        Timer playbackTimer = new Timer();
        int currentSeconds = 0;
        int totalSeconds = 0;
        bool isLooping = false; // 紀錄目前是否為循環播放模式

        public frmWAVPlayer()
        {
            InitializeComponent();
            this.AllowDrop = true;
            // 綁定事件
            this.DragEnter += new DragEventHandler(frmWAVPlayer_DragEnter);
            this.DragDrop += new DragEventHandler(frmWAVPlayer_DragDrop);

            playbackTimer.Interval = 1000; // 每秒跳動一次
            playbackTimer.Tick += PlaybackTimer_Tick;

            UpdateTimerLabel();
        }

        private void UpdateTimerLabel()
        {
            lblTime.Text = $"{FormatTime(currentSeconds)} / {FormatTime(totalSeconds)}";
        }

        private string FormatTime(int seconds)
        {
            // 計算分鐘與秒數
            int mins = seconds / 60;
            int secs = seconds % 60;

            // 回傳格式化字串，D2 表示不足兩位數自動補 0
            return string.Format("{0:D2}:{1:D2}", mins, secs);
        }

        /// <summary>
        /// 當使用者按下 瀏覽 按鈕時 開啟檔案對話框讓讀者選擇WAV檔案 並將選擇的檔案路徑顯示在txtFilePath文字框中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.ofdWAVFile.Filter = "WAV Files(*.wav)|*.wav";
            if (this.ofdWAVFile.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.ofdWAVFile.FileName;

                // 預先取得長度，讓使用者一選完檔案就能看到總時間
                totalSeconds = GetWavDuration(txtPath.Text);
                currentSeconds = 0;
                UpdateTimerLabel();

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
                // 1. 停止之前的計時與播放
                playbackTimer.Stop();
                player.Stop();

                // 2. 載入並播放音樂
                isLooping = false;
                player.SoundLocation = txtPath.Text;
                player.Load();
                player.Play();

                // 3. 更新進度條資訊
                totalSeconds = GetWavDuration(txtPath.Text);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = Math.Max(1, totalSeconds);
                progressBar1.Value = 0;
                currentSeconds = 0;

                // 4. 開始計時
                playbackTimer.Start();
                UpdateTimerLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("播放失敗：" + ex.Message);
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
                // 1. 停止之前的播放與計時
                playbackTimer.Stop();
                player.Stop();

                // 2. 開始循環播放
                isLooping = true;
                player.SoundLocation = txtPath.Text;
                player.PlayLooping();

                // 3. 重設進度條計時資訊
                totalSeconds = GetWavDuration(txtPath.Text);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = Math.Max(1, totalSeconds);
                progressBar1.Value = 0;
                currentSeconds = 0;

                // 4. 重新啟動計時器
                playbackTimer.Start();
                UpdateTimerLabel();
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
                playbackTimer.Stop();

                player.Stop(); // 停止播放音訊

                // 重設進度顯示
                progressBar1.Value = 0;
                currentSeconds = 0;
                isLooping = false;
                UpdateTimerLabel();
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
                playbackTimer.Stop();
                player.Stop(); // 關閉視窗時順便停止音樂
                player.Dispose(); // 釋放資源
            }
        }

        /// <summary>
        /// 當檔案拖入表單範圍時觸發
        /// </summary>
        private void frmWAVPlayer_DragEnter(object sender, DragEventArgs e)
        {
            // 檢查拖入的是否為檔案
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; // 顯示複製圖示
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 當使用者放開滑鼠完成拖曳時觸發
        /// </summary>
        private void frmWAVPlayer_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0];
                if (Path.GetExtension(filePath).ToLower() == ".wav")
                {
                    this.txtPath.Text = filePath;
                    totalSeconds = GetWavDuration(filePath);
                    currentSeconds = 0;
                    UpdateTimerLabel(); // 拖入後顯示總時長[cite: 1]

                    btnPlay.Enabled = true;
                    btnLoop.Enabled = true;
                }
            }
        }

        private int GetWavDuration(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    br.BaseStream.Seek(28, SeekOrigin.Begin);
                    int byteRate = br.ReadInt32();

                    br.BaseStream.Seek(40, SeekOrigin.Begin);
                    int dataSize = br.ReadInt32();

                    return byteRate > 0 ? dataSize / byteRate : 0; // 防止除以 0
                }
            }
            catch
            {
                return 0; // 若讀取失敗則回傳 0
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (currentSeconds < totalSeconds)
            {
                currentSeconds++;
                if (currentSeconds <= progressBar1.Maximum)
                {
                    progressBar1.Value = currentSeconds;
                }
            }

            UpdateTimerLabel();

            if (currentSeconds >= totalSeconds)
            {
                if (isLooping)
                {
                    currentSeconds = 0;
                    progressBar1.Value = 0;
                }
                else
                {
                    playbackTimer.Stop();
                }
            }
        }
    }
}
