WAV Player (音效播放器)
這是一個基於 C# Windows Forms 開發的簡單音效播放器，支援 .wav 檔案的播放、循環播放與停止功能，並針對常見的使用者操作錯誤加入了防呆檢查。

🛠 主要功能
瀏覽檔案：透過檔案對話方塊選擇本地端的 .wav 音訊檔。

播放一次：載入音效並播放。

重複播放：自動循環播放選定的音效檔。

停止播放：隨時中止目前正在播放的聲音。

防呆機制：

路徑檢查：若未選取檔案或檔案不存在時按下播放按鈕，系統會彈出警告視窗，避免程式當機。

資源管理：在關閉程式前會彈出確認視窗，並確保播放器資源被正確釋放。

一開始的執行畫面

<img width="1140" height="567" alt="image" src="https://github.com/user-attachments/assets/052321b7-bd76-4281-97e4-36eeef027207" />

加入wav檔案之後

<img width="1125" height="547" alt="image" src="https://github.com/user-attachments/assets/19d6514c-5837-47de-b189-64b8d4565155" />

關閉檔案時

<img width="1134" height="563" alt="image" src="https://github.com/user-attachments/assets/241ee84a-9a16-4679-9205-b7e293158d7e" />
