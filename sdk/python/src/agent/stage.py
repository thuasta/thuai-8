from enum import Enum


class Stage(Enum):
    REST = "REST"
    BATTLE = "BATTLE"
    END = "END"

    def __str__(self):
        return self.value
