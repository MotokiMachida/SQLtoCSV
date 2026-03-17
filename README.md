# SQL Server to CSV Export System

Docker上で動作するSQL Serverからデータを取得し、C#を用いてCSVファイルを出力する自動化ツールです。

##  概要
現場でのデータ抽出・転記作業を自動化することを想定したツールです。
Dockerを用いた環境構築から、C#によるDB操作、JSONによる設定管理までを組み合わせて実装しています。

##  使用技術
-   Language  : C# (.NET 9.0)
-   Database  : SQL Server (Docker Container)
-   Infrastructure  : Docker / Docker Compose
-   Library  : Microsoft.Data.SqlClient, System.Text.Json

##  フォルダ構成
- `src/`: C# ソースコード、プロジェクトファイル
- `docker/`: データベース環境構築用の docker-compose.yml
- `README.md`: 本ドキュメント

##  使い方
### 1. DBの起動
`docker/` フォルダへ移動し、以下のコマンドを実行します。
bash
docker-compose up -d
2. 設定ファイルの準備
src/jsConfig.json をコピーして src/jsconfig1.json を作成し、自身の接続環境（サーバー名、ユーザーID、パスワード等）に合わせて書き換えてください。

3. 実行
Visual Studio 2022 等でプロジェクトを開き、ビルド・実行します。設定した接続先からデータが取得され、指定したパスにCSVが出力されます。

工夫した点
インターフェースの活用
データベース操作をインターフェース化し、将来的にOracleなど他のDBへ切り替える際も、呼び出し側のコードを修正せず、最小限の工数で対応できるように設計しました。

環境のポータビリティ
Dockerを採用することで、開発者ごとに異なるDB環境の差異をなくし、どのPC環境でも docker-compose up 一発で同じ検証環境を構築できるようにしました。

セキュリティと保守性
機密情報（パスワード等）を含む設定ファイルをGitHubの管理から外す運用（.gitignore の活用と Example.json の同梱）を徹底し、安全な開発サイクルを意識しました。
