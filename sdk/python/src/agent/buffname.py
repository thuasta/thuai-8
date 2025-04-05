from enum import Enum


class BuffName(Enum):
    BULLET_COUNT = "BULLET_COUNT"
    BULLET_SPEED = "BULLET_SPEED"
    ATTACK_SPEED = "ATTACK_SPEED"
    LASER = "LASER"
    DAMAGE = "DAMAGE"
    ANTI_ARMOR = "ANTI_ARMOR"
    ARMOR = "ARMOR"
    REFLECT = "REFLECT"
    DODGE = "DODGE"
    KNIFE = "KNIFE"
    GRAVITY = "GRAVITY"
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
