import asyncio
import logging
from typing import List, Optional

from . import messages
from .enviroment_info import EnvironmentInfo, Wall, Fence, Bullet, PlayerPosition
from .player_info import PlayerInfo, WeaponInfo, ArmorInfo, SkillInfo
from .position import Position
from .position_int import PositionInt
from .game_statistics import GameStatistics, Score
from .stage import Stage
from .skill_name import SkillName
from .buffname import BuffName
from .available_buffs import AvailableBuffs
from .websocket_client import WebsocketClient

class Agent:
    def __init__(self, token: str, loop_interval: float):
        self._token = token
        self._loop_interval = loop_interval
        self._all_player_info: Optional[List[PlayerInfo]] = None
        self._environment_info: Optional[EnvironmentInfo] = None
        self._game_statistics: Optional[GameStatistics] = None
        self._availiable_buff: Optional[AvailableBuffs] = None
        self._ws_client = WebsocketClient()
        self._loop_task: Optional[asyncio.Task] = None
        self._ws_client.on_message = self._on_message

    def __str__(self) -> str:
        return f"Agent{{token: {self._token}}}"

    def __repr__(self) -> str:
        return str(self)
# Information received by the player
    @property
    def all_player_info(self) -> Optional[List[PlayerInfo]]:
        return self._all_player_info

    @property
    def environment_info(self) -> Optional[EnvironmentInfo]:
        return self._environment_info

    @property
    def game_statistics(self) -> Optional[GameStatistics]:
        return self._game_statistics

    @property
    def availiable_buff(self) -> Optional[AvailableBuffs]:
        return self._availiable_buff

    @property
    def token(self) -> str:
        return self._token

    async def connect(self, server: str):
        await self._ws_client.connect(server)
        self._loop_task = asyncio.create_task(self._loop())

    async def disconnect(self):
        if self._loop_task is not None:
            self._loop_task.cancel()
        await self._ws_client.disconnect()

    def is_connected(self) -> bool:
        return self._ws_client.is_connected()
