using Microsoft.Data.SqlClient;
using System.Text;

namespace ConsoleApp3
{
    public class SqlServerService : IDatabaseService
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        // コンストラクタ（クラスを作る時に接続文字列を受け取る）
        public SqlServerService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Connect()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
                Console.WriteLine("SQL Server に接続しました。");
            }
        }

        public void Disconnect()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                Console.WriteLine("SQL Server から切断しました。");
            }
        }
        public void CreateTestTable()
        {
            // SQL Server に「もし無ければテーブルを作る」という命令を送る
            string sql = @"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
        CREATE TABLE Users (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Name NVARCHAR(50),
            Email NVARCHAR(50),
            CreatedAt DATETIME DEFAULT GETDATE()
        );";

            using (var command = new SqlCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("テスト用テーブル 'Users' を作成（または確認）しました。");
            }
        }
        public void InsertTestData(string name, string email)
        {
            string sql = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";

            using (var command = new SqlCommand(sql, _connection))
            {
                // SQLインジェクションを防ぐための「パラメータ」指定
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);

                command.ExecuteNonQuery();
                Console.WriteLine($"{name} さんのデータを登録しました。");
            }
        }
        public void ExportToCsv(string query, string filePath)
        {
            try
            {
                using (var command = new SqlCommand(query, _connection))
                using (var reader = command.ExecuteReader())
                {
                    using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        // 1. ヘッダーへ書き込む
                        var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName);
                        writer.WriteLine(string.Join(",", columns));

                        // 2. データを一行ずつ書き込む
                        while (reader.Read())
                        {
                            var values = Enumerable.Range(0, reader.FieldCount)
                                                   .Select(i => reader.GetValue(i).ToString());
                            writer.WriteLine(string.Join(",", values));
                        }
                    }
                }
                Console.WriteLine($"CSV出力成功: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CSV出力エラー: {ex.Message}");
            }
        }
    }
    
}