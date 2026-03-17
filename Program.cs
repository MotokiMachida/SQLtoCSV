using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. 設定ファイルを読み込む準備
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // 実行フォルダを指定
                .AddJsonFile("jsconfig1.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. JSONの中から接続文字列を取り出す
            string connStr = configuration.GetConnectionString("SqlServer");

            if (string.IsNullOrEmpty(connStr))
            {
                Console.WriteLine("エラー：接続文字列が jsconfig1.json に見つかりません。");
                return;
            }

            // 3. クラス化した処理を呼び出す
            var dbService = new SqlServerService(connStr);

            try
            {
                dbService.Connect();

                // 1. テーブル作成とデータ挿入
                dbService.CreateTestTable();
                dbService.InsertTestData("Hello User", "test@example.com");
                dbService.InsertTestData("Taro San", "Taro@example.com");

                // 2. CSV出力
                string csvPath = "output_users.csv";
                dbService.ExportToCsv("SELECT * FROM Users", csvPath);

                dbService.Disconnect();

                Console.WriteLine("全工程が完了しました。フォルダを確認してください。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }
    }
}