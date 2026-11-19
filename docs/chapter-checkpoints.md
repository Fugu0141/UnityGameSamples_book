# 章別チェックポイント（構成案 v0.4 対応）

本書は**ゲームを1つずつ改良する**構成です。以下の章別シーンは執筆・動作検証用の見本であり、学習者は第1章で新規2Dプロジェクトを作り、同じ動作を自分の手で組み立てることを想定しています。

| 章 | 生成されるシーン | 観察すること | メインで読むコード |
|---|---|---|---|
| 1 | Chapter01 | カメラとプレイヤーの配置・保存 | なし（GUI操作） |
| 2 | Chapter02 | WASD・矢印キーでプレイヤー移動 | PlayerMovement.cs |
| 3 | Chapter03 | 速度の数値変更とConsole | PlayerMovement.cs |
| 4 | Chapter04 | Spaceで弾の生成・削除 | PlayerShooter.cs, Bullet.cs |
| 5 | Chapter05 | 敵と弾のTrigger衝突 | Enemy.cs, Bullet.cs |
| 6 | Chapter06 | 敵の自動生成 | EnemySpawner.cs |
| 7 | Chapter07 | ウェーブ（List/繰り返し） | EnemySpawner.cs |
| 8 | Chapter08 | 命中時のスコアUI（TMP） | GameSession.cs |
| 9 | Chapter09 | 敵との接触でHP低下、Rでリトライ | GameSession.cs |
| 10 | Chapter10 | FastEnemy継承・移動速度の違い | FastEnemy.cs |
| 11 | Chapter11 | 演出のための色変化、音声用の受け口 | PlayerPulse.cs, PlayerShooter.cs |
| 12 | Chapter12 | ゲームの全体を確認してリファクタリング | すべて |

## 使い方

Unity Hubで `UnityProject` を開き、`Tools → Book Samples → Generate Sample Scenes` を実行してください。すると同プロジェクト内の `Assets/BookSamples/Scenes` と `Prefabs` と `Art` にサンプルが生成されます。生成すると章別Scene/Prefabが**上書き**されます。独自に改造するときは別名で保存してください。

**初回検証：** 1) Chapter01でプレイヤーが下側に見えること、2) Chapter02でWASD移動、3) Chapter05で弾を敵に当てる、4) Chapter09でHP→ゲームオーバー→Rのリロード、5) Chapter10で異なる敵が出ることを確認します。

## 第1章の本文で重要なこと

見本のChapter01をいきなりコピーするだけにせず、Unity Hubで新規2Dプロジェクトを作る操作、Scene/Game/Hierarchy/Inspector/Project/Console、Scene保存、Play中の変更が保存されないこと、PlayerのTransform調整を扱います。**第1章にC#の説明を詰め込まない**のが構成案の意図です。

## コードの読み方

`Editor/BookSceneBuilder.cs` はシーンとPrefabをまとめて用意する**教材制作側の補助コード**です。初学者が第1章で理解する必要はありません。本文にはScriptsフォルダの短い処理を必要な章で順番に抜き出してください。

## 留意点

- 全コードが最初からプロジェクトに入りますが、章別シーンに配置するコンポーネントを変えて段階的に解説します。
- Input Systemのみを使います。UnityのPlayer SettingsでActive Input Handlingを新Input Systemにしてください。
- 第11章の効果音アセット・タイトル画面や完成ゲームの演出素材、第13〜23章のGit操作用演習ブランチは、実機確認と本文執筆に合わせて拡充する予定です。
- `Packages/packages-lock.json` と生成された `.meta` は、**Unity Editorで生成した正しい内容をGitへ追加**します。手書きした架空のロックファイルは配置しません。
- このリポジトリのコードはまだUnity Editorで実行検証していません。UnityのConsoleにエラーがないか実機確認する必要があります。
