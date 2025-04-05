# THUAI8 Python SDK 接口文档

欢迎使用我们为比赛编写的 Python SDK！本文档将详细介绍可供选手调用的接口和功能，包括参数、返回值类型以及用法示例，帮助您更好地理解如何使用该 SDK 来控制您的无人作战系统参与比赛。

仓库链接：[agent-template-python](https://github.com/thuasta/thuai-8/tree/main/sdk/python)

## 模版说明

版本要求：**Python 3.11**

首先请安装依赖项，需要在命令行中输入：

```powershell
pip install -r requirements.txt
```

你可以在 **logic.py** 中编写你的代码，**main.py**会调用 `setup()`和 `loop()`函数以运行你的代码。对于有经验的开发者，你也可以修改项目中的其他任何文件。在命令行中运行以下命令来启动 agent：

```powershell
python main.py --server <server> --token <token>
```

<server\>：游戏的服务器地址。（默认值：ws://localhost:14514）

\<token>：agent的令牌。（默认值：1919810）

例如：

```PowerShell
python main.py --server ws://localhost:14514 --token 1919810
```

请注意：运行此模板前，请确保你的环境满足Python版本要求，并已正确安装所有依赖项。如果修改了模板内容，可能需要相应地调整运行命令或代码逻辑。在运行前，请务必检查你的服务器地址和令牌是否正确，以确保agent能够成功连接到游戏服务器。

## 接口介绍

### 获取游戏状态信息

#### 获取玩家信息

```python
players_info = agent.all_player_info
```

* 返回类型：Optional[List[PlayerInfo]]

`all_player_info` 属性将返回一个包含所有玩家信息的列表（包括自己的和对手的）。每个玩家信息包括玩家的 Token、武器、护甲、技能、位置等。

* 武器 `weapon`包括每种武器的拥有情况
* 护甲 `armor`包括护甲和护甲类技能的基本情况
* 技能 `skill`包括所拥有的技能类别、使用时间等等
* 位置 `position`包括所在地的坐标和坦克朝向

#### 获取环境信息

```python
environment_info = agent.environment_info
```

* 返回类型：Optional[EnvironmentInfo]

`environment_info`属性将返回环境相关的信息，包括所有普通墙体的列表、特殊技能墙体的列表、现存子弹的列表、玩家位置列表。

* 普通墙体 `walls`包括每一处墙体的坐标和朝向
* 特殊技能墙体 `fences`包括每一处墙体的坐标、朝向以及当前生命值
* 子弹 `bullet`包括当前场上每一个子弹的位置、前进方向、速度、伤害大小以及已经飞行的距离
* 玩家位置 `player_positions`包括每一个玩家的token、当前位置坐标、前进方向

#### 获取游戏状态信息

```python
game_statistics = agent.game_statistics
```

* 返回类型：Optional[GameStatistics]

`game_statistics`属性将返回游戏状态相关信息，包括当前游戏阶段、倒计时、ticks数以及一个得分列表。

* 当前游戏阶段 `current_stage`表示当前游戏进行到哪一个阶段
* 倒计时 `count_down`表示离这一小局比赛结束还有多长时间
* `ticks`记录游戏总体的进行情况
* 得分 `scores`包含每个玩家的token以及对应的分数

#### 获取资源信息

```python
availiable_buff = agent.availiable_buff
```

* 返回类型：Optional[AvailableBuffs]

`availiable_buffs`属性将返回一个包含所有可获取资源的列表。

#### 获取玩家令牌

```python
token = agent.token
```

* 返回类型：str

`token`属性将返回玩家自身的token值，可用于读取自己的状态信息。

### 操作坦克

#### 向前移动

```python
agent.move_forward(distance: float = 1.0)
```

* 参数类型：distance: float - 需要移动的距离，默认为1.0
* 返回类型：None

使用`move_forward`方法让坦克向前移动指定的距离。

#### 向后移动

```python
agent.move_backward(distance: float = 1.0)
```

* 参数类型：distance: float - 需要移动的距离，默认为1.0
* 返回类型：None

使用`move_backward`方法让坦克向后移动指定的距离。

#### 顺时针转动

```python
agent.turn_clockwise(angle: int = 45)
```

* 参数类型：angle: int - 转动的角度，默认为45度
* 返回类型：None

使用`turn_clockwise`方法让坦克顺时针转动指定的角度。

#### 逆时针转动

```python
agent.turn_counterclockwise(angle: int = 45)
```

* 参数类型：angle: int - 转动的角度，默认为45度
* 返回类型：None

使用`turn_counterclockwise`方法让坦克逆时针转动指定的角度。

#### 攻击

```python
agent.attack()
```

* 参数类型：None
* 返回类型：None

使用`attack`方法让坦克发射子弹。

#### 使用技能

```python
agent.use_skill(skill: SkillName)
```

* 参数类型：skill: SkillName - 技能的种类
* 返回类型：None

使用`use_skill`方法让坦克使用某一技能。

#### 选择资源

```python
agent.select_buff(buff: BuffName)
```

* 参数类型：buff: BuffName - 资源的种类
* 返回类型：None

使用 `select_buff`方法为坦克选择某一资源。
