# DbHelper

[![.NET Test](https://github.com/huurou/DbHelper/actions/workflows/test.yml/badge.svg)](https://github.com/huurou/DbHelper/actions/workflows/test.yml)

## 概要
RDBへの接続・クエリの発行・コマンドの実行をラップしたヘルパークラスです。
`System.Data.Common`のDb系クラスを継承した各種プロバイダー向けの実装を含みます。

**例**
- `Oracle.ManagedDataAccess.Client`を使用したOracleHelper
- `Npgsql`を使用したPostgresHelper