# Internal function to check if the game is ready
    def is_game_ready(self) -> bool:
        return (
            self._all_player_info is not None
            and self._environment_info is not None
            and self._game_statistics is not None
            and self._availiable_buff is not None
            and self._self_id is not None
        )
    # direction is distinguished by the sign of the distance
    async def move(self,distance:float):
        await self._ws_client.send(
            messages.MoveMessage(
                token=self._token,
                target_distance=distance,
            )
        )
    # angle is in degrees, positive for clockwise, negative for counterclockwise
    async def turn(self, angle: float):
        await self._ws_client.send(
            messages.TurnMessage(
                token=self._token,
                target_angle=angle,
            )
        )

    async def attack(self):
        await self._ws_client.send(
            messages.AttackMessage(
                token=self._token
            )
        )

    async def use_skill(self, skill: SkillName):
        await self._ws_client.send(
            messages.UseSkillMessage(
                token=self._token,
                skill_name=skill
            )
        )

    async def select_skill(self, skill: SkillName):
        await self._ws_client.send(
            messages.SelectSkillMessage(
                token=self._token,
                skill_name=skill
            )
        )
    
    async def get_player_info(self):
        await self._ws_client.send(
            messages.GetPlayerInfoMessage(
                token=self._token,
            )
        )
    
    async def get_environment_info(self):
        await self._ws_client.send(
            messages.GetEnvironmentInfoMessage(
                token=self._token,
            )
        )
    
    async def get_game_statistics(self):
        await self._ws_client.send(
            messages.GetGameStatisticsMessage(
                token=self._token,
            )
        )
    
    async def get_available_buff(self):
        await self._ws_client.send(
            messages.GetAvailableBuffsMessage(
                token=self._token,
            )
        )

    async def _loop(self):
        while True:
            try:
                await asyncio.sleep(self._loop_interval)

                if not self._ws_client.is_connected():
                    continue

                await self._ws_client.send(
                    messages.GetPlayerInfoMessage(
                        token=self._token,
                    )
                )

                await self._ws_client.send(
                    messages.GetEnvironmentInfoMessage(
                        token=self._token,
                    )
                )


            except Exception as e:
                logging.error(f"{self} encountered an error in loop: {e}")


    def _on_message(self, message: messages.Message):
        try:
            msg_dict = message.msg
            msg_type = msg_dict["messageType"]

            if msg_type == "ERROR":
                logging.error(f"{self} got error from server: {msg_dict['message']}")

            elif msg_type == "PLAYERS_INFO":
                self._all_player_info = [
                    # Extract player information from the data received from the server
                    PlayerInfo(
                        token=data["token"],
                        weapon=WeaponInfo(
                            attack_speed=data["weapon"]["attackSpeed"],
                            bullet_speed=data["weapon"]["bulletSpeed"],
                            is_laser=data["weapon"]["isLaser"],
                            anti_armor=data["weapon"]["antiArmor"],
                            damage=data["weapon"]["damage"],
                            max_bullets=data["weapon"]["maxBullets"],
                            current_bullets=data["weapon"]["currentBullets"]
                        ),
                        armor=ArmorInfo(
                            can_reflect=data["armor"]["canReflect"],
                            armor_value=data["armor"]["armorValue"],
                            health=data["armor"]["health"],
                            gravity_field=data["armor"]["gravityField"],
                            knife=data["armor"]["knife"],
                            dodge_rate=data["armor"]["dodgeRate"]
                        ),
                        skill=SkillInfo(
                            name=data["skill"]["name"],
                            max_cooldown=data["skill"]["maxCooldown"],
                            current_cooldown=data["skill"]["currentCooldown"],
                            is_active=data["skill"]["isActive"]
                        ),
                        position=Position(
                            x=data["position"]["x"],
                            y=data["position"]["y"],
                            angle=data["position"]["angle"]
                        )
                    )
                    for data in msg_dict["players"]
                ]
            elif msg_type == "EnvironmentInfo":
                self._environment_info = EnvironmentInfo(
                    walls=[
                        Wall(PositionInt(
                        x=wall["x"],
                        y=wall["y"],
                        angle=wall["angle"]
                    ))
                    for wall in msg_dict["walls"]
                ],
                fences=[
                    Fence(
                        position=PositionInt(
                            x=fence["position"]["x"],
                            y=fence["position"]["y"],
                            angle=fence["position"]["angle"]
                        ),
                        health=fence["health"]
                    )
                    for fence in msg_dict["fences"]
                ],
                bullets=[
                    Bullet(
                        position=Position(
                            x=bullet["position"]["x"],
                            y=bullet["position"]["y"],
                            angle=bullet["position"]["angle"]
                        ),
                    speed=bullet["speed"],
                    damage=bullet["damage"],
                    traveled_distance=bullet["traveledDistance"]
                    )
                    for bullet in msg_dict["bullets"]
                ],
                player_positions=[
                    PlayerPosition(
                        token=player["token"],
                        position=Position(
                            x=player["position"]["x"],
                            y=player["position"]["y"],
                            angle=player["position"]["angle"]
                        )
                    )
                    for player in msg_dict["playerPositions"]
                ]
                )

            elif msg_type == "GameStatistics":
                self._game_statistics = GameStatistics(
                    current_stage=Stage(msg_dict["currentStage"]),
                    count_down=msg_dict["countDown"],
                    ticks=msg_dict["ticks"],
                    scores=[
                        Score(
                            token=score["token"],
                            score=score["score"]
                        )
                    for score in msg_dict["scores"]
                    ]
                )


            elif msg_type == "AvailableBuffs":
                self._available_buffs = AvailableBuffs(
                    buffs=[
                        BuffName(buff)
                        for buff in msg_dict["buffs"]
                    ]
                )
        except Exception as e:
            logging.error(f"{self} failed to handle message: {e}")
