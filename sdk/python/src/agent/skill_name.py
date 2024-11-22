from enum import Enum

class SkillName(Enum):
    BLACK_OUT = "BLACK_OUT"     # 视野限制
    SPEED_UP = "SPEED_UP"       # 加速
    FLASH = "FLASH"             # 闪现
    DESTROY = "DESTROY"         # 破坏墙体
    CONSTRUCT = "CONSTRUCT"     # 建造墙体
    TRAP = "TRAP"               # 陷阱
    MISSILE = "MISSILE"         # 导弹
    KAMUI = "KAMUI"             # 虚化

    def __str__(self):
        return self.value
