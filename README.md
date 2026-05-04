# WAV Player (音效播放器)

這是一個基於 C# Windows Forms 開發的簡單音效播放器，支援 .wav 檔案的播放、循環播放與停止功能，並針對常見的使用者操作錯誤加入了防呆檢查。

🛠 主要功能

瀏覽檔案：透過檔案對話方塊選擇本地端的 .wav 音訊檔。

播放一次：載入音效並播放。

重複播放：自動循環播放選定的音效檔。

停止播放：隨時中止目前正在播放的聲音。

視覺化進度條：使用 ProgressBar 呈現播放百分比。

動態時間標籤：即時更新 00:00 / 總時長

直覺式拖放互動：實作 DragEnter 與 DragDrop 事件，使用者可將檔案直接拖入表單，系統會自動過濾非 .wav 格式，並於拖入後立即解析時長。

防呆機制：若未選取有效檔案，播放與循環按鈕將保持停用狀態，防止程式異常。

資源管理：在關閉程式前會彈出確認視窗，並確保播放器資源被正確釋放。

一開始的執行畫面：程式初始化狀態，播放與循環按鈕預設為停用（Enabled = false），確保使用者必須先載入檔案。

<img width="1298" height="703" alt="image" src="https://github.com/user-attachments/assets/42210807-4688-4095-ab24-fe1d3bd216a9" />

加入wav檔案之後：載入檔案後，透過解析標頭即時顯示總時長，並開啟播放功能。

<img width="1299" height="698" alt="image" src="https://github.com/user-attachments/assets/7e7d1d7b-18dd-4ad1-9e5c-10398b4fc01c" />

關閉檔案時：實作 FormClosing 事件，透過彈出視窗確保使用者意圖並正確釋放資源。

<img width="1321" height="701" alt="image" src="https://github.com/user-attachments/assets/27b56e50-7bd8-4898-9e05-f335152557e6" />

防呆機制，當系統偵測到未載入音訊流時按下停止，會觸發異常處理邏輯並跳出警告提示。

<img width="1295" height="711" alt="image" src="https://github.com/user-attachments/assets/15569578-f4c1-449d-8dab-5bb27c2dab25" />

