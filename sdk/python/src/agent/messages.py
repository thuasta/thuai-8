import json


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

    def __init__(self, token: str, direction: str, distance: float):
        super().__init__()
        self.msg["messageType"] = "PERFORM_MOVE"
        self.msg["token"] = token
        self.msg["direction"] = direction
        self.msg["distance"] = distance


class TurnMessage(Message):
    def __init__(self, token: str, direction: str, angle: int):
        super().__init__()
        self.msg["messageType"] = "PERFORM_TURN"
        self.msg["token"] = token
        self.msg["direction"] = direction
        self.msg["angle"] = angle


class AttackMessage(Message):
    def __init__(self, token: str):
        super().__init__()
        self.msg["messageType"] = "PERFORM_ATTACK"
        self.msg["token"] = token


class UseSkillMessage(Message):
    def __init__(self, token: str, skill_name: str):
        super().__init__()
        self.msg["messageType"] = "PERFORM_SKILL"
        self.msg["token"] = token
        self.msg["skillName"] = skill_name


class SelectBuffMessage(Message):
    def __init__(self, token: str, buff_name: str):
        super().__init__()
        self.msg["messageType"] = "PERFORM_SELECT"
        self.msg["token"] = token
        self.msg["buffName"] = buff_name


class GetPlayerInfoMessage(Message):
    def __init__(self, token: str):
        super().__init__()
        self.msg["messageType"] = "GET_PLAYER_INFO"
        self.msg["token"] = token
        self.msg["request"] = "SELF"


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
        self.msg["messageType"] = "GET_AVAILABLE_BUFFS"
        self.msg["token"] = token
