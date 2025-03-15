class PositionInt:
    def __init__(self, x: int, y: int, angle: float):
        self.x = x
        self.y = y
        self.angle = angle

    def __str__(self):
        return f"Position(x={self.x}, y={self.y}, angle={self.angle})"
