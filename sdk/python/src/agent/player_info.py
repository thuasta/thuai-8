from typing import List, Literal

from .position import Position


class WeaponInfo:
    def __init__(
        self,
        attack_speed: float,
        bullet_speed: float,
        is_laser: bool,
        anti_armor: bool,
        damage: int,
        max_bullets: int,
        current_bullets: int,
    ):
        self.attack_speed = attack_speed
        self.bullet_speed = bullet_speed
        self.is_laser = is_laser
        self.anti_armor = anti_armor
        self.damage = damage
        self.max_bullets = max_bullets
        self.current_bullets = current_bullets

    def __str__(self):
        return (
            f"Weapon(attack_speed={self.attack_speed}, bullet_speed={self.bullet_speed}, "
            f"is_laser={self.is_laser}, anti_armor={self.anti_armor}, damage={self.damage}, "
            f"max_bullets={self.max_bullets}, current_bullets={self.current_bullets})"
        )


class ArmorInfo:
    def __init__(
        self,
        can_reflect: bool,
        armor_value: int,
        health: int,
        gravity_field: bool,
        knife: Literal["NOT_OWNED", "AVAILABLE", "ACTIVE", "BROKEN"],
        dodge_rate: float,
    ):
        self.can_reflect = can_reflect
        self.armor_value = armor_value
        self.health = health
        self.gravity_field = gravity_field
        self.knife = knife
        self.dodge_rate = dodge_rate

    def __str__(self):
        return (
            f"Armor(can_reflect={self.can_reflect}, armor_value={self.armor_value}, "
            f"health={self.health}, gravity_field={self.gravity_field}, knife={self.knife}, "
            f"dodge_rate={self.dodge_rate})"
        )


class SkillInfo:
    def __init__(
        self, name: str, max_cooldown: int, current_cooldown: int, is_active: bool
    ):
        self.name = name
        self.max_cooldown = max_cooldown
        self.current_cooldown = current_cooldown
        self.is_active = is_active

    def __str__(self):
        return (
            f"Skill(name={self.name}, max_cooldown={self.max_cooldown}, "
            f"current_cooldown={self.current_cooldown}, is_active={self.is_active})"
        )


class PlayerInfo:
    def __init__(
        self,
        token: str,
        weapon: WeaponInfo,
        armor: ArmorInfo,
        skills: List[SkillInfo],
        position: Position,
    ):
        self.token = token
        self.weapon = weapon
        self.armor = armor
        self.skills = skills
        self.position = position

    def __str__(self) -> str:
        return f"PlayerInfo{{token: {self.token}, weapon: {self.weapon}, armor: {self.armor}, skill: {self.skills}, position: {self.position}}}"
