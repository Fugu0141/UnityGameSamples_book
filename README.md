# UnityGameSamples_book

『Unity × GitHub 実践ゲーム制作入門』の教材用サンプルコードです。

> **作りながら学ぶ**：第1章でプレイヤーを配置し、第2章で動かし、弾・敵・スコアを追加しながら2Dシューティングを完成させます。第13章以降は、このゲームをGitHubで管理・改良する練習に使います。

## 対応環境

- Unity **6.3 LTS（6000.3系）**（Windows 11を基準）
- Unity Input System（従来の `UnityEngine.Input` は使用しません）
- VS Code + C# / Unity拡張機能
- GitHub Desktop（本書第2部）

## サンプルを開く

1. リポジトリをCloneし、Unity Hubで `UnityProject` フォルダを開きます。
2. 初回にUnity Package Managerの解決・アセットのインポートが完了するまで進めます。もし最小構成のProjectSettingsが原因で開けない場合は、Unity Hubで新しい2Dプロジェクトを作成し、`UnityProject/Assets/BookSamples` をそのプロジェクトの `Assets` へコピーしてください（Input SystemとuGUIをPackage Managerで導入）。
3. Unityのメニュー **Tools → Book Samples → Generate Sample Scenes** を選び、章ごとのシーンと教材用Prefabを生成します。
4. 第8章以降でTextMeshProのフォントが表示されない場合、Unityのメニューから **Window → TextMeshPro → Import TMP Essential Resources** を実行してください。
5. `Assets/BookSamples/Scenes/Chapter01.unity` を開きます。第1章ではプレイヤーの配置、色、位置、シーンの保存を試しましょう。
6. 第2章の見本は `Chapter02.unity`、最終ゲームの見本は `Chapter12.unity` です。Gameビューで **Play** して確認します。

**注意：** SceneとPrefabはUnity Editor側で生成します。生成後のアセットと `.meta` はGitで追跡できます。生成コマンドを再実行すると教材用Scene/Prefabが上書きされるため、学習者が編集したものは別名で保存してください。

## 操作方法

- 移動：WASD / 矢印キー
- 攻撃：Space（第4章以降）
- リトライ：R（第9章以降）
- 第1章はコードなしで画面配置・保存・再生を体験します。

## ソースコード

- `UnityProject/Assets/BookSamples/Scripts/PlayerMovement.cs` — 第2・3章：移動、入力、変数
- `.../PlayerShooter.cs` / `Bullet.cs` — 第4章：弾の生成と移動
- `.../Enemy.cs` — 第5章：当たり判定
- `.../EnemySpawner.cs` — 第6・7章：自動出現とウェーブ
- `.../GameSession.cs` — 第8・9章：スコア、HP、ゲームオーバー
- `.../FastEnemy.cs` — 第10章：継承とオーバーライド
- `.../Editor/BookSceneBuilder.cs` — 教材見本のSceneとPrefabを作る**編集用補助ツール**（本文で写すコードではありません）

章との対応・執筆に使うチェックポイントは [docs/chapter-checkpoints.md](docs/chapter-checkpoints.md) を参照してください。

## GitHubでの管理方針

Unityプロジェクトで必要な `Assets/`（`.meta`を含む）、`Packages/`、`ProjectSettings/` は管理し、`Library/` や `Temp/`、`Obj/` は管理しません。`Packages/manifest.json` に加え、Unityが生成する `Packages/packages-lock.json` も追跡してください。大きな制作素材のみ必要に応じてGit LFSへ登録します。

## 検証状況

ソースとUnity Editor用シーン生成機能を用意していますが、ここではUnity Editor本体によるインポート・コンパイル・Play確認は未実施です。まず実機でUnity 6.3 LTSを使って検証し、教材のスクリーンショットは確認した画面を使ってください。

## ライセンス

現時点ではライセンスを未設定です。公開・再配布方針が決まった段階で追加します。
