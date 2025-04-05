from enum import Enum


class SkillName(Enum):
    BLACK_OUT = "BLACK_OUT"
    SPEED_UP = "SPEED_UP"
    FLASH = "FLASH"
    DESTROY = "DESTROY"
    CONSTRUCT = "CONSTRUCT"
    TRAP = "TRAP"
    RECOVER = "RECOVER"
    KAMUI = "KAMUI"

    def __str__(self):
        return self.value
