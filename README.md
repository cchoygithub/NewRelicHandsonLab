New Relic Handson教材
---

このリポジトリはNew Relicの製品を実際に試すためのサンプルアプリと手順をまとめたものです。New Relicの各製品が有効なアカウントが必要です。
現状 ASP.NET CoreアプリをLinux上で動かすことを想定しています。

## 準備

1. Linuxホストの準備 (.NET Core 3.1がサポートされているディストリビューション、手順はUbuntu 18.04 LTSで確認しています)

2. .NET Core 3.1 SDKのインストール
  
    [ドキュメント](https://dotnet.microsoft.com/download/dotnet-core/3.1)にしたがってください。Ubuntu 18.04 LTSの場合は次のコマンドです。
    
    ```bash
    wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    
    sudo add-apt-repository universe
    sudo apt-get update
    sudo apt-get install apt-transport-https
    sudo apt-get update
    sudo apt-get install dotnet-sdk-3.1
    ```
    
3. ローカルホストへのPostgreSQL 11のインストール

    Ubuntu 18.04 LTSの場合は次のようになります。

    ```
    echo 'deb http://apt.postgresql.org/pub/repos/apt/ bionic-pgdg main' | sudo tee /etc/apt/sources.list.d/pgdg.list
    
    wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
    sudo apt-get update
    sudo apt-get install postgresql-11	 pgadmin4
    sudo systemctl enable postgresql
    sudo systemctl start postgresql

    sudo su - postgres
    psql
    ```

    psqlで以下のSQLを実行し、ユーザーとパスワードを発行します。ユーザー名、パスワード名を変更する場合は、アプリケーションの接続文字列も変更してください。

    ```
    CREATE DATABASE tododb;
    CREATE ROLE todouser WITH LOGIN PASSWORD 'P@ssW0rd!';
    GRANT ALL PRIVILEGES ON DATABASE tododb to todouser;
    ```

3. New Relic Infrastructure Agentのインストール
  
    [ドキュメント]()にしたがってください。Ubuntu 18.04 LTSの場合は次のコマンドです。New Relicライセンスキーの設定が必要です。
    
    ```bash
    echo "license_key: <ライセンスキー>"| sudo tee -a /etc/newrelic-infra.yml
    curl https://download.newrelic.com/infrastructure_agent/gpg/newrelic-infra.gpg | sudo apt-key add -
    printf "deb [arch=amd64] https://download.newrelic.com/infrastructure_agent/linux/apt bionic main" | sudo tee -a /etc/apt/sources.list.d/newrelic-infra.list
    sudo apt-get update
    sudo apt-get install newrelic-infra -y
    systemctl restart newrelic-infra.service
    ```

4. New Relic .NET Core Agentのインストールをします。[ドキュメント](https://docs.newrelic.co.jp/docs/agents/net-agent/installation/install-net-agent-linux) にしたがってください。 

5. このリポジトリのclone

## アプリの起動とNew Relic APMの検証

1. New Relic APMが有効なアカウントでライセンスキーを取得します。

2. 準備の際にcloneしたこのリポジトリのフォルダに移動し、次のコマンドでビルドします。

    ```bash
    cd <リポジトリのパス>
    dotnet publish -c Release -o Publish/StoreService StoreService/
    dotnet publish -c Release -o Publish/WebPortal WebPortal/
    ```

3. 別のターミナルを二つ開いて、取得したライセンスキーとアプリ名を指定してアプリを起動します。アプリ名はテストで同じアカウントを共有する場合は、一意になるように適宜prefixやpostfixをつけてください。プロセスがListenするポートは任意で構いませんが、WebPortalアプリはStoreServiceアプリに依存しているため、`STORE_SVC_URL`環境変数を適宜変更してください。

    ```bash
    cd <リポジトリのパス>/Publish/StoreService

    NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true NEW_RELIC_LICENSE_KEY=<ライセンスキー> NEW_RELIC_APP_NAME=StoreService-<PREFIX> $CORECLR_NEWRELIC_HOME/run.sh dotnet /home/clouduser/NewRelicHandsonLab/StoreService/bin/Release/netcoreapp3.1/StoreService.dll --urls "http://*:8080"
    ```

    ```bash
    cd <リポジトリのパス>/Publish/WebPortal

    NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true NEW_RELIC_LICENSE_KEY=<ライセンスキー> NEW_RELIC_APP_NAME=WebPortal-<PREFIX> STORE_SVC_URL=http://localhost:8080 $CORECLR_NEWRELIC_HOME/run.sh dotnet /home/clouduser/NewRelicHandsonLab/WebPortal/bin/Release/netcoreapp3.1/WebPortal.dll --urls "http://*:8081" 
    ```

4. WebPortalのTasksページに何度かリクエストを発行してください。

   ```bash
   curl http://localhost:8081/Tasks
   ```

5. 5分ほどすればNew Relic APMの画面で二つのアプリが表示されるはずです。

## New Relic APM

### New Relic APMに二つアプリが表示されていることを確認

### WebPortalではExternalが、StoreServiceではPostgesが表示されていることを確認

### WebPortalのTransactionからStoreServiceのTransactionに飛べることを確認

### StoreServiceのTransaction TraceでDatabase Queryが表示されていることを確認
