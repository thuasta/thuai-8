import json
from enum import Enum

class RequestType(Enum):
    SELF = "SELF"
    OPPONENT = "OPPONENT"

class Position:
    def __init__(self, x, y):
        self.x = x
        self.y = y


class Message:
    def __init__(self, json_string: str = "{}"):
        try:
            self.msg = json.loads(json_string)
        except json.JSONDecodeError:
            self.msg = {}

    def json(self):
        try:
            return json.dumps(self.msg)
        except Exception:
            return "{}"


class MoveMessage(Message):
    def __init__(self, token: str,target_distance: float):
        super().__init__()
        self.msg["messageType"] = "PERFORM_MOVE"
        self.msg["token"] = token
        self.msg["distance"] = target_distance

class TurnMessage(Message):
    def __init__(self, token: str,target_angle: float):
        super().__init__()
        self.msg["messageType"] = "PERFORM_TURN"
        self.msg["token"] = token
        self.msg["angle"] = target_angle

class AttackMessage(Message):
    def __init__(self, token: str):
        super().__init__()
        self.msg["messageType"] = "PERFORM_ATTACK"
        self.msg["token"] = token

class UseSkillMessage(Message):
    def __init__(self, token: str, skill_name: str):
        super().__init__()
        self.msg["messageType"] = "PERFORM_USE_SKILL"
        self.msg["token"] = token
        self.msg["skillName"] = skill_name

class SelectSkillMessage(Message):
    def __init__(self, token: str, skill_name: str):
        super().__init__()
        self.msg["messageType"] = "SELECT_SKILL"
        self.msg["token"] = token
        self.msg["skillName"] = skill_name

class GetPlayerInfoMessage(Message):
    def __init__(self, token: str, request: RequestType):
        super().__init__()
        self.msg["messageType"] = "GET_PLAYER_INFO"
        self.msg["token"] = token
        self.msg["request"] = request.value


class GetEnvironmentInfoMessage(Message):
    def __init__(self, token: str):
        super().__init__()
        self.msg["messageType"] = "GET_ENVIRONMENT_INFO"
        self.msg["token"] = token

class GetGameStatisticsMessage(Message):
    def __init__(self, token: str):
        super().__init__()
        self.msg["messageType"] = "GET_GAME_STATISTICS"
        self.msg["token"] = token

class GetAvailableBuffsMessage(Message):
    def __init__(self, token: str):
        super().__init__()
        self.msg["messageType"] = "GET_AVAILABLE_SKILLS"
        self.msg["token"] = token
