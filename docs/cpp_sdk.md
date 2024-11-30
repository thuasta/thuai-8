# THUAI8 C++ SDK 接口文档

欢迎使用 THUAI8 C++ SDK！本文档详细介绍了可供选手调用的接口和功能，包括参数、返回值类型以及用法示例，帮助您更好地理解如何使用该 SDK 来控制您的无人作战系统参与比赛。

仓库链接：[agent-template-cpp](https://github.com/thuasta/thuai-8/tree/main/sdk/cpp)

## 准备工作

使用 Windows 操作系统的选手可参考如下 [视频教程](https://cloud.tsinghua.edu.cn/f/9f18a58882614cbea368/) ，了解如何使用 C++ SDK 开发自己的 Agent。

### 环境要求

要求 XMake >= 2.9.6，安装方法请参考 [XMake 官方文档](https://xmake.io/#/zh-cn/guide/installation)。若您没有开启代理，可使用如下 Windows 下载地址：[XMake 2.9.6](https://cloud.tsinghua.edu.cn/f/c204dcdce21e43648edd/?dl=1)

具备 C++23 支持的 C++ 编译器工具链。推荐使用 MSVC。可使用如下 VS Build Tools 下载地址：[Visual Studio Build Tools](https://aka.ms/vs/17/release/vs_BuildTools.exe)。若您使用的是其他编译器，请确保其支持 C++23 标准。

合适的代码编辑器。推荐使用 Visual Studio Code。可使用如下 Windows 下载地址：[Visual Studio Code](https://code.visualstudio.com/sha/download?build=stable&os=win32-x64-user)

### 构建项目

若您没有开启代理，则需要更改 xmake 的 proxy，在命令行中输入：

```bash
xmake g --proxy_pac=github_mirror.pac
```

运行以下命令以配置项目：

```bash
xmake f -m debug
```

或者在发布模式下：

```bash
xmake f -m release
```

然后构建项目：

```bash
xmake
```

若您需要生成 compile_commands.json 文件，可以使用以下命令：

```bash
xmake project -k compile_commands
```

### 编写代码

您可以在 logic.cc 文件中编写您的代码。对于有经验的开发者，您也可以修改项目中的其他任何文件。在 logic.cc 中，我们已经为您提供了一个示例代码，您可以根据自己的需求进行修改。

### 运行

如果您修改了 main.cc 中 `ParseOptions(int argc, char** argv)` 方法的代码，则此部分可能无效。

运行以下命令启动 Agent：

```bash
./agent --server <server> --token <token>
```

\<server\>：游戏服务器地址。（默认值：ws://localhost:14514）

\<token\>：Agent 的令牌。（默认值：1919810）

例如：

```bash
./agent --server ws://localhost:14514 --token 1919810
```

注意： 运行前，请确保您的服务器地址和令牌是正确的，以确保 Agent 能够成功连接到游戏服务器。

## 接口介绍

### 获取信息

#### 获取自身玩家信息

```cpp
const auto& self_info{agent.self_info()};
```

- **返回类型：** `const PlayerInfo&`

`self_info` 方法将返回自身玩家信息。玩家信息包括玩家的 Token、位置、武器库、护甲库和技能库等。

#### 获取对方玩家信息

```cpp
const auto& opponent_info{agent.opponent_info()};
```

- **返回类型：** `const PlayerInfo&`

`opponent_info` 方法将返回对方玩家。玩家信息包括玩家的 Token、位置、武器库、护甲库和技能库等。

#### 获取环境信息

```cpp
const auto& environment_info{agent.environment_info()};
```

- **返回类型：** `const EnvironmentInfo&`

`environment_info` 方法将返回环境信息。环境信息包括墙体的列表、技能墙体的列表和当前子弹的列表。每个墙体信息包括墙体的位置。每个技能墙体信息包括技能墙体的位置和当前生命值。每个子弹信息包括子弹的位置、速度、伤害值和已经经过的距离。

#### 获取游戏状态信息

```cpp
const auto& game_statistics{agent.game_statistics()};
```

- **返回类型：** `const GameStatistics&`

`game_statistics` 方法将返回游戏状态信息。游戏状态信息包括游戏的当前阶段、游戏的倒计时、游戏当前的 ticks 和每位玩家的累计分数。

#### 获取资源信息

```cpp
const auto& available_buffs{agent.available_buffs()};
```

- **返回类型：** `const std::vector<BuffKind>&`

`agent.available_buffs` 方法将返回可选择的资源列表。

### 操作坦克

#### 向前移动

```cpp
agent.MoveForward();
```

- **返回类型：** 无

使用 `MoveForward` 方法使坦克向前移动。

#### 向后移动

```cpp
agent.MoveBackward();
```

- **返回类型：** 无

使用 `MoveBackward` 方法使坦克向后移动。

#### 顺时针旋转

```cpp
agent.TurnClockwise();
```

- **返回类型：** 无

使用 `TurnClockwise` 方法使坦克顺时针旋转。

#### 逆时针旋转

```cpp
agent.TurnCounterClockwise();
```

- **返回类型：** 无

使用 `TurnCounterClockwise` 方法使坦克逆时针旋转。

#### 攻击

```cpp
agent.Attack();
```

- **返回类型：** 无

使用 `Attack` 方法使坦克攻击。

#### 使用技能

```cpp
agent.UseSkill(SkillKind skill);
```

- **参数：**
  - `skill`：技能种类。
- **返回类型：** 无

使用 `UseSkill` 方法使用指定技能。

#### 选择资源

```cpp
agent.SelectBuff(BuffKind buff);
```

- **参数：**
  - `buff`：资源种类。
- **返回类型：** 无

使用 `SelectBuff` 方法选择指定的资源。

### 状态查询

#### 判断是否连接到服务器

```cpp
bool connected = agent.IsConnected();
```

- **返回类型：** bool

`IsConnected` 方法将返回一个布尔值，指示 Agent 是否已连接到服务器。

#### 判断游戏是否准备就绪

```cpp
bool ready = agent.IsGameReady();
```

- **返回类型：** bool

`IsGameReady` 方法将返回一个布尔值，指示游戏是否已准备就绪，即是否已获取到所有必要的游戏状态信息。
