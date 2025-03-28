from typing import List

from .position import Position
from .position_int import PositionInt


class Wall:
    def __init__(self, position: PositionInt):
        self.position = position

    def __str__(self):
        return f"Wall(position={self.position})"


class Fence:
    def __init__(self, position: PositionInt, health: int):
        self.position = position
        self.health = health

    def __str__(self):
        return f"Fence(position={self.position}, health={self.health})"


class Bullet:
    def __init__(
        self,
        no: int,
        isMissile: bool,
        isAntiArmor: bool,
        position: Position,
        speed: float,
        damage: float,
        traveled_distance: float,
    ):
        self.no = no
        self.isMissile = isMissile
        self.isAntiArmor = isAntiArmor
        self.position = position
        self.speed = speed
        self.damage = damage
        self.traveled_distance = traveled_distance

    def __str__(self):
        return f"Bullet(position={self.position}, speed={self.speed}, damage={self.damage}, traveled_distance={self.traveled_distance})"


class EnvironmentInfo:
    def __init__(
        self,
        walls: List[Wall],
        fences: List[Fence],
        bullets: List[Bullet],
        map_size: int,
    ):
        self.walls = walls
        self.fences = fences
        self.bullets = bullets
        self.map_size = map_size

    def __str__(self):
        return f"EnvironmentInfo(walls={self.walls}, fences={self.fences}, bullets={self.bullets}, map_size={self.map_size})"
