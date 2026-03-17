using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public interface IDatabaseService
    {
        // DBに接続する
        void Connect();

        // データを取得してCSVにする
        void ExportToCsv(string query, string filePath);

        // 切断する
        void Disconnect();
    }
}
