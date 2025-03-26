from typing import List
from .stage import Stage


class Score:
    def __init__(self, token: str, score: int):
        self.token = token
        self.score = score

    def __str__(self):
        return f"Score(token={self.token}, score={self.score})"


class GameStatistics:
    def __init__(
        self, current_stage: Stage, count_down: int, ticks: int, scores: List[Score]
    ):
        self.current_stage = current_stage
        self.count_down = count_down
        self.ticks = ticks
        self.scores = scores

    def __str__(self):
        scores_str = ", ".join(str(score) for score in self.scores)
        return f"GameStatistics(current_stage={self.current_stage}, count_down={self.count_down}, ticks={self.ticks}, scores=[{scores_str}])"
